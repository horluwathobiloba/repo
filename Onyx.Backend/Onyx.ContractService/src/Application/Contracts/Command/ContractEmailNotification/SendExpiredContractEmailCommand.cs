using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Command.ContractEmailNotification
{
    public class SendExpiredContractEmailCommand:IRequest<Result>
    {
    }

    public class SendExpiredContractEmailCommandHandler : IRequestHandler<SendExpiredContractEmailCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public SendExpiredContractEmailCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, IAuthService authService, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
        }
        public async Task<Result> Handle(SendExpiredContractEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contracts = await _context.Contracts.Where(a => a.Status == Status.Active && a.ContractStatus == ContractStatus.Active).ToListAsync();
                var contractEmailInfos = await GetContractEmailInfos(contracts);
                //SetExpiryTime(contractEmailInfos);
                string loginPage = _configuration["LoginPage"];
                var emailList = GetEmailMessages(contractEmailInfos, loginPage);
                if (emailList == null)
                {
                    return Result.Success("No emails to send");
                }
                await _emailService.SendBulkEmail(emailList);
                return Result.Success($"{emailList.Count} emails sent - Expired Contract Email Notification successful");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Expired Contract Email Notification failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
           
            }
            
        }

        internal async Task<List<EmailRecipient>> GetContractEmailInfos(List<Domain.Entities.Contract> contracts)
        {
          //  var emailInfos = new List<ContractEmailInfo>();
            var reciepeintEmails = new List<EmailRecipient>();
            var allContractRecipients = await _context.ContractRecipients.Where(a => a.Status == Status.Active).Include(x => x.Contract).ToDictionaryAsync(a => a.Id);
            var allReminderRecipients = await _context.ReminderRecipients.Include(x=>x.Contract).ToDictionaryAsync(a => a.Id);
            foreach (var item in allContractRecipients)
            {
                reciepeintEmails.Add(new EmailRecipient
                {
                    RecipientEmail = item.Value.Email,
                    Contract = item.Value.Contract
                });
            }
            foreach (var item in allReminderRecipients)
            {
                reciepeintEmails.Add(new EmailRecipient
                {
                    RecipientEmail = item.Value.Email,
                    Contract = item.Value.Contract
                });
            }
            foreach (var item in reciepeintEmails)
            {
                item.ExpiryMonthValue = item.Contract.ContractExpirationDate.Value.Month;
                item.ExpiryYearValue = item.Contract.ContractExpirationDate.Value.Year;
            }
            SetExpiryTime(reciepeintEmails);
               //var emails = new List<string>();
           
            return reciepeintEmails;
        }
        internal void SetExpiryTime(List<EmailRecipient> contractEmailInfos)
        {
            var expiredContracts = new List<Domain.Entities.Contract>();
            foreach (var item in contractEmailInfos)
            {
                string expiryTime = "";
                int daysBetween = 0;
                var expirationMonthValue = item.ExpiryMonthValue;
                var expirationYearValue = item.ExpiryYearValue;
                if (expirationMonthValue == DateTime.Now.Month + 2)
                {
                    if (expirationYearValue == DateTime.Now.Year)
                    {
                        expiryTime = 3 + " month(s)";
                        item.ExpiryTime = expiryTime;
                    }
                }
                if (expirationMonthValue == DateTime.Now.Month + 1)
                {
                    if (expirationYearValue == DateTime.Now.Year)
                    {
                        expiryTime = 2 + " month(s)";
                        item.ExpiryTime = expiryTime;
                    }

                }
                if (expirationMonthValue <= DateTime.Now.Month)
                {
                    daysBetween = DateTime.Now.DaysBetween(item.Contract.ContractExpirationDate.Value);
                    item.DaysBetween = daysBetween;
                    if (expirationYearValue == DateTime.Now.Year)
                    {
                        if (daysBetween == 30 || daysBetween == 31 || daysBetween == 28 || daysBetween == 29)
                        {
                            expiryTime = 1 + " month(s)";
                            item.ExpiryTime = expiryTime;
                        }
                        if (daysBetween > 0 && daysBetween <= 7)
                        {
                            expiryTime = daysBetween.ToString() + " day(s)";
                            item.ExpiryTime = expiryTime;
                        }
                        else
                        {
                            item.Contract.ContractStatus = ContractStatus.Expired;
                            item.Contract.ContractStatusDesc = ContractStatus.Expired.ToString();
                            expiredContracts.Add(item.Contract);
                        }
                    }

                }
            }

        }


        //internal async Task<List<EmailVm>>Create
        //private static void CreateEmailMessage(Domain.Entities.Contract contract, string loginPage, List<EmailVm> list, string recipient, string expiryTime, int daysBetween = 0)
        //{
        //    var emailMessage = $"Kindly note that the contract document '{contract.Name}' would expire in {expiryTime}";
        //    if (daysBetween > 7)
        //    {
        //        emailMessage = $"Kindly note that the contract document has expired";
        //    }
        //    list.Add(new EmailVm
        //    {
        //        Subject = "Contract Expiration for -" + contract.Name,
        //        Text = emailMessage,
        //        RecipientEmail = recipient,
        //        ButtonText = (contract.CreatedByEmail == recipient) ? "Renew Contract Now!" : "",
        //        ButtonLink = contract.CreatedByEmail == recipient ? loginPage : "",
        //        RecipientName = recipient
        //    });
        //}
        private static List<EmailVm> GetEmailMessages(List<EmailRecipient> emailRecipients, string loginPage)
        {
            var list = new List<EmailVm>();
            var recipients = new List<string>();
           // var emailRecipients =new  List<EmailRecipient>();
            foreach (var item in emailRecipients)
            {
              
                var emailMessage=item.DaysBetween>7 ? $"Kindly note that the contract document has expired" : $"Kindly note that the contract document '{item.Contract.Name}' would expire in {item.ExpiryTime}";
              
                list.Add(new EmailVm
                {
                    Subject = "Contract Expiration for -" + item.Contract.Name,
                    Text = emailMessage,
                    RecipientEmail = item.RecipientEmail,
                    ButtonText = (item.Contract.CreatedByEmail == item.RecipientEmail) ? "Renew Contract Now!" : "",
                    ButtonLink = item.Contract.CreatedByEmail == item.RecipientEmail ? loginPage : "",
                    RecipientName = item.RecipientEmail
                });
            }

         
            return list;

           
        }

    }
}
