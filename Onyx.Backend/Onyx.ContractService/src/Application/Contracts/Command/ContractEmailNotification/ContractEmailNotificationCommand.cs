using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models; 
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

namespace Onyx.ContractService.Application.Contracts.Commands.ContractEmailNotification
{
    public class ContractEmailNotificationCommand : IRequest<Result>
    {

    }

    public class ContractEmailNotificationCommandHandler : IRequestHandler<ContractEmailNotificationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public ContractEmailNotificationCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService,IAuthService authService, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
        }
        public async Task<Result> Handle(ContractEmailNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contracts = await _context.Contracts.Where(a=>a.Status == Status.Active && a.ContractStatus == ContractStatus.Active && a.ContractExpirationDate.HasValue).ToListAsync();
                var users = await _authService.GetAllUsersAsync();

                if (contracts == null || contracts.Count == 0)
                {
                    return Result.Failure($"Invalid Contracts specified!");
                }
                var reminderConfigurations = await _context.ReminderConfigurations.Where(a => a.Status == Status.Active && a.ReminderType == ReminderType.Expiries).ToListAsync();
                string loginPage = _configuration["LoginPage"];
                //get all contract recipients on the system, dictionary makes it faster
                var allContractRecipients = await _context.ContractRecipients.Where(a => a.Status == Status.Active).ToDictionaryAsync(a => a.Id);
                var allReminderRecipients = await _context.ReminderRecipients.ToDictionaryAsync(a => a.Id);
                var expiredContracts = new List<Domain.Entities.Contract>();
                var inboxes = new List<Inbox>();
                var emailList = new List<EmailVm>();
                await _context.BeginTransactionAsync();
                foreach (var contract in contracts)
                {
                    List<string> emailRecipients = new List<string>();
                    emailRecipients.Add(contract.CreatedByEmail);
                    var reminderRecipients=allReminderRecipients.Where(a => a.Value?.ContractId == contract.Id && a.Value?.Status == Status.Active).Select(a => a.Value?.Email).ToList();
                    emailRecipients.AddRange(reminderRecipients);
                    var contractRecipients = allContractRecipients.Where(a => a.Value?.ContractId == contract.Id && a.Value?.Status == Status.Active).Select(a => a.Value?.Email).ToList();
                    emailRecipients.AddRange(contractRecipients);
                    if (DateTime.Now.DaysBetween(contract.ContractExpirationDate.Value) <=0)
                    {
                        contract.ContractStatus = ContractStatus.Expired;
                        contract.ContractStatusDesc = ContractStatus.Expired.ToString();
                        expiredContracts.Add(contract);
                        continue;
                    }
                    //send email to recipients
                    var result = SendContractExpirationEmail(contract, emailRecipients,loginPage,request,_authService,users);
                    emailList.AddRange(result.emailList);
                    inboxes.AddRange(result.inboxes);
                }
                if (expiredContracts != null && expiredContracts.Count > 0)
                {
                    _context.Contracts.UpdateRange(expiredContracts);
                }
                if (emailList != null && emailList.Count > 0)
                {
                     await _emailService.SendBulkEmail(emailList);
                }
                if (inboxes != null && inboxes.Count > 0)
                {
                    await _context.Inboxes.AddRangeAsync(inboxes);
                }
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success($"{emailList.Count} emails sent - Contract Expiry Email Notification successful");
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Contract Expiry Email Notification failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        private (Result result, List<EmailVm> emailList, List<Inbox> inboxes) SendContractExpirationEmail(Domain.Entities.Contract contract,List<string> recipients, string loginPage,
            ContractEmailNotificationCommand request,IAuthService authService, UserListVm userList)
        {

            try
            {
                List<EmailVm> emailList = new List<EmailVm>();
                var inboxes = new List<Inbox>();
                string expiryTime = "";
                int daysBetween = 0;
                var expirationMonthValue = contract.ContractExpirationDate.Value.Month;
                var expirationYearValue = contract.ContractExpirationDate.Value.Year;
                if (expirationMonthValue == DateTime.Now.Month + 2)
                {
                    if (expirationYearValue == DateTime.Now.Year)
                    {
                        expiryTime = 3 + " month(s)";
                    }
                }
                if (expirationMonthValue == DateTime.Now.Month + 1)
                {
                    if (expirationYearValue == DateTime.Now.Year)
                    {
                        expiryTime = 2 + " month(s)";
                    }
                      
                }
                if (expirationMonthValue <= DateTime.Now.Month)
                {
                    daysBetween = DateTime.Now.DaysBetween(contract.ContractExpirationDate.Value);
                    if (expirationYearValue == DateTime.Now.Year)
                    {
                        if (daysBetween == 30 || daysBetween == 31 || daysBetween == 28 || daysBetween == 29)
                        {
                            expiryTime = 1 + " month(s)";
                        }
                        if (daysBetween > 0 && daysBetween <= 7)
                        {
                            expiryTime = daysBetween.ToString() + " day(s)";
                        }
                        
                    }
                }
               
                if (!string.IsNullOrWhiteSpace(expiryTime))
                {
                    foreach (var recipient in recipients) //Send email to all the contract recipients
                    {
                        if (recipient != null)
                        {
                            var userDetails = userList.Entity.FirstOrDefault(a => a.Email == recipient);
                            var emailMessage = $"Kindly note that the contract document '{contract.Name}' would expire in {expiryTime}";
                            if (daysBetween > 7)
                            {
                                emailMessage = $"Kindly note that the contract document has expired";
                            }
                            var result = CreateEmailAndInboxList(contract, loginPage, recipient, userDetails, emailMessage);
                            emailList.Add(result.email);
                            inboxes.Add(result.inbox);
                        }

                    } 
                }

                return (Result.Success(),emailList, inboxes);
            }
            catch (Exception ex)
            {
                return (Result.Failure(ex?.Message +" "+ ex?.InnerException?.Message), null, null);
            }
        }

        private static (EmailVm email, Inbox inbox) CreateEmailAndInboxList(Domain.Entities.Contract contract, string loginPage,
                                                      string recipient, UserDto userDetails, string emailMessage)
        {
            var email = new EmailVm
            {
                Subject = "Contract Expiration for -" + contract.Name,
                Text = emailMessage,
                RecipientEmail = recipient,
                ButtonText = (contract.CreatedByEmail == recipient) ? "Renew Contract Now!" : "",
                ButtonLink = contract.CreatedByEmail == recipient ? loginPage : "",
                RecipientName = recipient
            };
            var inbox = new Inbox();
            if (userDetails != null)
            {
                inbox = new Inbox
                {
                    Name = "Contract Expiration  -" + contract.Name,
                    ReciepientEmail = recipient,
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
