using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipientActions.Commands.LogRecipientAction;
using Onyx.ContractService.Domain.Constants;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Onyx.ContractService.Application.ContractDocuments.Commands.SaveSignedDocument
{
    public class SaveSignedDocumentCommand : AuthToken,IRequest<Result>
    {
        public string SigningAPIUrl { get; set; }
        public string SignedFile { get; set; }
    }

    public class SaveSignedDocumentCommandHandler : IRequestHandler<SaveSignedDocumentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IEmailService _emailService;
        public readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        public SaveSignedDocumentCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService,
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
        public async Task<Result> Handle(SaveSignedDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();

                var existingContractDocuments = await _context.ContractDocuments.Where(x => x.DocumentSigningUrl == request.SigningAPIUrl).ToListAsync();
                if (existingContractDocuments == null || existingContractDocuments.Count == 0)
                {
                    return Result.Failure($"No contract documents exist for signing");
                }
                var contractId = existingContractDocuments.FirstOrDefault().ContractId;

                //get contract document to update
               
                var signedDocument = existingContractDocuments.Where(a => a.IsSigned).FirstOrDefault();
                //get last recipient with his signing url to prevent duplicate urls
                var lastRecipientAction = await _context.ContractRecipientActions.Where(a => a.ContractId == contractId &&
                                                                          a.AppSigningUrl == request.SigningAPIUrl).FirstOrDefaultAsync();
                if (lastRecipientAction != null)
                {
                    return Result.Failure($"This document has been {GetActionMessage(lastRecipientAction.RecipientAction)} already");
                }
                ////log contract recipient action attached to this url
                var contractRecipients = await _context.ContractRecipients.Where(a => a.ContractId == contractId
                && (a.RecipientCategory == RecipientCategory.InternalSignatory.ToString() ||
                    a.RecipientCategory == RecipientCategory.ExternalSignatory.ToString())).ToListAsync();
                if (contractRecipients == null || contractRecipients.Count == 0)
                {
                    return Result.Failure($"No Contract Recipients Retrieved");
                }

                var contractRecipientForUpdate = contractRecipients.FirstOrDefault(a => a.DocumentSigningUrl == request.SigningAPIUrl);
                if (contractRecipientForUpdate == null)
                {
                    return Result.Failure($"Invalid Contract Recipient Retrieved !");
                }
                //ContractRecipient nextContractRecipient = null;
                //if (contractRecipientForUpdate != null)
                //{
                //    //log contract recipient action
                //    var contractRecipientAction = new ContractRecipientAction
                //    {
                //        AppSigningUrl = request.SigningAPIUrl,
                //        SignedDocumentUrl = request.SignedFile,
                //        CreatedDate = DateTime.Now,
                //        CreatedBy = contractRecipientForUpdate.Email,
                //        ContractId = contractId,
                //        Status = Status.Active,
                //        StatusDesc = Status.Active.ToString(),
                //        OrganisationId = contractRecipientForUpdate.OrganisationId,
                //        ContractRecipientId = contractRecipientForUpdate.Id,
                //        RecipientAction = RecipientAction.Sign.ToString()
                //    };

                //    nextContractRecipient = contractRecipients.FirstOrDefault(a => a.ContractId == contractId && a.Rank == contractRecipientForUpdate.Rank + 1);
                //    await _context.ContractRecipientActions.AddAsync(contractRecipientAction);
                //    await _context.SaveChangesAsync(cancellationToken);
                //}
                //get all signed documents to update the version
                //var unsignedVersion = await _context.ContractDocuments.Where(a => a.DocumentSigningUrl == request.SigningAPIUrl && !a.IsSigned).FirstOrDefaultAsync();
                //var contractDocuments = await _context.ContractDocuments.Where(a => a.ContractId == contractId
                //                        && a.DocumentSigningUrl != request.SigningAPIUrl && a.IsSigned).ToListAsync();
                //var version = 0;
                //if (contractDocuments != null)
                //{
                //    version = contractDocuments.Count();
                //}
                var emailResponse = "";
                var contractDocument = new ContractDocument
                {
                    ContractId = contractId,
                    MimeType = MimeTypes.Pdf,
                    Extension = MimeTypes.Pdf.Split('/', StringSplitOptions.None)[1],
                    CreatedBy = contractRecipientForUpdate.Email,
                    Email = contractRecipientForUpdate.Email,
                    OrganisationId = contractRecipientForUpdate.OrganisationId,
                    OrganisationName = contractRecipientForUpdate.OrganisationName,
                    CreatedDate = DateTime.Now,
                    File = request.SignedFile,
                    IsSigned = true,
                    Version = "1",
                    DocumentSigningUrl = request.SigningAPIUrl,
                };
                await _context.ContractDocuments.AddAsync(contractDocument);
                var logAction = new LogRecipientActionCommand
                {
                    AppSigningUrl = request.SigningAPIUrl,
                    ContractId = contractId,
                    ContractRecipientId = contractRecipientForUpdate.Id,
                    OrganisationId = contractRecipientForUpdate.OrganisationId,
                    OrganisationName = contractRecipientForUpdate.OrganisationName,
                    RecipientAction = RecipientAction.Sign,
                    SignedDocumentUrl = request.SignedFile,
                    UserId = contractRecipientForUpdate.Email,
                    AccessToken = request.AccessToken
                };

                var result = await new LogRecipientActionCommandHandler(_context, _mapper, _blobStorageService, _emailService, _configuration, _authService,_mediator,_notificationService)
                    .LogAction(logAction, cancellationToken);
                if (!result.Succeeded)
                {
                    return Result.Failure(result.Message);
                }

                ////send email to legal since this is the last
                //if (nextContractRecipient == null)
                //{
                //    //get contract and update it to active since all recipients have signed
                //    var contractToUpdate = await _context.Contracts.FirstOrDefaultAsync(a => a.Id == contractRecipientForUpdate.ContractId);
                //    contractToUpdate.ContractStatus = ContractStatus.Active;
                //    _context.Contracts.Update(contractToUpdate);
                //    await _context.SaveChangesAsync(cancellationToken);

                //    var legalEmail = contractRecipients.FirstOrDefault(a => a.RecipientCategory == RecipientCategory.ContractGenerator.ToString()).Email;

                //    var email = new EmailVm
                //    {
                //        Subject = "Documents for Signing",
                //        Text = $"Document Recipients have signed successfully.",
                //        RecipientEmail = legalEmail,
                //        ButtonText = "View Document",
                //        ButtonLink = request.SignedFile
                //    };
                //    emailResponse = await _emailService.SendEmail(email);
                //}
                //else
                //{
                //    // get dimensions and update file value for next recipient
                //    // var hash = nextContractRecipient.DocumentSigningUrl.Split('=')[1];
                //    //var dimension = await _context.Dimensions.Where(a => a.Hash == hash).FirstOrDefaultAsync();
                //    //if (dimension != null)
                //    //{
                //    //    dimension.File = request.SignedFile;
                //    //    _context.Dimensions.Update(dimension);
                //    //}
                //    //await _context.SaveChangesAsync(cancellationToken);
                //    var email = new EmailVm
                //    {
                //        Subject = "Documents for Signing",
                //        Body = $"You have received a document for signing!",
                //        RecipientEmail = nextContractRecipient.Email,
                //        ButtonText = "View Document",
                //        ButtonLink = unsignedVersion.File
                //    };
                //    emailResponse = await _emailService.SendEmail(email);
                //}

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Document signed and saved successfully!", new { NextRecipientEmailResponse = emailResponse });
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Document signing and saving failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        private string GetActionMessage(string recipientAction)
        {
            var action = "";
            switch (recipientAction)
            {
                case "Approve":
                    action = "approved";
                    break;
                case "Sign":
                    action = "signed";
                    break;
                case "Reject":
                    action = "rejected";
                    break;
                case "Cancel":
                    action = "cancelled";
                    break;
                default:
                    break;
            }
            return action;
        }
    }
}
