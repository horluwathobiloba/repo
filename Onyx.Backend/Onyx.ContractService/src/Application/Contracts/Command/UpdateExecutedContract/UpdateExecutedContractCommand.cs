using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Commands.UpdateExecutedContract
{
    public class UpdateExecutedContractCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DocumentType DocumentType { get; set; }

        public int ContractTypeId { get; set; }
        public int PermitTypeId { get; set; }
        public int VendorId { get; set; }
        public int VendorTypeId { get; set; }
        public int ProductServiceTypeId { get; set; }
        public int PaymentPlanId { get; set; }
        public bool IsComplianceDueDiligence { get; set; }
        public bool IsTouchPointVendor { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractExpirationDate { get; set; }
        public decimal ContractValue { get; set; }
        public int ContractDuration { get; set; }
        public string Currency { get; set; }
        public List<string> ReminderRecipients { get; set; }
        public FileUploadRequest ExecutedContractFile { get; set; } = new FileUploadRequest();
        public List<FileUploadRequest> SupportingDocuments { get; set; }

        public string UserId { get; set; }

        #region Vendor Details
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SupplierClass { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        //[DefaultValue(false)]
        public bool TouchPointVendor { get; set; }
        public string Address { get; set; }
        public string RCNumber { get; set; }
        #endregion  
    }

    public class UpdateExecutedContractCommandHandler : IRequestHandler<UpdateExecutedContractCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IAuthService _authService;

        public UpdateExecutedContractCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateExecutedContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user==null)
                {
                    return Result.Failure("UserId is not found");
                }
                //check if the name of the other record has the new name
                var updatedNameExists = await _context.Contracts
                    .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id && x.Name.ToLower() == request.Name.ToLower());

                if (updatedNameExists)
                {
                    return Result.Failure($"An executed document with the record name '{request.Name}' already exists. Please change the name.");
                }

                var entity = await _context.Contracts
                    .Where(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Record not found! Invalid executed contract type specified.");
                }

                //to get the old values of the contract before updating.
                
                var oldValuesEntity = entity;

                if (entity.ContractStatus.EqualTo(ContractStatus.Active.ToString()) == false)
                {
                    return Result.Failure($"Contract status must be active in order to update this contract.");
                }
                var file = request.ExecutedContractFile.FileBase64String;
                if (!string.IsNullOrWhiteSpace(file))
                {
                    if (file.Contains("https://") == false)
                    {
                        if (!string.IsNullOrWhiteSpace(request.ExecutedContractFile.FileExtension)
                       && !string.IsNullOrWhiteSpace(request.ExecutedContractFile.FileMimeType))
                        {
                            entity.ExecutedContract = await request.ExecutedContractFile.SaveBlobFile(request.Name, _blobService);
                            entity.ExecutedContractFileExtension = request.ExecutedContractFile.FileExtension;
                            entity.ExecutedContractMimeType = request.ExecutedContractFile.FileMimeType;
                        }
                    }

                }
                entity.ContractExpirationDate = request.ContractExpirationDate;
                entity.ContractStartDate = request.ContractStartDate;
                entity.ContractTypeId = request.ContractTypeId;
                entity.PermitTypeId = request.PermitTypeId;
                entity.ContractValue = request.ContractValue;
                entity.Description = request.Description;
                entity.Name = request.Name;
                entity.IsComplianceDueDiligence = request.IsComplianceDueDiligence;
                entity.IsTouchPointVendor = request.IsTouchPointVendor;
                entity.PaymentPlanId = request.PaymentPlanId;
                entity.ProductServiceTypeId = request.ProductServiceTypeId;
                entity.VendorId = request.VendorId;
                entity.VendorTypeId = request.VendorTypeId;
                entity.DocumentTypeDesc = request.DocumentType.ToString();
                entity.DocumentType = request.DocumentType;
                entity.SupplierClass = request.SupplierClass;
                entity.SupplierCode = request.SupplierCode;
                entity.ShortName = request.ShortName;
                entity.Country = request.Country;
                entity.State = request.State;
                entity.Email = request.Email;
                entity.OrganisationId = request.OrganisationId;
                entity.OrganisationName = _authService.Organisation?.Name;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                entity.Currency = request.Currency;
                entity.UserId = request.UserId;
                entity.CreatedByEmail = user.Entity.Email;
                entity.ContactEmail = request.ContactEmail;
                entity.RCNumber = request.RCNumber;
                entity.TouchPointVendor = request.TouchPointVendor;
                entity.PhoneNumber = request.PhoneNumber;
                entity.ContactPhoneNumber = request.ContactPhoneNumber;
                entity.Address = request.Address;
                entity.ContactName = request.ContactName;
                //placeholder for duration
                if (!entity.ContractExpirationDate.HasValue)
                {
                    entity.ContractExpirationDate = DateTime.Now.AddDays(request.ContractDuration);
                    
                }

                if (entity.ContractExpirationDate >= DateTime.Now)
                {
                    entity.ContractStatus = Domain.Enums.ContractStatus.Expired;
                }

                await _context.BeginTransactionAsync();

                _context.Contracts.Update(entity);

                //get existing reminder recipients
                var existingReminderRecipients = await _context.ReminderRecipients.
                    Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.Id).ToListAsync();

                var reminderRecipients = new List<ReminderRecipient>();
                if (request.ReminderRecipients != null && request.ReminderRecipients.Count > 0)
                {
                    foreach (var recipient in request.ReminderRecipients)
                    {
                        if (string.IsNullOrWhiteSpace(recipient) || (!recipient.Contains("@")))
                        {
                            continue;
                        }
                        if (existingReminderRecipients != null && existingReminderRecipients.Count > 0)
                        {
                            if (existingReminderRecipients.FirstOrDefault().Email == recipient)
                            {
                                continue;
                            }
                        }
                       
                        reminderRecipients.Add(new ReminderRecipient
                        {
                            ContractId = entity.Id,
                            Email = recipient,
                            OrganisationId = entity.OrganisationId,
                            OrganisationName = entity.OrganisationName,
                            CreatedBy = entity.CreatedBy,
                            CreatedDate = entity.CreatedDate,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        });
                    }
                    await _context.ReminderRecipients.AddRangeAsync(reminderRecipients);
                }
              
                #region Supporting Documents 
                if (request.SupportingDocuments != null && request.SupportingDocuments.Count > 0)
                {
                    var list = new List<SupportingDocument>();
                    foreach (var x in request.SupportingDocuments)
                    {
                        // save each of the documents on blob server and create the contract document in the db
                        var document = await this.Convert(x, entity);
                        if (document != null && document.Name != null)
                        {
                            list.Add(document);
                        }
                    }
                    _context.SupportingDocuments.AddRange(list);
                }
                await _context.SaveChangesAsync(cancellationToken);
                #endregion

                //create audit log
                if (entity.DocumentType == DocumentType.Contract)
                {
                    var newValuesEntity = entity;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = entity.OrganisationName,
                        LastModifiedBy = request.UserId,
                        RoleId = entity.RoleId,
                        RoleName = entity.RoleName,
                        UserId = request.UserId,
                        FirstName = user.Entity.FirstName,
                        LastName = user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Contract.ToString(),
                        OldValue = oldValuesEntity,
                        NewValue = newValuesEntity,
                        Action = AuditType.Update.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }

                if (entity.DocumentType == DocumentType.Permit)
                {
                    var newValuesEntity = entity;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = entity.OrganisationName,
                        RoleId = entity.RoleId,
                        RoleName = entity.RoleName,
                        UserId = request.UserId,
                        FirstName = user.Entity.FirstName,
                        LastName = user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Permit.ToString(),
                        OldValue = oldValuesEntity,
                        NewValue = newValuesEntity,
                        Action = AuditType.Update.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
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

        private async Task<SupportingDocument> Convert(FileUploadRequest item, Domain.Entities.Contract contract)
        {
            var blobUrl = await item.SaveBlobFile(contract.Name, _blobService);

            if (string.IsNullOrEmpty(blobUrl))
            {
                return null;
            }
            SupportingDocument entity = new SupportingDocument
            {
                ContractId = contract.Id,
                Extension = item.FileExtension,
                OrganisationId = contract.OrganisationId,
                OrganisationName =  contract.OrganisationName,
                File = blobUrl,
                MimeType = item.FileMimeType,
                Name = item.FileName,
                Description = item.FileName,

                CreatedBy = contract.CreatedBy,
                CreatedDate = DateTime.Now,
                LastModifiedBy = contract.LastModifiedBy,
                LastModifiedDate = DateTime.Now,
                Status = Status.Active,
                StatusDesc = Status.Active.ToString()
            };
            return entity;
        }

    }

}
