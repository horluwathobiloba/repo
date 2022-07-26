using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.RecipientActions.Commands.LogRecipientAction;
using OnyxDoc.DocumentService.Domain.Constants;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Domain.ViewModels;
using OnyxDoc.DocumentService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OnyxDoc.DocumentService.Application.Documents.Commands.SendToDocumentSignatories
{
    public class SendToDocumentSignatoriesCommand : AuthToken, IRequest<Result>
    {
        public int DocumentId { get; set; }
        public int SubscriberId { get; set; }
        public SigningRecipients[] RecipientDetails { get; set; }
        public string FilePath { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string[] Recipients { get; set; }

    }

    public class SendToDocumentSignatoriesCommandHandler : IRequestHandler<SendToDocumentSignatoriesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        public SendToDocumentSignatoriesCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService,
            IStringHashingService stringHashingService, IEmailService emailService, 
            IConfiguration configuration, IAuthService authService, IMediator mediator,INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _stringHashingService = stringHashingService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
            _mediator = mediator;
            _notificationService = notificationService;
        }
        public async Task<Result> Handle(SendToDocumentSignatoriesCommand request, CancellationToken cancellationToken)
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
                document.DocumentMessage = request.Message;
                //if (document.DocumentStatus != DocumentStatus.Draft)
                //{
                //    return Result.Failure("Document has been sent already");
                //}
                var recipients = await _context.Recipients.Where(a => a.DocumentId == request.DocumentId).ToDictionaryAsync(a => a.Email);
                bool hasExistingRecipients = (recipients != null && recipients.Count > 0);

                bool hasRecipientsInRequestParameter = (request.Recipients != null && request.Recipients.Count() > 0);
                //send email for just the first person
                var recipientsForCreation = new List<Domain.Entities.Recipient>();
                await _context.BeginTransactionAsync();
                //send document to the normal signatories
                var existingRecipients = new List<Domain.Entities.Recipient>();
                if (hasRecipientsInRequestParameter)
                {
                    var lastRank = (hasExistingRecipients) ? recipients.LastOrDefault(a => a.Value.Rank > 0).Value.Rank : 0;
                    lastRank = lastRank + 1;

                    foreach (var recipient in request.Recipients)
                    {
                        var dateTicks = DateTime.Now.Ticks;
                        var hash = (recipient + dateTicks).ToString();
                        hash = _stringHashingService.CreateDESStringHash(hash);
                        hash = HttpUtility.UrlEncode(hash);
                        if (hasExistingRecipients)
                        {
                            if (recipients.TryGetValue(recipient, out Domain.Entities.Recipient recipientValue))
                            {
                                recipientValue.DocumentSigningUrl = _configuration["WebUrlSign"] + "?hash=" + hash;
                                existingRecipients.Add(recipientValue);
                                continue;
                            }
                        }
                        recipientsForCreation.Add(new Domain.Entities.Recipient
                        {
                            CreatedByEmail = request.UserId,
                            CreatedDate = DateTime.Now,
                            DocumentId = request.DocumentId,
                            DocumentMessage = request.Message,
                            Email = recipient,
                            CreatedBy = request.UserId,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString(),
                            SubscriberId = request.SubscriberId,
                            Rank = lastRank,
                            DocumentSigningUrl = _configuration["WebUrlSign"] + "?hash=" + hash
                        });

                        await _context.Recipients.AddRangeAsync(recipientsForCreation);
                    }
                }

                var componentForFirstRecipient = new Component();
                var firstRecipient = new Domain.Entities.Recipient();
                if (request.RecipientDetails != null && request.RecipientDetails.Count() > 0)
                {
                    List<Component> components = new List<Component>();
                    foreach (var detail in request.RecipientDetails)
                    {
                        var coordinate = new Coordinate
                        {
                            Position = detail.Component.Coordinate.Position,
                            Transform = detail.Component.Coordinate.Transform,
                            Height = detail.Component.Coordinate.Height,
                            Width = detail.Component.Coordinate.Width
                        };
                        Domain.Entities.Component component = new Component
                        {
                            Email = detail.Component.RecipientEmail,
                            FilePath = request.FilePath,
                            Coordinate = coordinate,
                            SelectOptions = JsonConvert.SerializeObject(detail.Component.SelectOptions),
                            Validators = JsonConvert.SerializeObject(detail.Component.Validators),
                            Value = detail.Component.Value,
                            Name = detail.Component.Name,
                            SubscriberId = request.SubscriberId,
                            Type = detail.Component.Type,
                            RecipientId = detail.Component.RecipientId,
                            SubscriberName = _authService?.Subscriber?.Name,
                            Rank = detail.Component.Rank,
                            DocumentId = request.DocumentId,
                            CreatedDate = DateTime.Now,
                            Status = Status.Active,
                            PageNumber = detail.Component.PageNumber,
                            StatusDesc = Status.Active.ToString()
                        };
                        if (recipientsForCreation != null || recipientsForCreation.Count > 0)
                        {
                            var recipientDetail = recipientsForCreation.Where(a => a.Email == component.Email).FirstOrDefault();
                            if (recipientDetail == null)
                            {
                                if (existingRecipients != null || existingRecipients.Count > 0)
                                {
                                    recipientDetail = existingRecipients.Where(a => a.Email == component.Email).FirstOrDefault();
                                    component.Hash = recipientDetail.DocumentSigningUrl.Split("hash=")[1];
                                    
                                }
                            }
                            else
                            {
                                component.Hash = recipientDetail.DocumentSigningUrl.Split("hash=")[1];
                            }
                        }
                        components.Add(component);

                    }
                     componentForFirstRecipient = components.FirstOrDefault(a => a.Rank > 0 && !string.IsNullOrWhiteSpace(a.Hash));
                    await _context.Components.AddRangeAsync(components);
                    _context.Recipients.UpdateRange(existingRecipients);
                    await _context.SaveChangesAsync(cancellationToken);
                    //send email to first recipient with component
                    firstRecipient = recipients.FirstOrDefault(a => a.Value.Email == componentForFirstRecipient.Email).Value;
                    if (firstRecipient == null)
                    {
                        firstRecipient = recipientsForCreation.FirstOrDefault(a => a.Email == componentForFirstRecipient.Email);
                    }
                    if (firstRecipient == null)
                    {
                        return Result.Failure($"No valid recipient found");
                    }
                    EmailVm email = new EmailVm
                    {
                        Subject = " Document Request for Signing",
                        Body = request.Message,
                        Body1 = $"You have received a document request for you to sign! Kindly click the button below to sign the document.",
                        RecipientEmail = firstRecipient.Email,
                        ButtonText = "Sign Document Now!",
                        ButtonLink = firstRecipient.DocumentSigningUrl,
                        FirstName = firstRecipient.FirstName,
                        LastName = firstRecipient.LastName
                    };
                    await _emailService.SendEmail(email);
                }

                //update document
                document.DocumentSigningUrl = firstRecipient.DocumentSigningUrl;
                document.DocumentStatus = DocumentStatus.Sent;
                document.DocumentStatusDesc = DocumentStatus.Sent.ToString();
                _context.Documents.Update(document);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success($"Sending Document to {firstRecipient.Email} was successfull!");
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Sending Document to first recipient failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

      


    }

}
