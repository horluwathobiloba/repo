using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments;
using OnyxDoc.DocumentService.Domain.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Documents.Commands.SendDocumentExpirationNotification
{
    public class SendDocumentExpirationNotificationCommand : AuthToken, IRequest<Result>
    {
        public int DocumentId { get; set; }
        public int SubscriberId { get; set; }

        public SigningRecipients[] RecipientDetails { get; set; }
        public string UserId { get; set; }
        public int SystemSettingId { get; set; }
        public string[] Recipients { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
    }


    public class SendDocumentExpirationNotificationCommandHandler : IRequestHandler<SendDocumentExpirationNotificationCommand, Result>
    {
        private readonly IAuthService _authService;
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;

        public SendDocumentExpirationNotificationCommandHandler(IAuthService authService, IApplicationDbContext contect,
            IMediator mediator, IMapper mapper, IEmailService emailService, IStringHashingService stringHashingService,
            IConfiguration configuration)
        {
            _authService = authService;
            _context = contect;
            _mediator = mediator;
            _mapper = mapper;
            _emailService = emailService;
            _stringHashingService = stringHashingService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(SendDocumentExpirationNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                var document = await _context.Documents.Where(a => a.Id == request.DocumentId).FirstOrDefaultAsync();
                if (document == null)
                {
                    return Result.Failure("Invalid Document details");
                }
                var recipients = await _context.Recipients.Where(r => r.DocumentId == request.DocumentId).ToListAsync();
                var systemSetting = await _authService.GetSystemSettingAsync(request.AccessToken, request.SubscriberId, request.UserId);



                foreach (var recipient in recipients)
                {

                    var entity = systemSetting.entity;
                    var systemSettingsModel = new SystemSettingDto
                    {
                        Id = entity.Id,
                        ExpirationReminder = entity.ExpirationReminder,
                        ExpirationSettingsFrequency = entity.ExpirationSettingsFrequency,
                        ExpirationSettingsFrequencyDesc = entity.ExpirationSettingsFrequencyDesc,
                        ShouldSentDocumentsExpire = entity.ShouldSentDocumentsExpire,
                        DocumentExpirationPeriod = entity.DocumentExpirationPeriod,
                        WorkflowReminder = entity.WorkflowReminder,
                        WorkflowReminderSettingsFrequency = entity.WorkflowReminderSettingsFrequency,
                        WorkflowReminderSettingsFrequencyDesc = entity.WorkflowReminderSettingsFrequencyDesc,
                        Currency = entity.Currency,
                        Language = entity.Language,
                        Name = entity.Name,
                        SubscriberId = entity.SubscriberId,
                        CreatedByEmail = entity.CreatedByEmail,
                        CreatedById = entity.CreatedById,
                        CreatedDate = entity.CreatedDate,
                        LastModifiedById = entity.LastModifiedById,
                        LastModifiedByEmail = entity.LastModifiedByEmail,
                        Status = entity.Status,
                        StatusDesc = entity.StatusDesc


                    };



                    var email = new EmailVm
                    {
                        Subject = "Document Expiry Notification",
                        Body = $"This document is expiring soon. Kindly click the button below to sign this document.",
                        RecipientEmail = recipient.Email,
                        ButtonText = "Sign Document Now!",
                        ButtonLink = document.DocumentSigningUrl
                    };


                    var daysLeftBeforeExpired = document.ExpiryDate - DateTime.Now;

                    

                    foreach (var item in systemSettingsModel.ExpirationReminder)
                    {
                        var frequencySetting = item.ExpirationSettingsFrequency;
                        switch (frequencySetting)
                        {
                            case Domain.Enums.SettingsFrequency.Days:
                                if (daysLeftBeforeExpired.Days <= systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    await _emailService.SendEmail(email);
                                }
                                break;
                            case Domain.Enums.SettingsFrequency.Weeks:
                                if ((daysLeftBeforeExpired.Days / (int)Domain.Enums.Period.Weekly) <= systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    await _emailService.SendEmail(email);
                                }
                                break;
                            case Domain.Enums.SettingsFrequency.Months:
                                if ((daysLeftBeforeExpired.Days / (int)Domain.Enums.Period.Monthly) <= systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    await _emailService.SendEmail(email);
                                }
                                break;
                            case Domain.Enums.SettingsFrequency.Year:
                                if ((daysLeftBeforeExpired.Days / (int)Domain.Enums.Period.Yearly) == systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    await _emailService.SendEmail(email);
                                }
                                break;
                            default:
                                break;
                        } 
                    }


                }
                return Result.Success("Sending reminder mail successful");
            }
            catch (Exception ex)
            {

                return Result.Failure($"Sending notification mail to recipients failed. Error: { ex?.Message ?? ex?.InnerException?.Message}"); ;
            }


        }
    }
}
