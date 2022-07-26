using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Application.ContractRecipientActions.Queries.GetContractRecipientActions;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Application.Inboxs.Commands.CreateInbox; 
using Onyx.ContractService.Domain.Common;
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

namespace Onyx.ContractService.Application.ContractRecipientActions.Commands.LogRecipientAction
{
    public class LogRecipientActionCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public RecipientAction RecipientAction { get; set; }
        /// <summary>
        /// This can be null depending on if the approver or signatory signs.
        /// </summary>
        public FileUploadRequest ApproverSignature { get; set; } = new FileUploadRequest();
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }
        public int ContractRecipientId { get; set; }
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
        private UserVm initiator;
        private IMediator _mediator;
        private INotificationService _notificationService;

        public LogRecipientActionCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, IEmailService emailService,
            IConfiguration configuration, IAuthService authService, IMediator mediator, INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
            _mediator = mediator;
            _notificationService = notificationService;
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
                return Result.Failure($"Contract recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        internal async Task<Result> LogAction(LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                //var exists = await _context.ContractRecipientActions
                //    .Where(x => x.OrganisationId == request.OrganisationId && x.ContractId == request.ContractId
                //    && x.ContractRecipientId == request.ContractRecipientId && x.AppSigningUrl == request.AppSigningUrl
                //    && x.RecipientAction == request.RecipientAction.ToString() && x.Status == Status.Active).FirstOrDefaultAsync();

                //if (exists != null && exists.Id > 0)
                //{
                //    return Result.Failure($"This document version has already been {GetActionMessage(request.RecipientAction)} by {exists.ContractRecipient.Email}");
                //}

                await _context.BeginTransactionAsync();
                
                //The signing app can have a null access token because of external people signing
                if (request.AccessToken != null && !(request.UserId.Contains("@")))
                {

                    this.accessToken = request.AccessToken;
                    this.initiator = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                    if (initiator == null)
                    {
                        return Result.Failure("UserId is not valid");
                    }
                }
                    //get previously logged recipient action
                    var loggedRecipientActions = await _context.ContractRecipientActions.Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId).ToListAsync();
                #region Save the current action 
                var entity = new ContractRecipientAction
                {
                    ApproverSignatureBlobFileUrl = await request.ApproverSignature.SaveBlobFile(request.ContractRecipientId.ToString(), _blobService),
                    ApproverSignatureFileExtension = request.ApproverSignature.FileExtension,
                    ApproverSignatureMimeType = request.ApproverSignature.FileMimeType,

                    OrganisationId = request.OrganisationId,
                    OrganisationName = request.OrganisationName,
                    ContractId = request.ContractId,
                    ContractRecipientId = request.ContractRecipientId,
                    RecipientAction = request.RecipientAction.ToString(),
                    AppSigningUrl = request.AppSigningUrl,
                    SignedDocumentUrl = request.SignedDocumentUrl,

                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.ContractRecipientActions.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                #endregion

                var email = new EmailVm();
                var emailResponse = "";
                string loginPage = _configuration["LoginPage"];

                var currentRecipient = entity.ContractRecipient;
                // at this point,  we need to update the contract specifying the next recipient if any.
                var contract = await _context.Contracts.Include(a => a.ContractRecipients).Include(a=>a.ContractDuration).Where(a => a.OrganisationId == request.OrganisationId && a.Id == request.ContractId).FirstOrDefaultAsync();
                var recipientcategory = currentRecipient.RecipientCategory.ParseEnumString<RecipientCategory>();

                if (contract != null)
                {
                    var nextRecipient = new ContractRecipient();

                    switch (request.RecipientAction)
                    {
                        case RecipientAction.Approve:
                        case RecipientAction.Sign:
                            {
                                nextRecipient = contract.ContractRecipients.Where(a => a.Rank == (entity.ContractRecipient.Rank + 1)).FirstOrDefault();

                                #region move to the next recipient

                                if (nextRecipient == null || nextRecipient.Id <= 0) // There is no more approver. At this point, set the contract status to active
                                {
                                    #region Contract renewal - startdate and contract status logic code block
                                    if (contract.RenewedContractId <= 0) //Contract is not been renewed
                                    {
                                        contract.ContractStartDate = DateTime.Now;
                                        contract.ContractStatus = ContractStatus.Active;
                                        contract.ContractStatusDesc = ContractStatus.Active.ToString();
                                    }
                                    else //This is a potential renewal
                                    {
                                        var existingContract = await _context.Contracts.FirstOrDefaultAsync(x => x.Id == contract.RenewedContractId);
                                        if (existingContract == null) //Renewed contract does not exist/ is invalid. At this point, simply activate the contract and continue the execution.
                                        {
                                            contract.ContractStartDate = DateTime.Now;
                                            contract.ContractStatus = ContractStatus.Active;
                                            contract.ContractStatusDesc = ContractStatus.Active.ToString();
                                        }
                                        else //Renewed contract exists and is valid. At this point, simply complete the execution and set the contract status to "PendingExecution"
                                        {
                                            contract.ContractStartDate = existingContract.ContractExpirationDate.Value.AddDays(1);
                                            contract.ContractStatus = ContractStatus.PendingActivation;
                                            contract.ContractStatusDesc = ContractStatus.PendingActivation.ToString();
                                        }
                                    }
                                    #endregion
                                   
                                    contract.ContractExpirationDate = contract.ComputeContractExpirationDate();
                                    contract.ExecutedContract = request.SignedDocumentUrl;
                                    contract.IsAnExecutedDocument = true;
                                    contract.ExecutedContractFileExtension = null;
                                    contract.ExecutedContractMimeType = null;
                                    await SendSignedDocumentEmailToRecipients(contract, request, cancellationToken);
                                }
                                else
                                {
                                    // This is the recipient category for the next recipient. This allows you to know who is next and the contract status
                                    var recipientCategory = nextRecipient.RecipientCategory.ParseEnumString<RecipientCategory>();
                                    switch (recipientCategory)
                                    {
                                        case RecipientCategory.Approver:
                                            {
                                                contract.NextActorAction = RecipientCategory.Approver.ToString();
                                                contract.ContractStatus = ContractStatus.PendingApproval;
                                                contract.ContractStatusDesc = ContractStatus.PendingApproval.ToString();
                                                email = new EmailVm
                                                {
                                                    Subject = "New Contract - Approval Request",
                                                    Text = $"You have received a document initiation request for you to approve on the contract: {contract.Name}.",
                                                    RecipientEmail = nextRecipient.Email,
                                                    ButtonText = "Approve Request Now!",
                                                    ButtonLink = loginPage
                                                };

                                              //  emailResponse =
                                                await _emailService.SendEmail(email);
                                                await this.CreateInbox(request, email); 
                                            }
                                            break;

                                        case RecipientCategory.ContractGenerator:
                                            {
                                                contract.NextActorAction = RecipientCategory.ContractGenerator.ToString();
                                                contract.ContractStatus = ContractStatus.Approved;
                                                contract.ContractStatusDesc = ContractStatus.Approved.ToString();
                                                await SendEmailToContractGenerators(contract, loginPage, request, cancellationToken); // send emails to all contract generators just in case there are multiple generators
                                            }
                                            break;
                                        case RecipientCategory.ExternalSignatory:
                                        case RecipientCategory.InternalSignatory:
                                            {
                                                if (nextRecipient.RecipientCategory == RecipientCategory.ExternalSignatory.ToString())
                                                {
                                                    contract.NextActorAction = RecipientCategory.ExternalSignatory.ToString();
                                                }
                                                if (nextRecipient.RecipientCategory == RecipientCategory.InternalSignatory.ToString())
                                                {
                                                    contract.NextActorAction = RecipientCategory.InternalSignatory.ToString();
                                                }
                                               
                                                contract.ContractStatus = ContractStatus.PendingSignatories;
                                                contract.ContractStatusDesc = ContractStatus.PendingSignatories.ToString();
                                                if (!string.IsNullOrWhiteSpace(request.AppSigningUrl))
                                                {
                                                    email = new EmailVm
                                                    {
                                                        Subject = "Contract Document Request for Signing",
                                                        Body = $"You have received a document request for you to sign on the contract: {contract.Name}! Kindly click the button below to sign the document.",
                                                        RecipientEmail = nextRecipient.Email,
                                                        ButtonText = "Sign Document Now!",
                                                        ButtonLink = nextRecipient.DocumentSigningUrl
                                                    };
                                                    await _emailService.SendEmail(email);
                                                    await this.CreateInbox(request, email);
                                                }
                                            }
                                            break;

                                        default:
                                            throw new Exception("Invalid recipient category");
                                    }
                                    contract.NextActorEmail = nextRecipient.Email;
                                    contract.NextActorRank = nextRecipient.Rank;
                                }
                                #endregion
                            }
                            break;

                        case RecipientAction.Reject:
                            {
                                #region update the contract next actor and deactivate the remaining action performed by the previous actors
                                switch (recipientcategory)
                                {
                                    case RecipientCategory.Approver:
                                    case RecipientCategory.ContractGenerator:
                                        {
                                            //run the query to deactivate all previous actions
                                            var contractRecipientActions = await _context.ContractRecipientActions.Where(a => a.OrganisationId == request.OrganisationId &&
                                                                           a.ContractId == request.ContractId && a.RecipientAction == RecipientAction.Approve.ToString()).ToListAsync();
                                           
                                            if (contractRecipientActions != null && contractRecipientActions.Count > 0)
                                            {
                                                var contractRecipientIds = contractRecipientActions.Select(a => a.ContractId).ToList();
                                                List<ContractRecipientAction> recipientActionsForUpdate = new List<ContractRecipientAction>();
                                                foreach (var recipientAction in contractRecipientActions)
                                                {
                                                    recipientAction.Status = Status.Deactivated;
                                                    recipientAction.StatusDesc = Status.Deactivated.ToString();
                                                    recipientActionsForUpdate.Add(recipientAction);
                                                }
                                                _context.ContractRecipientActions.UpdateRange(recipientActionsForUpdate);
                                                //run the query to deactivate all previous recipients
                                                var contractRecipients = await _context.ContractRecipients.Where(a => a.Id.IN<int>(contractRecipientIds) && 
                                                                         a.OrganisationId == request.OrganisationId &&
                                                                         a.ContractId == request.ContractId).ToListAsync();
                                                if (contractRecipients != null && contractRecipients.Count > 0)
                                                {
                                                    List<ContractRecipient> recipients = new List<ContractRecipient>();
                                                    foreach (var recipient in contractRecipients)
                                                    {
                                                        recipient.Status = Status.Deactivated;
                                                        recipient.StatusDesc = Status.Deactivated.ToString();
                                                        recipients.Add(recipient);
                                                    }
                                                    _context.ContractRecipients.UpdateRange(recipients);
                                                }
                                            }

                                          
                                            //var rowCount = await _context.ExecuteSqlInterpolatedAsync($"UPDATE ContractRecipientActions SET status={(int)Status.Deactivated}, statusdesc='{Status.Deactivated.ToString()}' WHERE organisationid={request.OrganisationId} AND contractid={contract.Id} AND recipientaction='{RecipientAction.Approve.ToString()}';", cancellationToken);

                                            //if (rowCount > 0 || loggedRecipientActions.Count == 0)
                                            //{
                                            contract.NextActorEmail = null;
                                                contract.NextActorRank = 0;
                                                contract.NextActorAction = null;
                                                contract.ContractStatus = ContractStatus.Rejected;
                                                contract.ContractStatusDesc = ContractStatus.Rejected.ToString();
                                                //await SendRejectionEmailToContractGenerators(contract, currentRecipient.Email, request, cancellationToken);
                                                await SendRejectionEmailToApprovers(contract, currentRecipient.Email, request, cancellationToken);
                                                emailResponse = await _emailService.SendEmail(email);
                                            //}
                                            //else
                                            //{
                                            //    throw new Exception("Reject contract action failed because there was an error cancelling previous approvers.");
                                            //}
                                        }
                                        break;

                                    case RecipientCategory.ExternalSignatory:
                                    case RecipientCategory.InternalSignatory:
                                        {
                                            //run the query to deactivate all previous actions 
                                          //  var rowCount = await _context.ExecuteSqlInterpolatedAsync($"UPDATE ContractRecipientActions SET status={(int)Status.Deactivated}, statusdesc={Status.Deactivated.ToString()} WHERE organisationid={request.OrganisationId} AND contractid={contract.Id} AND recipientaction='{RecipientAction.Sign.ToString()}';", cancellationToken);
                                            //run the query to deactivate all previous actions 
                                            var contractRecipientActions = await _context.ContractRecipientActions.Where(a => a.OrganisationId == request.OrganisationId &&
                                                                           a.ContractId == request.ContractId && a.RecipientAction == RecipientAction.Sign.ToString()).ToListAsync();
                                            if (contractRecipientActions != null && contractRecipientActions.Count > 0)
                                            {
                                                List<ContractRecipientAction> recipientActionsForUpdate = new List<ContractRecipientAction>();
                                                foreach (var recipientAction in contractRecipientActions)
                                                {
                                                    recipientAction.Status = Status.Deactivated;
                                                    recipientAction.StatusDesc = Status.Deactivated.ToString();
                                                    recipientActionsForUpdate.Add(recipientAction);
                                                }
                                                _context.ContractRecipientActions.UpdateRange(recipientActionsForUpdate);
                                            }
                                           // if (rowCount > 0 || loggedRecipientActions.Count > 0)
                                            {
                                                //delete previous document dimensions
                                                // get the contract generator as the contract recipient and set it as the next actor on contract
                                                nextRecipient = contract.ContractRecipients.Where(a => a.RecipientCategory == RecipientCategory.ContractGenerator.ToString()).FirstOrDefault();
                                                if (nextRecipient != null)
                                                {
                                                    contract.NextActorEmail = nextRecipient.Email;
                                                    contract.NextActorRank = nextRecipient.Rank;
                                                    contract.NextActorAction = RecipientCategory.ContractGenerator.ToString();
                                                    contract.ContractStatus = ContractStatus.Approved;
                                                    contract.ContractStatusDesc = ContractStatus.Approved.ToString();
                                                    //delete previous document dimensions
                                                    var dimensions = await _context.Dimensions.Where(a => a.ContractId == request.ContractId).ToListAsync();
                                                    if (dimensions != null || dimensions.Count > 0)
                                                    {
                                                        _context.Dimensions.RemoveRange(dimensions);
                                                    }
                                                    //remove last generator action so regeneration can take place
                                                    var contractGeneratorAction = await _context.ContractRecipientActions.Where(a => a.ContractId == contract.Id && nextRecipient.Id == a.ContractRecipientId).FirstOrDefaultAsync();
                                                    if (contractGeneratorAction != null)
                                                    {
                                                        _context.ContractRecipientActions.Remove(contractGeneratorAction);
                                                    }
                                                    //send emails to generators,initiators, approvers an
                                                    await SendRejectionEmailToContractGenerators(contract, currentRecipient.Email, request, cancellationToken);
                                                    await SendRejectionEmailToApproversAndSignatories(contract, currentRecipient.Email, request, cancellationToken);
                                                }
                                                else
                                                {
                                                    throw new Exception("Reject contract action failed because there was an error voiding previous signatures. Please contact support.");
                                                }
                                            }
                                        }
                                        break;

                                    default:
                                        {
                                            throw new Exception("Invalid recipient."); // throw an exception
                                        }
                                }
                                #endregion
                            }
                            break;

                        case RecipientAction.Cancel:
                            {
                                //run the query to deactivate all previous actions 
                                var rowCount = await _context.ExecuteSqlInterpolatedAsync($"UPDATE ContractRecipientActions SET status={(int)Status.Deactivated}, statusdesc={Status.Deactivated.ToString()} WHERE organisationid={request.OrganisationId} AND contractid={contract.Id};", cancellationToken);

                                if (rowCount > 0 || loggedRecipientActions.Count == 0)
                                {
                                    contract.NextActorEmail = null;
                                    contract.NextActorRank = 0;
                                    contract.NextActorAction = null;
                                    contract.ContractStatus = ContractStatus.Cancelled;
                                    contract.ContractStatusDesc = ContractStatus.Cancelled.ToString();
                                    await SendCancellationEmailToRecipients(contract, currentRecipient.Email, request, cancellationToken);
                                }
                                else
                                {
                                    throw new Exception("Cancelling contract action failed because there was an error cancelling previous approvers.");
                                }
                            }
                            break;

                        default:
                            throw new Exception("No action specified.");
                    }

                    contract.LastModifiedBy = request.UserId;
                    contract.LastModifiedDate = DateTime.Now;
                    _context.Contracts.Update(contract);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                //get user object
                //Bypass for Audit id AccessToken is null, mainly for accept and sign 
                //create audit log

                if (initiator != null)
                {
                    var newValuesEntity = new
                    {

                        ContractName = contract.Name,
                        RecipientAction = request.RecipientAction,
                        DateCreated = DateTime.Now,
                        UserId = request.UserId
                    };

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = entity.OrganisationName,
                        LastModifiedBy = request.UserId,
                        RoleId = contract.RoleId,
                        UserId = request.UserId,
                        RoleName = contract.RoleName,
                        FirstName = initiator.Entity.FirstName,
                        LastName = initiator.Entity.LastName,
                        JobFunctionId = initiator.Entity.JobFunctionId,
                        JobFunctionName = initiator.Entity.JobFunction?.Name,
                        Module = Module.Contract.ToString(),
                        NewValue = newValuesEntity,
                        Action = request.RecipientAction.ToString()
                    };

                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }

                await _context.CommitTransactionAsync();


                var result = _mapper.Map<ContractRecipientActionDto>(entity);
                return Result.Success($" {request.RecipientAction} Action was completed successfully by {entity.ContractRecipient.Email}");

            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        private string GetActionMessage(RecipientAction RecipientAction)
        {
            var action = "";
            switch (RecipientAction)
            {
                case RecipientAction.Approve:
                    action = "approved";
                    break;
                case RecipientAction.Sign:
                    action = "signed";
                    break;
                case RecipientAction.Reject:
                    action = "rejected";
                    break;
                case RecipientAction.Cancel:
                    action = "cancelled";
                    break;
                default:
                    break;
            }
            return action;
        }

        internal async Task SendSignedDocumentEmailToRecipients(Domain.Entities.Contract contract, LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipients = contract.ContractRecipients.Where(a => a.Status == Status.Active).ToList();

                var subject = "Contract Signing Completed";
                var body = $"The contract document '{contract.Name}' has been successfully signed by all recipients.";
                var buttonText = "View Signed Document";

                var list = new List<EmailVm>(); 
                foreach (var item in recipients) //Send email to all the contract recipients
                {
                    list.Add(new EmailVm
                    {
                        Subject = subject,
                        Body = body,
                        ButtonText = buttonText,
                        RecipientEmail = item.Email,
                        ButtonLink = contract.ExecutedContract
                    }); 
                }
               
                list.Add(await this.ComposeEmailToInitiator(contract, subject, body, buttonText));
                await _emailService.SendBulkEmail(list);                 
                await this.CreateInboxes(request, list);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task SendRejectionEmailToInitiator(LogRecipientActionCommand request, Domain.Entities.Contract contract, ContractRecipient currentRecipient, CancellationToken cancellationToken)
        {
            try
            {
                var initiator = await _authService.GetUserAsync(request.AccessToken, contract.UserId);
                //Todo : send email to initiator
                var email = new EmailVm
                {
                    Subject = "Contract Request Rejected!",
                    RecipientName = initiator.Entity?.Name,
                    Text = $"The document \"{contract.Name}\" has been rejected by ({currentRecipient.Email}). You will no longer be able to access and perform any action on the document.",
                    RecipientEmail = initiator.Entity?.Email,
                };
                 await _emailService.SendEmail(email);
                await this.CreateInbox(request, email);               
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task SendEmailToContractGenerators(Domain.Entities.Contract contract, string loginPage, LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipients = contract.ContractRecipients.Where(a => a.RecipientCategory == RecipientCategory.ContractGenerator.ToString()).ToList();

                var subject = "New Document Generation Request";
                var body = $"A document initiation request has been approved for you to generate and share with signatories.";
                var buttonText = "Generate Document Now!";

                var list = new List<EmailVm>();
                foreach (var recipient in recipients) //Send email to all the contract generators
                {
                    list.Add(new EmailVm
                    {
                        Subject = subject,
                        RecipientName = recipient.Email,
                        Body = body,
                        ButtonText = buttonText,
                        RecipientEmail = recipient.Email,
                        ButtonLink = loginPage
                    }); 
                }
                //add initiator to the email list
                list.Add(await this.ComposeEmailToInitiator(contract, subject, body, buttonText));
                 await _emailService.SendBulkEmail(list);
                await this.CreateInboxes(request, list);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task SendRejectionEmailToContractGenerators(Domain.Entities.Contract contract, string rejectActor, LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string loginPage = _configuration["LoginPage"];
                var recipients = contract.ContractRecipients.Where(a => a.Status == Status.Active && a.RecipientCategory == RecipientCategory.ContractGenerator.ToString()).ToList();

                var subject = "Document Generation Request Rejected!";
                var body = $"The document \"{contract.Name}\" has been rejected by one of the signatories({rejectActor}) and requires your action. Please login to view the comments and regenerate the document for the signatories to sign.";
                var buttonText = "Generate Document Now!";

                var list = new List<EmailVm>();
                foreach (var recipient in recipients) //Send email to all the contract generators
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
                list.Add(await this.ComposeEmailToInitiator(contract, subject, body, buttonText));
                await _emailService.SendBulkEmail(list);
                await this.CreateInboxes(request, list);
            }
            catch (Exception ex)
            {
                //email sending failed
                //return Result.Failure(ex.Message+" "+ex.InnerException?.Message);
            }
        }

        internal async Task SendRejectionEmailToApprovers(Domain.Entities.Contract contract, string rejectActor, LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string loginPage = _configuration["LoginPage"];
                var recipients = contract.ContractRecipients.Where(a => a.Status == Status.Active && a.RecipientCategory == RecipientCategory.Approver.ToString()).ToList();

                var subject = "Contract Generation Request Rejected!";
                var body = $"The document \"{contract.Name}\" has been rejected by ({rejectActor}). You will no longer be able to access and perform any action on the document.";
                var buttonText = "Login";

                var list = new List<EmailVm>(); 
                foreach (var recipient in recipients) //Send email to all the contract request approvers
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
                list.Add(await this.ComposeEmailToInitiator(contract, subject, body, buttonText));
                 await _emailService.SendBulkEmail(list);
                await this.CreateInboxes(request, list);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task SendRejectionEmailToApproversAndSignatories(Domain.Entities.Contract contract, string rejectActor, LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string loginPage = _configuration["LoginPage"];
                var recipients = contract.ContractRecipients.Where(a => a.Status == Status.Active && a.RecipientCategory != RecipientCategory.ContractGenerator.ToString()).ToList();

                var subject = "Document Generation Request Rejected!";
                var body = $"The document \"{contract.Name}\" has been rejected by one of the signatories({rejectActor}). Please login to view the comments.";
                var buttonText = "Login";

                var list = new List<EmailVm>();                
                foreach (var recipient in recipients) //Send email to all the contract generators
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
                list.Add(await this.ComposeEmailToInitiator(contract, subject, body, buttonText));
                await _emailService.SendBulkEmail(list);
                await this.CreateInboxes(request, list);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task SendCancellationEmailToRecipients(Domain.Entities.Contract contract, string rejectActor, LogRecipientActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipients = contract.ContractRecipients.Where(a => a.Status == Status.Active).ToList();
                var subject = "Document Generation Request Rejected!";
                var body = $"The document \"{contract.Name}\" has been cancelled by ({rejectActor}). You will no longer be able to access and perform any action on the document.";
                var buttonText = "";

                var list = new List<EmailVm>(); 
                foreach (var recipient in recipients) //Send email to all the contract generators
                {
                    list.Add(new EmailVm
                    {
                        Subject = subject,
                        RecipientName = recipient.Email,
                        Body = body,
                        RecipientEmail = recipient.Email,
                        ButtonText = buttonText,
                        ButtonLink = ""
                    });
                }
                //add initiator to the email list
                list.Add(await this.ComposeEmailToInitiator(contract, subject, body, buttonText));
                await _emailService.SendBulkEmail(list);
                await this.CreateInboxes(request, list);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }


        internal async Task DeleteDocumentDimensionsForRejectedDocument(Domain.Entities.Contract contract,List<Dimension> dimensions, CancellationToken cancellationToken)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task<EmailVm> ComposeEmailToInitiator(Domain.Entities.Contract contract, string subject, string body, string buttonText = "")
        {
            try
            {
                var email = new EmailVm
                {
                    Subject = subject,
                    Body = body,
                    ButtonText = buttonText,
                    RecipientName = initiator?.Entity?.Name,
                    RecipientEmail = contract.CreatedByEmail
                };

                return email;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal async Task CreateInbox(LogRecipientActionCommand request, EmailVm email)
        {
            try
            {
                var command = new CreateInboxCommand
                {
                    AccessToken = request.AccessToken,
                    OrganisationName = request.OrganisationName,
                    OrganisationId = request.OrganisationId,
                    Body = email.Text,
                    Name = email.Subject,
                    Delivered = false,
                    RecipeintEmail = email.RecipientEmail,
                    EmailAction = EmailAction.Received,
                    UserId = request.UserId
                };
                await new CreateInboxCommandHandler(_context, _mapper, _authService).Handle(command, new CancellationToken());
            }
            catch (Exception ex)
            {

            }
        }

        internal async Task<string> CreateInboxes(LogRecipientActionCommand request, List<EmailVm> emails)
        {
            try
            {
                var inboxes = new List<CreateInboxRequest>();

                foreach (var email in emails) //Send email to all the contract generators
                {
                    if (email != null)
                    {
                        inboxes.Add(new CreateInboxRequest
                        {
                            Name = email.Subject,
                            ReciepientEmail = email.RecipientEmail,
                            Body = email.Body,
                            EmailAction = EmailAction.Received,
                            Delivered = true,
                            OrganizationId = request.OrganisationId,
                            OrganizationName = request.OrganisationName,
                            UserId = request.UserId,
                        }); 
                    }
                }
                var command = new CreateInboxesCommand
                {
                    AccessToken = request.AccessToken,
                    OrganisationName = request.OrganisationName,
                    OrganisationId = request.OrganisationId,
                    Inboxes = inboxes,
                    UserId = request.UserId
                };
               var check= await new CreateInboxesCommandHandler(_context, _mapper, _authService).Handle(command, new CancellationToken());
                if (check.Succeeded==false)
                {
                    return check.Message;
                }
                return "Success";
                
            }
            catch (Exception ex)
            {
                return $"Inboxes creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}";
            }
        }

    }
}
