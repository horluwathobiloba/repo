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

namespace Onyx.ContractService.Application.Contract.Commands.UploadSupportingDocument
{
    public class UploadSupportingDocument : AuthToken, IRequest<Result>
    {
        public int ContractId { get; set; }
        public string MimeType { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
    }

    public class UploadSupportingDocumentHandler : IRequestHandler<UploadSupportingDocument, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public UploadSupportingDocumentHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;

        }
        public async Task<Result> Handle(UploadSupportingDocument request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);

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
                    return Result.Success("Error uploading to blob server! Please contact support.");
                }

                SupportingDocument supportingDocument = new SupportingDocument
                {
                    ContractId = request.ContractId,
                    Extension = request.Extension,
                    CreatedBy = request.UserId,
                    OrganisationId = request.OrganizationId,
                    OrganisationName = _authService.Organisation.Name,
                    CreatedDate = DateTime.Now,
                    File = filePath,
                    MimeType = request.MimeType,
                    Description = request.Description,
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active
                };
                await _context.SupportingDocuments.AddAsync(supportingDocument);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Document uploaded and created successfully!", supportingDocument);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Documment upload failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
