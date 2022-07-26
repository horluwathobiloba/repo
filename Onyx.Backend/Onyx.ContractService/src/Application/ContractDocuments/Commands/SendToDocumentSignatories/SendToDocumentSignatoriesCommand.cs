using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipientActions.Commands.LogRecipientAction;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Application.Inboxs.Commands.CreateInbox;
using Onyx.ContractService.Domain.Constants;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractDocuments.Commands.SendToDocumentSignatories
{
    public class SendToDocumentSignatoriesCommand : AuthToken, IRequest<Result>
    {
        public int ContractId { get; set; }
        public int OrganizationId { get; set; }
        public SigningRecipients[] RecipientDetails { get; set; }
        public string FilePath { get; set; }
        public string UserId { get; set; }

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
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);

                var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganizationId && x.Id == request.ContractId);
                if (contract == null)
                {
                    return Result.Failure($"Invalid Contract Specified");
                }
               
                //get contract recipients that are signatories tied to this contract id
                var contractRecipients = await _context.ContractRecipients.Where(x => x.ContractId == request.ContractId &&
                (x.RecipientCategory == RecipientCategory.InternalSignatory.ToString() || x.RecipientCategory == RecipientCategory.ExternalSignatory.ToString())).ToListAsync();
                if (contractRecipients == null || contractRecipients.Count == 0)
                {
                    return Result.Failure($"No Contract Recipients Retrieved");
                }
                //get contract documents that have have been created but unsigned so i dont'save twice
                var contractDocuments = await _context.ContractDocuments.Where(a => a.ContractId == request.ContractId &&
                                        a.OrganisationId == request.OrganizationId && a.Version == "Unsigned").ToListAsync();
                //get last recipient that signed , is this rejected ? then skip 
                var lastRejectRecipient = await _context.ContractRecipientActions.Where(a => a.ContractId == request.ContractId &&
                                                                          a.OrganisationId == request.OrganizationId &&
                                                                          a.RecipientAction == RecipientAction.Reject.ToString()).FirstOrDefaultAsync();

                //loop through the list and save dimensions
                List<ContractRecipientsVm> recipientDetails = new List<ContractRecipientsVm>();
                var list = new List<EmailVm>();
                await _context.BeginTransactionAsync();
                foreach (var detail in request.RecipientDetails)
                {
                    if (lastRejectRecipient == null)
                    {
                        if (contractDocuments != null && contractDocuments.Count > 0)
                        {
                            var uploadedContractDocument = contractDocuments.FirstOrDefault(a => a.Email == detail.RecipientEmail);
                            if (uploadedContractDocument != null)
                            {
                                if (!string.IsNullOrWhiteSpace(uploadedContractDocument.Email))
                                {
                                    return Result.Failure($"Documents has been sent to {detail.RecipientEmail} for signing already");
                                };
                            }

                        } 
                    }
                    var recipient = contractRecipients.FirstOrDefault(a => a.Email == detail.RecipientEmail && 
                    (a.RecipientCategory == RecipientCategory.InternalSignatory.ToString() || a.RecipientCategory == RecipientCategory.ExternalSignatory.ToString()));
                    if ( recipient != null)
                    {
                        var emailResponse = "";
                        var hash = (detail.RecipientEmail + DateTime.Now.Ticks).ToString();
                        hash = _stringHashingService.CreateDESStringHash(hash);
                        ContractDocument document = new ContractDocument
                        {
                            ContractId = request.ContractId,
                            MimeType = MimeTypes.Pdf,
                            Extension = MimeTypes.Pdf.Split('/', StringSplitOptions.None)[1],
                            CreatedBy = detail.RecipientEmail,
                            Email = detail.RecipientEmail,
                            OrganisationId = request.OrganizationId,
                            CreatedDate = DateTime.Now,
                            File = request.FilePath,
                            Version = "Unsigned",
                            Status = Domain.Enums.Status.Active,
                            StatusDesc = Domain.Enums.Status.Active.ToString(),
                            IsSigned = false,
                            Hash = hash
                        };
                        document.DocumentSigningUrl = _configuration["WebUrlSign"] + "?key=" + document.Hash;
                        await _context.ContractDocuments.AddAsync(document);

                        foreach (var item in detail.Dimensions)
                        {
                            Dimension dimension = new Dimension
                            {
                                ContractId = request.ContractId,
                                Email = detail.RecipientEmail,
                                File = request.FilePath,
                                Height = item.Height,
                                Top = item.Top,
                                Left = item.Left,
                                OrganizationId = request.OrganizationId,
                                Type = item.Type,
                                Width = item.Width,
                                Hash = hash,
                                Rank =  recipient.Rank
                            };
                            await _context.Dimensions.AddAsync(dimension);
                        }
                        //log recipient action for contract generator
                        //send email for just the first person
                        var firstRecipient = contractRecipients.FirstOrDefault(a => (a.RecipientCategory == RecipientCategory.InternalSignatory.ToString()
                                             || a.RecipientCategory == RecipientCategory.ExternalSignatory.ToString()));
                        //update signing url of contract recipients 
                        recipient.DocumentSigningUrl = document.DocumentSigningUrl;
                        _context.ContractRecipients.Update(recipient);
                        if (detail.RecipientEmail == firstRecipient.Email)
                        {
                            try
                            {
                                list.Add(new EmailVm
                                {
                                    Subject = "Contract Document Request for Signing",
                                    Body = $"You have received a document request for you to sign on the contract:{contract.Name}! Kindly click the button below to sign the document.",
                                    RecipientEmail = detail.RecipientEmail,
                                    ButtonText = "Sign Document Now!",
                                    ButtonLink = document.DocumentSigningUrl
                                });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        recipientDetails.Add(new ContractRecipientsVm { Email = detail.RecipientEmail, SigningAppUrl = document.DocumentSigningUrl, EmailResponse = emailResponse });
                    }

                }
                await _context.SaveChangesAsync(cancellationToken);
                //log contractgenerator's action and send email for update of vendor details
                var contractGenerator = await _context.ContractRecipients.Where(x => x.ContractId == request.ContractId &&
                                         x.RecipientCategory == RecipientCategory.ContractGenerator.ToString()).FirstOrDefaultAsync();
                if (contractGenerator != null)
                {
                    //get vendor details and send email if vendor is detail is incomplete
                    if (contract.VendorId > 0)
                    {

                        var vendor = await _context.Vendors.Where(a => a.Id == contract.VendorId && a.OrganisationId == request.OrganizationId).FirstOrDefaultAsync();
                        if (vendor != null)
                        {
                            if (vendor.RCNumber == null || vendor.Address == null)
                            {
                                list.Add(new EmailVm
                                {
                                    Subject = "Incomplete Vendor Details",
                                    Body = $"You are receiving this email because the details for Vendor; {vendor.Name} on contract: {contract.Name} is incomplete. Kindly update this on vendor configurations module" +
                                             $" for a more fluid contract initiation request.",
                                    RecipientEmail = contractGenerator.Email,
                                    ButtonText = "Login!",
                                    ButtonLink =  _configuration["LoginPage"]
                            });
                            }
                        }
                    }
                    var logAction = new LogRecipientActionCommand
                    {
                        AppSigningUrl = "",
                        ContractId = contract.Id,
                        ContractRecipientId = contractGenerator.Id,
                        OrganisationId = contractGenerator.OrganisationId,
                        OrganisationName = contractGenerator.OrganisationName,
                        RecipientAction = RecipientAction.Approve,
                        SignedDocumentUrl = "",
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };
                    var result = await new LogRecipientActionCommandHandler(_context, _mapper, _blobStorageService, _emailService, _configuration, _authService, _mediator, _notificationService)
                    .LogAction(logAction, cancellationToken);
                }
                //send emails and inboxes
                 await _emailService.SendBulkEmail(list);
                 await this.CreateInboxes(request, list);
                await _context.CommitTransactionAsync();
                if (recipientDetails.Count == 0)
                {
                    return Result.Failure($"Invalid Contract Recipients Specified");
                }

                return Result.Success("Sending Document for signing was successfull!", recipientDetails);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Sending Document for signing failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        internal async Task CreateInbox(SendToDocumentSignatoriesCommand request, EmailVm email)
        {
            try
            {
                var command = new CreateInboxCommand
                {
                    Email="",
                    AccessToken = request.AccessToken,
                   UserId=request.UserId,
                    OrganisationId = request.OrganizationId,
                    Body = email.Text,
                    Name = email.Subject,
                    Delivered = false,
                    RecipeintEmail = email.RecipientEmail,
                    EmailAction = EmailAction.Received,
                    
                };
                await new CreateInboxCommandHandler(_context, _mapper, _authService).Handle(command, new CancellationToken());
            }
            catch (Exception ex)
            {

            }
        }
        internal async Task CreateInboxes(SendToDocumentSignatoriesCommand request, List<EmailVm> emails)
        {
            try
            {
                var inboxes = new List<CreateInboxRequest>();
                foreach (var email in emails) //Send email to all the contract generators
                {
                    inboxes.Add(new CreateInboxRequest
                    {
                        Name = email.Subject,
                        ReciepientEmail = email.RecipientEmail,
                        Body = email.Body,
                        EmailAction = EmailAction.Received,
                        Delivered = true,
                        OrganizationId = request.OrganizationId,
                        UserId = request.UserId,
                        Email=""
                    });
                }
                var command = new CreateInboxesCommand
                {
                    AccessToken = request.AccessToken,
                   
                    OrganisationId = request.OrganizationId,
                    Inboxes = inboxes,
                    UserId = request.UserId
                };
                await new CreateInboxesCommandHandler(_context, _mapper, _authService).Handle(command, new CancellationToken());
            }
            catch (Exception ex)
            {

            }
        }


    }

}
