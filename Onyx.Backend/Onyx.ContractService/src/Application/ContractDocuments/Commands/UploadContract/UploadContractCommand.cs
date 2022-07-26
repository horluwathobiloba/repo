using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Onyx.ContractService.Domain.Constants;

namespace Onyx.ContractService.Application.Contract.Commands.UploadContract
{
    public class UploadContractCommand : AuthToken, IRequest<Result>
    {
        public int ContractId { get; set; }
        public string MimeType { get; set; }
        public string File { get; set; }
        public string Extension { get; set; }
        public string UserId { get; set; }
        public string SignedDocumentUrl { get; set; }
        public string Email { get; set; }
        public int OrganizationId { get; set; }
    }

    public class UploadContractCommandHandler : IRequestHandler<UploadContractCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public UploadContractCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }
        public async Task<Result> Handle(UploadContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
               // var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);

                var contractExists = await _context.Contracts.AnyAsync(x => x.OrganisationId == request.OrganizationId && x.Id == request.ContractId);
                if (!contractExists)
                {
                    return Result.Failure($"Invalid Contract specified.");
                }
                //getAllContractTypes on the system that are not supporting douments for version purposes
                //Todo convert word to pdf
                if (request.Extension == MimeTypes.Word)
                {

                }
                var fileName = request.ContractId.ToString() + "_" + DateTime.Now.Ticks + "." + request.Extension;
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
                ////for versioning
                //string version = "";
                //var documentId = 0;
                //var getDocuments = await _context.ContractDocuments.Where(a => a.ContractId == request.ContractId && a.Email == request.Email
                //&& a.UserId == request.UserId && a.MimeType == request.mi).OrderByDescending(a => a.CreatedDate).ToListAsync();
                //if (getDocuments != null && getDocuments.Count > 0)
                //{
                //    version = getDocuments.Count().ToString();
                //    var lastUpdatedDocument = getDocuments.FirstOrDefault();
                //    lastUpdatedDocument.Version = version;
                //    lastUpdatedDocument.File = filePath;
                //    lastUpdatedDocument.LastModifiedBy = request.Email;
                //    lastUpdatedDocument.LastModifiedDate = DateTime.Now;
                //    documentId = lastUpdatedDocument.Id;
                //    _context.ContractDocuments.Update(lastUpdatedDocument);
                //    await _context.SaveChangesAsync(cancellationToken);
                //    return Result.Success("Document uploaded and updated successfully!", filePath);
                //}
                ContractDocument document = new ContractDocument
                {
                    ContractId = request.ContractId,
                    MimeType = request.MimeType,
                    Extension = request.Extension,
                    CreatedBy = request.UserId,
                    Email = request.Email,
                    OrganisationId = request.OrganizationId,
                    DocumentSigningUrl = request.SignedDocumentUrl,
                    CreatedDate = DateTime.Now,
                    File = filePath
                };
                var documentId = document.Id;
                await _context.ContractDocuments.AddAsync(document);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Document uploaded and created successfully!", new { File = filePath, ContractDocumentId = documentId });
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document upload failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
