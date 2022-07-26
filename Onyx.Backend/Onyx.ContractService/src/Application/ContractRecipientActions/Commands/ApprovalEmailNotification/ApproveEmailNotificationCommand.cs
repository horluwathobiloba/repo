using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Commands.ApprovalEmailNotification
{
    public class ApprovalEmailNotificationCommand : IRequest<Result>
    {

    }

    public class ApprovalEmailNotificationCommandHandler : IRequestHandler<ApprovalEmailNotificationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IReminderScheduleService _reminderScheduleService;
        public ApprovalEmailNotificationCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, IAuthService authService,
                                                      IConfiguration configuration, IEmailService emailService, IReminderScheduleService reminderScheduleService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
            _reminderScheduleService = reminderScheduleService;
        }
        public async Task<Result> Handle(ApprovalEmailNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //start at 3pm
                if (DateTime.Now.Hour == 15)
                {
                    var contracts = await _context.Contracts.Where(a => a.Status == Status.Active && a.ContractStatus == ContractStatus.PendingApproval).ToListAsync();
                    var reminderConfigurations = await _context.ReminderConfigurations.Where(a => a.Status == Status.Active && a.ReminderType == ReminderType.Request).ToListAsync();
                    var users = await _authService.GetAllUsersAsync();
                    if (contracts == null || contracts.Count == 0)
                    {
                        return Result.Failure($"No Valid Contracts on the system!");
                    }
                    if (reminderConfigurations == null || reminderConfigurations.Count == 0)
                    {
                        return Result.Failure($"No Reminder Configurations on the system!");
                    }
                    string loginPage = _configuration["LoginPage"];
                    var inboxes = new List<Inbox>();
                    var emailList = new List<EmailVm>();
                    var reminderConfigsForUpdate = new List<Domain.Entities.ReminderConfiguration>();
                    foreach (var contract in contracts)
                    {
                        List<string> emailRecipients = new List<string>();
                        var approver = contract.NextActorEmail;
                        var reminderConfiguration = reminderConfigurations.Where(a => a.OrganisationId == contract.OrganisationId).ToList();
                        if (reminderConfiguration != null && reminderConfiguration.Count > 0)
                        {
                            var user = users.Entity.FirstOrDefault(a => a.Email == approver);
                            //send email to recipients
                            var result = await SendEmailBasedOnReminder(reminderConfiguration, contract, loginPage, user, _reminderScheduleService);
                            if (result.result.Succeeded)
                            {
                                if (result.sendEmail)
                                {
                                    var setupEmail = SetupApprovalEmailAndInbox(contract, approver, loginPage, _reminderScheduleService, user);
                                    if (setupEmail.result.Succeeded)
                                    {
                                        reminderConfigsForUpdate.Add(result.config);
                                        emailList.Add(setupEmail.email);
                                        inboxes.Add(setupEmail.inbox);
                                    }
                                }

                            }
                        }

                    }

                    if (emailList != null && emailList.Count > 0)
                    {
                        await _emailService.SendBulkEmail(emailList);
                    }
                    if (inboxes != null && inboxes.Count > 0)
                    {
                        await _context.Inboxes.AddRangeAsync(inboxes);

                    }
                    //after service runs, deplete the recurring count balance
                        if (reminderConfigsForUpdate != null && reminderConfigsForUpdate.Count > 0)
                        {
                            foreach (var config in reminderConfigsForUpdate)
                            {
                                config.RecurringCountBalance = config.RecurringCountBalance - 1;
                            }
                            _context.ReminderConfigurations.UpdateRange(reminderConfigsForUpdate);
                        }
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success($"{emailList.Count} emails sent -Contract Approval Email Notification successful"); 
                }
                else
                {
                    return Result.Success($"Not yet time to send contract email notification");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract Approval Email Notification failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        private (Result result, EmailVm email, Inbox inbox) SetupApprovalEmailAndInbox(Domain.Entities.Contract contract, string approver, 
                                                                        string loginPage, IReminderScheduleService _reminderScheduleService, UserDto user)
        {

            try
            {
                List<EmailVm> emailList = new List<EmailVm>();
                var inboxes = new List<Inbox>();
                string emailMessage = $"You have a pending document initiation request for you to approve. Kindly log into the system to approve !";
                if (approver != null)
                {
                    emailList.Add(new EmailVm
                    {
                        Subject = "Contract Approval for -" + contract.Name,
                        Text = emailMessage,
                        RecipientEmail = approver,
                        ButtonText = "Approve Request Now!",
                        ButtonLink = loginPage
                    });
                    var result = CreateEmailAndInboxList(contract, loginPage, approver, user, emailMessage);
                    return (Result.Success(),result.email, result.inbox);
                }
                return (Result.Failure("Invalid Approver"), null,null);
            }
            catch (Exception ex)
            {
                return (Result.Failure(ex?.Message +" "+ ex?.InnerException?.Message), null, null);
            }
        }
        private async Task<(Result result, bool sendEmail,Domain.Entities.ReminderConfiguration config)> SendEmailBasedOnReminder(List<Domain.Entities.ReminderConfiguration> configurations,
                                            Domain.Entities.Contract contract, string loginPage ,UserDto user, IReminderScheduleService _reminderScheduleService)
        {
            bool sendEmail = false;
            var reminderSchedule = new Domain.Entities.ReminderConfiguration();
            foreach (var config in configurations)
            {
                
                    if ((config.EndDate.DaysBetween(config.NextRecurringPeriod) < 0) || (config.EndDate.DaysBetween(DateTime.Now) > 0) )
                    {
                        continue;
                    }
                    switch (config.ReminderScheduleFrequency)
                    {
                        case ReminderScheduleFrequency.Daily:
                            if ((DateTime.Now.Subtract(config.StartDate).TotalDays == 0) || config.EndDate.Date >= DateTime.Now.Date)
                            {
                                if (config.RecurringCountBalance > 0)
                                {
                                    //if (config.RecurringCountBalance == 0)
                                    //{
                                    //    continue;
                                    //}
                                    //config.RecurringCountBalance -= 1;
                                    reminderSchedule = await _reminderScheduleService.ComputeReminderSchedule(config);
                                    sendEmail = true;
                                }

                            }
                            break;
                        case ReminderScheduleFrequency.Weekly:
                            if (config.StartDate == DateTime.Now)
                            {
                                if (config.RecurringCountBalance > 0)
                                {
                                    //if (config.RecurringCountBalance == 0)
                                    //{
                                    //    continue;
                                    //}
                                    //config.RecurringCountBalance -= 1;
                                    reminderSchedule = await _reminderScheduleService.ComputeReminderSchedule(config);
                                    sendEmail = true;
                                }

                            }
                            break;
                        case ReminderScheduleFrequency.Monthly:
                            if (config.StartDate == DateTime.Now)
                            {
                                if (config.RecurringCountBalance > 0)
                                {
                                    //if (config.RecurringCountBalance == 0)
                                    //{
                                    //    continue;
                                    //}

                                    //config.RecurringCountBalance -= 1;
                                    reminderSchedule = await _reminderScheduleService.ComputeReminderSchedule(config);
                                    sendEmail = true;
                                }

                            }
                            break;
                        case ReminderScheduleFrequency.Yearly:
                            if (config.StartDate == DateTime.Now)
                            {
                                if (config.RecurringCountBalance > 0)
                                {
                                    //if (config.RecurringCountBalance == 0)
                                    //{
                                    //    continue;
                                    //}
                                   // config.RecurringCountBalance -= 1;
                                    reminderSchedule = await _reminderScheduleService.ComputeReminderSchedule(config);
                                    sendEmail = true;
                                }
                            }
                            break;
                        default:
                            break; 
                    

                }
            }
            return (Result.Success(),sendEmail, reminderSchedule);
        }
        private static (EmailVm email, Inbox inbox) CreateEmailAndInboxList(Domain.Entities.Contract contract, string loginPage,
                                                             string approver, UserDto userDetails, string emailMessage)
        {
            var email = new EmailVm
            {
                Subject = "Contract Approval for -" + contract.Name,
                Text = emailMessage,
                RecipientEmail = approver,
                ButtonText ="Approve Contract" ,
                ButtonLink =  loginPage ,
                RecipientName = approver
            };
            var inbox = new Inbox();
            if (userDetails != null)
            {
                inbox = new Inbox
                {
                    Name = "Contract Approval for -" + contract.Name,
                    ReciepientEmail = approver,
                    Body = emailMessage,
                    EmailAction = EmailAction.Received,
                    Delivered = true,
                    OrganisationId = userDetails.OrganisationId,
                    CreatedBy = userDetails.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = userDetails.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    Read = false,
                    StatusDesc = Status.Active.ToString(),
                    Sender = userDetails.FirstName + " " + userDetails.LastName
                };
            }
            return (email, inbox);
        }
    }
}
