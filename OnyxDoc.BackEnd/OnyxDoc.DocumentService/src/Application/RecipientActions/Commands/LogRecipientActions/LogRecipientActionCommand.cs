using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Inboxes.Commands.CreateInboxes;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Domain.ViewModels;
using OnyxDoc.DocumentService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.DocumentService.Application.RecipientActions.Commands.LogRecipientAction
{
    public class LogRecipientActionCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentId { get; set; }
        public DocumentRecipientAction DocumentRecipientAction { get; set; }
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }
        public int RecipientId { get; set; }
        public string UserId { get; set; }
    }


    public class LogRecipientActionCommandHandler : IRequestHandler<LogRecipientActionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private string accessToken;

        public LogRecipientActionCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, IEmailService emailService,
            IConfiguration configuration, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
        }

        public async Task<Result> Handle(LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await LogAction(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        internal async Task<Result> LogAction(LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // var user=await _authService.GetUserAsync()
                var authResponse = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var user = _authService.User;

                var document = await _context.Documents.Include(a => a.Recipients).Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.DocumentId).FirstOrDefaultAsync();
                if (document == null)
                {
                    return Result.Failure("Invalid Document");
                }
                var existingAction = await _context.RecipientActions
                    .Where(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId
                    && x.RecipientId == request.RecipientId && x.AppSigningUrl == request.AppSigningUrl
                    && x.DocumentRecipientAction == request.DocumentRecipientAction && x.Status == Status.Active).FirstOrDefaultAsync();


                if (existingAction != null && existingAction.Id > 0)
                {
                    return Result.Failure($"This document version has already been {request.DocumentRecipientAction.ToString()} by {existingAction.Recipient.Email}");
                }
                //var user = await _authService.GetUserAsync();
                await _context.BeginTransactionAsync();

                //The signing app can have a null access token because of external people signing
                if (request.AccessToken != null && !(request.UserId.Contains("@")))
                {
                    this.accessToken = request.AccessToken;
                }
                //get previously logged recipient action
                var loggedRecipientActions = await _context.RecipientActions.Where(a => a.SubscriberId == request.SubscriberId && a.DocumentId == request.DocumentId).ToListAsync();
                #region Save the current action 
                var entity = new RecipientAction
                {
                    SubscriberId = request.SubscriberId,
                    SubscriberName = request.SubscriberName,
                    DocumentId = request.DocumentId,
                    RecipientId = request.RecipientId,
                    DocumentRecipientAction = request.DocumentRecipientAction,
                    RecipientActionDesc = request.DocumentRecipientAction.ToString(),
                    AppSigningUrl = request.AppSigningUrl,
                    SignedDocumentUrl = request.SignedDocumentUrl,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.RecipientActions.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                #endregion

                var email = new EmailVm();
                var emailResponse = "";
                string loginPage = _configuration["LoginPage"];

                var currentRecipient = entity.Recipient;
                var nextRecipient = new Recipient();
                switch (request.DocumentRecipientAction)
                {
                    case DocumentRecipientAction.Sign:
                        {
                            nextRecipient = document.Recipients.Where(a => a.Rank == (entity.Recipient.Rank + 1)).FirstOrDefault();

                            #region move to the next recipient

                            if (nextRecipient == null || nextRecipient.Id <= 0)
                            {
                                document.DocumentStatus = DocumentStatus.Completed;
                                document.DocumentStatusDesc = DocumentStatus.Completed.ToString();
                                document.SignedDocument = request.SignedDocumentUrl;
                                await SendSignedDocumentEmailToRecipients(document, request, cancellationToken);
                            }
                            else
                            {
                                // This is the recipient category for the next recipient. This allows you to know who is next and the document status
                                //var recipientCategory = nextRecipient.RecipientCategory.ParseEnumString<RecipientCategory>();
                                //switch (recipientCategory)
                                {
                                    // case RecipientCategory.ExternalSignatory:
                                    // case RecipientCategory.InternalSignatory:
                                    {
                                        if (nextRecipient.RecipientCategory == RecipientCategory.ExternalSignatory.ToString())
                                        {
                                            document.NextActorAction = RecipientCategory.ExternalSignatory.ToString();
                                        }
                                        if (nextRecipient.RecipientCategory == RecipientCategory.InternalSignatory.ToString())
                                        {
                                            document.NextActorAction = RecipientCategory.InternalSignatory.ToString();
                                        }
                                        document.DocumentStatus = DocumentStatus.WaitingForOthers;
                                        document.DocumentStatusDesc = DocumentStatus.WaitingForOthers.ToString();

                                        email = new EmailVm
                                        {
                                            Subject = "Document Request for Signing",
                                            Body = document.DocumentMessage,
                                            Body1 = $"You have received a document request for you to sign on {document.Name}! Kindly click the button below to sign the document.",
                                            RecipientEmail = nextRecipient.Email,
                                            FirstName = nextRecipient.FirstName,
                                            RecipientName = nextRecipient.FirstName,
                                            ButtonText = "Sign Document Now!",
                                            ButtonLink = nextRecipient.DocumentSigningUrl
                                        };
                                        emailResponse = await _emailService.SendEmail(email);
                                        await CreateInbox(request, email, cancellationToken);

                                    }
                                    // break;

                                    // default:
                                    //  throw new Exception("Invalid recipient category");
                                    // }
                                    document.NextActorEmail = nextRecipient.Email;
                                    document.NextActorRank = nextRecipient.Rank;
                                    #endregion

                                }
                            }
                        }
                        break;

                    case DocumentRecipientAction.Reject:
                        {
                            #region update the document next actor and deactivate the remaining action performed by the previous actors
                            //run the query to deactivate all previous actions
                            var recipientActions = await _context.RecipientActions.Where(a => a.SubscriberId == request.SubscriberId &&
                                                           a.DocumentId == request.DocumentId).ToListAsync();

                            if (recipientActions != null && recipientActions.Count() > 0)
                            {
                                var documentRecipientIds = recipientActions.Select(a => a.DocumentId).ToList();
                                List<RecipientAction> recipientActionsForUpdate = new List<RecipientAction>();
                                foreach (var recipientAction in recipientActions)
                                {
                                    recipientAction.Status = Status.Deactivated;
                                    recipientAction.StatusDesc = Status.Deactivated.ToString();
                                    recipientActionsForUpdate.Add(recipientAction);
                                }
                                _context.RecipientActions.UpdateRange(recipientActionsForUpdate);
                                //run the query to deactivate all previous recipients
                                var documentRecipients = await _context.Recipients.Where(a => a.Id.IN<int>(documentRecipientIds) &&
                                                         a.SubscriberId == request.SubscriberId &&
                                                         a.DocumentId == request.DocumentId).ToListAsync();
                                if (documentRecipients != null && documentRecipients.Count() > 0)
                                {
                                    List<Recipient> recipients = new List<Recipient>();
                                    foreach (var recipient in recipients)
                                    {
                                        recipient.Status = Status.Deactivated;
                                        recipient.StatusDesc = Status.Deactivated.ToString();
                                        recipients.Add(recipient);
                                    }
                                    _context.Recipients.UpdateRange(recipients);
                                }
                            }
                            document.NextActorEmail = null;
                            document.NextActorRank = 0;
                            document.NextActorAction = null;
                            document.DocumentStatus = DocumentStatus.Rejected;
                            document.DocumentStatusDesc = DocumentStatus.Rejected.ToString();
                            await SendRejectionEmailToSignatories(document, currentRecipient.Email, request, user, cancellationToken);
                            await _emailService.SendEmail(email);
                            await CreateInbox(request, email, cancellationToken);
                            //run the query to deactivate all previous actions 
                            //  var rowCount = await _context.ExecuteSqlInterpolatedAsync($"UPDATE DocumentRecipientActions SET status={(int)Status.Deactivated}, statusdesc={Status.Deactivated.ToString()} WHERE organisationid={request.OrganisationId} AND documentid={document.Id} AND recipientaction='{RecipientAction.Sign.ToString()}';", cancellationToken);
                            //run the query to deactivate all previous actions 
                            var documentRecipientActions = await _context.RecipientActions.Where(a => a.SubscriberId == request.SubscriberId &&
                                                           a.DocumentId == request.DocumentId && a.DocumentRecipientAction == DocumentRecipientAction.Sign).ToListAsync();
                            if (documentRecipientActions != null && documentRecipientActions.Count > 0)
                            {
                                List<RecipientAction> recipientActionsForUpdate = new List<RecipientAction>();
                                foreach (var recipientAction in documentRecipientActions)
                                {
                                    recipientAction.Status = Status.Deactivated;
                                    recipientAction.StatusDesc = Status.Deactivated.ToString();
                                    recipientActionsForUpdate.Add(recipientAction);
                                }
                                _context.RecipientActions.UpdateRange(recipientActionsForUpdate);
                            }
                            document.NextActorEmail = document.SenderEmail;
                            document.DocumentStatus = DocumentStatus.Sent;
                            document.DocumentStatusDesc = DocumentStatus.Sent.ToString();
                            //delete previous document dimensions
                            var components = await _context.Components.Where(a => a.DocumentId == request.DocumentId).ToListAsync();
                            if (components != null || components.Count > 0)
                            {
                                _context.Components.RemoveRange(components);
                            }
                            #endregion
                        }
                        break;

                    default:
                        throw new Exception("No action specified.");
                }
                document.LastModifiedBy = request.UserId;
                document.LastModifiedDate = DateTime.Now;
                _context.Documents.Update(document);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success($" {request.DocumentRecipientAction} Action was completed successfully by {entity.Recipient.Email}");
            }
            catch (Exception ex)
            {

                _context.RollbackTransaction();
                return Result.Failure($"Document recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }


        internal async Task SendSignedDocumentEmailToRecipients(Document document, LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipients = document.Recipients.Where(a => a.Status == Status.Active).ToList();
                var subject = "Document Signing Completed";
                var body = document.DocumentMessage;
                var body1 = $"The document '{document.Name}' has been successfully signed by all recipients.";
                var buttonText = "View Signed Document";

                var list = new List<EmailVm>();
                foreach (var item in recipients) //Send email to all the document recipients
                {
                    list.Add(new EmailVm
                    {
                        Subject = subject,
                        Body = body,
                        Body1 = body1,
                        ButtonText = buttonText,
                        RecipientEmail = item.Email,
                        RecipientName = item.FirstName,
                        ButtonLink = document.SignedDocument
                    });
                }
                list.Add(await ComposeEmailToInitiator(document, subject, body, buttonText));
                await _emailService.SendBulkEmail(list);
                await CreateInboxes(request, list,cancellationToken,recipients);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task SendRejectionEmailToInitiator(LogRecipientActionCommand request, Document document, Recipient currentRecipient, UserDto user, CancellationToken cancellationToken)
        {
            try
            {
                var initiator = await _authService.GetUserAsync(request.AccessToken, document.SubscriberId, document.UserId, document.UserId);
                //Todo : send email to initiator
                var email = new EmailVm
                {
                    Subject = "Document Request Rejected!",
                    RecipientName = initiator.entity?.Name,
                    Text = $"The document \"{document.Name}\" has been rejected by ({currentRecipient.Email}). You will no longer be able to access and perform any action on the document.",
                    RecipientEmail = initiator.entity?.Email,
                };
                await _emailService.SendEmail(email);
                await CreateInbox(request, email, cancellationToken);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task SendRejectionEmailToSignatories(Document document, string rejectActor, LogRecipientActionCommand request, UserDto user, CancellationToken cancellationToken)
        {
            try
            {
                string loginPage = _configuration["LoginPage"];
                var recipients = document.Recipients.Where(a => a.Status == Status.Active).ToList();

                var subject = "Document Generation Request Rejected!";
                var body = $"The document \"{document.Name}\" has been rejected by one of the signatories({rejectActor}). Please login to view the comments.";
                var buttonText = "Login";

                var list = new List<EmailVm>();
                foreach (var recipient in recipients) //Send email to all the document generators
                {
                    list.Add(new EmailVm
                    {
                        Subject = subject,
                        RecipientName = recipient.Email,
                        Body = body,
                        RecipientEmail = recipient.Email,
                        ButtonText = buttonText,
                        ButtonLink = loginPage
                    });
                }
                //add initiator to the email list
                list.Add(await this.ComposeEmailToInitiator(document, subject, body, buttonText));
                await _emailService.SendBulkEmail(list);
                await CreateInboxes(request, list, cancellationToken);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task<EmailVm> ComposeEmailToInitiator(Document document, string subject, string body, string buttonText = "")
        {
            try
            {
                var email = new EmailVm
                {
                    Subject = subject,
                    Body = body,
                    ButtonText = buttonText,
                    RecipientName = document.CreatedByEmail,
                    RecipientEmail = document.CreatedByEmail
                };

                return email;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        internal async Task<string> CreateInboxes(LogRecipientActionCommand request, List<EmailVm> emails, CancellationToken cancellationToken, List<Recipient> recipients = null)
        {
            try
            {
                var inboxes = new List<InboxVm>();
                var reciepientNames = new List<string>();
                foreach (var item in recipients)
                {
                    string name;
                    if (string.IsNullOrEmpty(item.FirstName) && string.IsNullOrEmpty(item.LastName))
                    {
                        name = item.Email;
                        reciepientNames.Add(name);
                    }
                    else
                    {
                        name = item.FirstName + " " + item.LastName;
                        reciepientNames.Add(name);
                    }

                }
                foreach (var email in emails) //Send email to all the contract generators
                {
                    if (email != null)
                    {
                        inboxes.Add(new InboxVm
                        {
                            Subject = email.Subject,
                            Email = email.RecipientEmail,
                            Body = email.Body,
                            Description = email.Body,
                            SenderEmail = request.UserId,
                            Sender = request.UserId,
                            DocumentId = request.DocumentId,
                            EmailAction = EmailAction.Recieved,
                            Time = DateTime.Now,
                            SubscriberId = request.SubscriberId,
                            DocumentUrl = email.ButtonLink,
                            UserId = request.UserId,
                            Recipients = reciepientNames
                        });
                    }
                    await CreateInboxes(inboxes, cancellationToken);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Inboxes creation failed. Error: { ex?.Message + " " + ex?.InnerException.Message}";
            }
        }

        private async Task CreateInboxes(List<InboxVm> inboxes, CancellationToken cancellationToken)
        {
            var inboxHandler = new CreateInboxesCommandHandler(_context, _authService);
            var createInboxesCommand = new CreateInboxesCommand
            {
                InboxVms = inboxes
            };
            await inboxHandler.Handle(createInboxesCommand, cancellationToken);
        }

        internal async Task<string> CreateInbox(LogRecipientActionCommand request, EmailVm email, CancellationToken cancellationToken)
        {
            try
            {
                var inboxes = new List<InboxVm>();
                var reciepients = new List<string>();


                if (email != null)
                {
                    inboxes.Add(new InboxVm
                    {
                        Subject = email.Subject,
                        Email = email.RecipientEmail,
                        Description = email.Body,
                        SenderEmail = request.UserId,
                        Recipients = new List<string> { email.RecipientEmail },
                        Sender = request.UserId,
                        Body = email.Body,
                        EmailAction = EmailAction.Recieved,
                        Time = DateTime.Now,
                        SubscriberId = request.SubscriberId,
                        DocumentUrl = email.ButtonLink,
                        UserId = request.UserId,
                        DocumentId = request.DocumentId,
                    });
                }
               await CreateInboxes(inboxes, cancellationToken);
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Inboxes creation failed. Error: { ex?.Message + " " + ex?.InnerException.Message}";
            }
        }


    }
}


