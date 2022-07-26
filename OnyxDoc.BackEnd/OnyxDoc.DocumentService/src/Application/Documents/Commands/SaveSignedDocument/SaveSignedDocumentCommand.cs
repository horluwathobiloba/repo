using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.DocumentService.Application.RecipientActions.Commands.LogRecipientAction;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OnyxDoc.DocumentService.Application.Documents.Commands.SaveSignedDocument
{
    public class SaveSignedDocumentCommand : AuthToken,IRequest<Result>
    {
        public int DocumentId { get; set; }
        public string SignedFile { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string RecipientEmail { get; set; }
        public string SigningAPIUrl { get; set; }
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
        public SaveSignedDocumentCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService,
            IStringHashingService stringHashingService, IEmailService emailService, 
            IConfiguration configuration, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _stringHashingService = stringHashingService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
        }
        public async Task<Result> Handle(SaveSignedDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var document = await _context.Documents.Where(x => x.Id == request.DocumentId).FirstOrDefaultAsync();
                if (document == null)
                {
                    return Result.Failure($"Invalid Document Details");
                }
                var documentMessage = document.DocumentMessage;
                //get last recipient with his signing url to prevent duplicate urls
                var lastRecipientAction = await _context.RecipientActions.Where(a => a.DocumentId == request.DocumentId &&
                                                                          a.AppSigningUrl == request.SigningAPIUrl).FirstOrDefaultAsync();
                if (lastRecipientAction != null)
                {
                    return Result.Failure($"This document has been signed already");
                }
                ////log document recipient action attached to this url
                var documentRecipients = await _context.Recipients.Where(a => a.DocumentId == request.DocumentId).ToListAsync();
                if (documentRecipients == null || documentRecipients.Count == 0)
                {
                    return Result.Failure($"No Recipients Retrieved");
                }

                var documentRecipientForUpdate = documentRecipients.FirstOrDefault(a => a.Email == request.RecipientEmail);
                if (documentRecipientForUpdate == null)
                {
                    return Result.Failure($"Invalid Recipient Retrieved !");
                }
                var signedFile = "";
                if (!request.SignedFile.Contains("https"))
                {
                    var fileBytes = Convert.FromBase64String(request.SignedFile);
                    if (fileBytes.Length > 0)
                    {
                        signedFile = await _blobStorageService.UploadFileToBlobAsync(request.RecipientEmail, fileBytes, request.MimeType);
                    }
                }
                else
                {
                    signedFile = request.SignedFile;
                }
               
               
               
                await _context.BeginTransactionAsync();
                var emailResponse = "";
                document.IsSigned = true;
                document.MimeType = request.MimeType;
                document.Extension = request.Extension;
                document.DocumentMessage = documentMessage;
                document.SignedDocument = signedFile;
                document.DocumentSigningUrl = request.SigningAPIUrl;
                document.LastModifiedDate = DateTime.Now;
                document.LastModifiedBy = documentRecipientForUpdate.Email;
                 _context.Documents.Update(document);
                await _context.SaveChangesAsync(cancellationToken);
                var logAction = new LogRecipientActionCommand
                {
                    AppSigningUrl = request.SigningAPIUrl,
                    DocumentId = request.DocumentId,
                    RecipientId = documentRecipientForUpdate.Id,
                    SubscriberId = documentRecipientForUpdate.SubscriberId,
                    SubscriberName = documentRecipientForUpdate.SubscriberName,
                    DocumentRecipientAction = DocumentRecipientAction.Sign,
                    SignedDocumentUrl = signedFile,
                    UserId = documentRecipientForUpdate.Email,
                    AccessToken = request.AccessToken
                };

                var result = await new LogRecipientActionCommandHandler(_context, _mapper, _blobStorageService, _emailService, _configuration, _authService)
                    .LogAction(logAction, cancellationToken);
                if (!result.Succeeded)
                {
                    return Result.Failure(result.Message);
                }

                await _context.CommitTransactionAsync();
                return Result.Success("Document signed and saved successfully!", new { NextRecipientEmailResponse = emailResponse });
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Document signing and saving failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
