using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Commands.UpdateContract
{
    public class UpdateContractCommand_Old : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DocumentType DocumentType { get; set; }
        public int ContractTypeId { get; set; }
        public int VendorId { get; set; }
        public int VendorTypeId { get; set; }
        public int ProductServiceTypeId { get; set; }
        public int PaymentPlanId { get; set; }
        public bool IsComplianceDueDiligence { get; set; }
        public bool IsTouchPointVendor { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractExpirationDate { get; set; }
        public int ContractDurationId { get; set; }
        public decimal ContractValue { get; set; }
        public string Signature { get; set; }
        public string UserId { get; set; }

        #region Vendor Details
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SupplierClass { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        #endregion 

        public FileUploadRequest ExecutedContractFile { get; set; } = new FileUploadRequest();
        public FileUploadRequest InitiatorSignatureFile { get; set; } = new FileUploadRequest();
        public List<FileUploadRequest> SupportingDocuments { get; set; }
    }

    public class UpdateContractCommand_OldHandler : IRequestHandler<UpdateContractCommand_Old, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;

        public UpdateContractCommand_OldHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
        }
        public async Task<Result> Handle(UpdateContractCommand_Old request, CancellationToken cancellationToken)
        {
            try
            {
                //check if the name of the other record has the new name
                var UpdatedNameExists = await _context.Contracts.AnyAsync(x => x.OrganisationId == request.OrganisationId
                && x.Id != request.Id && x.Name.ToLower() == request.Name.ToLower());

                if (UpdatedNameExists)
                {
                    return Result.Failure($"A record with this contract {request.Name} already exists. Please change the name.");
                }

                var entity = await _context.Contracts
                    .Where(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid contract type specified.");
                }

                entity.InitiatorSignature = await request.InitiatorSignatureFile.SaveBlobFile(request.Name, _blobService);
                entity.InitiatorSignatureFileExtension = request.InitiatorSignatureFile.FileExtension;
                entity.InitiatorSignatureMimeType = request.InitiatorSignatureFile.FileMimeType;

                entity.ExecutedContract = await request.ExecutedContractFile.SaveBlobFile(request.Name, _blobService);
                entity.ExecutedContractFileExtension = request.ExecutedContractFile.FileExtension;
                entity.ExecutedContractMimeType = request.ExecutedContractFile.FileMimeType;

                entity.ContractExpirationDate = request.ContractExpirationDate;
                entity.ContractStartDate = request.ContractStartDate;
                entity.ContractTypeId = request.ContractTypeId;
                entity.ContractValue = request.ContractValue;
                entity.Description = request.Description;
                entity.Name = request.Name;
                entity.IsComplianceDueDiligence = request.IsComplianceDueDiligence;
                entity.IsTouchPointVendor = request.IsTouchPointVendor;
                entity.PaymentPlanId = request.PaymentPlanId;
                entity.ProductServiceTypeId = request.ProductServiceTypeId;
                entity.InitiatorSignature = request.Signature;
                entity.VendorId = request.VendorId;
                entity.VendorTypeId = request.VendorTypeId;
                entity.DocumentTypeDesc = request.DocumentType.ToString();
                entity.DocumentType = request.DocumentType;
                entity.SupplierClass = request.SupplierClass;
                entity.Country = request.Country;
                entity.State = request.State;
                entity.Email = request.Email;

                entity.OrganisationId = request.OrganisationId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.BeginTransactionAsync();

                _context.Contracts.Update(entity);

                if (request.SupportingDocuments != null && request.SupportingDocuments.Count > 0)
                {
                    var list = new List<SupportingDocument>();
                    foreach (var x in request.SupportingDocuments)
                    {
                        var supportingDoc = await _context.SupportingDocuments
                            .Where(a => a.OrganisationId == request.OrganisationId && a.Id == x.DocumentId)
                            .FirstOrDefaultAsync();

                        // save each of the documents on blob server and create the contract document in the db
                        var val = await this.Convert(entity, x, supportingDoc);
                        if (val != null)
                        {
                            list.Add(val);
                        }
                    }
                    _context.SupportingDocuments.UpdateRange(list);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                return Result.Success("Executed contract was updated successfully");
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Executed contract update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        private async Task<SupportingDocument> Convert(Domain.Entities.Contract contract, FileUploadRequest item, SupportingDocument entity)
        {
            dynamic blob = await item.SaveBlobFile(contract.Name, _blobService);

            if (string.IsNullOrEmpty(blob))
            {
                return null;
            }

            if (entity == null)
            {
                entity = new SupportingDocument
                {
                    Id = item.DocumentId,
                    ContractId = contract.Id,
                    Extension = item.FileExtension,
                    OrganisationId = contract.OrganisationId,
                    File = blob.blobUrl,
                    MimeType = item.FileMimeType,
                    Name = blob.newFileName,
                    Description = blob.newFileName,

                    CreatedBy = contract.CreatedBy,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = contract.LastModifiedBy,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };
            }
            else
            {
                entity.ContractId = contract.Id;
                entity.Extension = item.FileExtension;
                entity.OrganisationId = contract.OrganisationId;
                entity.File = blob.blobUrl;
                entity.MimeType = item.FileMimeType;
                entity.Name = blob.newFileName;
                entity.Description = blob.newFileName;
                entity.LastModifiedBy = contract.LastModifiedBy;
                entity.LastModifiedDate = DateTime.Now;
                entity.Status = Status.Active;
                entity.StatusDesc = Status.Active.ToString();
            }
            return entity;

        }

    }

}


