using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using OnyxDoc.DocumentService.Domain.Constants;

namespace OnyxDoc.DocumentService.Application.Document.Commands.UploadDocument
{
    public class UploadDocumentCommand : AuthToken, IRequest<Result>
    {
        public string MimeType { get; set; }
        public string Name { get; set; }
        public DocumentType DocumentType { get; set; }
        public string File { get; set; }
        public string Extension { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public int SubscriberId { get; set; }
        public int FolderId { get; set; }
    }

    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public UploadDocumentCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }
        public async Task<Result> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                if (_authService.Subscriber == null)
                {
                    if (_authService.User == null)
                    {
                        return Result.Failure(new string[] { "Invalid User details specified." });
                    }
                }
                //getAllDocumentTypes on the system that are not supporting douments for version purposes
                //Todo convert word to pdf
                if (request.Extension == MimeTypes.Word)
                {

                }
                var fileName = request.Email.ToString() + "_" + DateTime.Now.Ticks + "." + request.Extension;
                var fileBytes = Convert.FromBase64String(request.File);
                var filePath = "";
                if (fileBytes.Length > 0)
                {
                    filePath = await _blobStorageService.UploadFileToBlobAsync(fileName, fileBytes, request.MimeType);
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    return Result.Success("Error uploading to filepath. Please contact support");
                }

                Domain.Entities.Document document = new Domain.Entities.Document
                {
                    MimeType = request.MimeType,
                    Extension = request.Extension,
                    CreatedBy = request.UserId,
                    Email = request.Email,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now,
                    Name = request.Name,
                    File = filePath,
                    DocumentMessage = "",
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    DocumentStatus = DocumentStatus.Draft,
                    DocumentStatusDesc = DocumentStatus.Draft.ToString(),
                    SubscriberName = _authService?.Subscriber?.SubscriberName,
                    SubscriberType = _authService?.Subscriber?.SubscriberTypeDesc,
                    DocumentType = request.DocumentType,
                    DocumentTypeDesc = request.DocumentType.ToString(),
                    FolderId = request.FolderId
                };
                await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync(cancellationToken);
                var documentId = document.Id;
                return Result.Success("Document uploaded and created successfully!", new { File = filePath, DocumentId = documentId });
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document upload failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
