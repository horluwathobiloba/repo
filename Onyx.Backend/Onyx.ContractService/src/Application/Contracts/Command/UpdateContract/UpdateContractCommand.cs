using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ConractService.Application.ReminderRecipients.Commands.UpdateReminderRecipients;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Application.ContractRecipients.Commands.CreateContractRecipients;
using Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipients;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Commands.UpdateContract
{
    public class UpdateContractCommand : AuthToken, IRequest<Result>
    {
        public int ContractId { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }

        public int InitiatorRoleId { get; set; }
        public string RoleName { get; set; }

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
        public decimal ContractValue { get; set; }
        public string Currency { get; set; }
        public FileUploadRequest ExecutedContractFile { get; set; } = new FileUploadRequest();
        public FileUploadRequest InitiatorSignatureFile { get; set; } = new FileUploadRequest();
        public List<FileUploadRequest> SupportingDocuments { get; set; }
        public List<UpdateContractRecipientRequest> ContractRecipients { get; set; }
        public int ContractDurationId { get; set; }
        public string UserId { get; set; }
        public List<UpdateReminderRecipientRequest> ReminderRecipients { get; set; }

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

    public class CreateContractCommandHandler : IRequestHandler<UpdateContractCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBlobStorageService _blobService;
        private readonly IAuthService _authService;

        public CreateContractCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator, IBlobStorageService blobService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _blobService = blobService;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user == null)
                {
                    return Result.Failure("User not found");
                }
                var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.ContractId);
                var contractDuration = await _context.ContractDurations.FirstOrDefaultAsync(c => c.Id == request.ContractDurationId);

                if (contract == null)
                {
                    return Result.Failure($"Contract with this id does not exists!");
                }

                //to get the old values of the contract before updating.
                var oldValuesEntity = contract;
                //var oldValuesEntity = new
                //{
                //    contractEntity = contract
                //};

                //save the blob file to azure blob server
                contract.InitiatorSignature = await request.InitiatorSignatureFile.SaveBlobFile(request.Name, _blobService);
                contract.InitiatorSignatureFileExtension = request.InitiatorSignatureFile.FileExtension;
                contract.InitiatorSignatureMimeType = request.InitiatorSignatureFile.FileMimeType;
                contract.ExecutedContract = await request.ExecutedContractFile.SaveBlobFile(request.Name, _blobService);
                contract.ExecutedContractFileExtension = request.ExecutedContractFile.FileExtension;
                contract.ExecutedContractMimeType = request.ExecutedContractFile.FileMimeType;

                //contract.ContractDuration = contractDuration;
                contract.ContractExpirationDate = contract.ComputeContractExpirationDate();
                contract.ContractStartDate = request.ContractStartDate;
                contract.ContractStatus = ContractStatus.Processing;
                contract.ContractStatusDesc = ContractStatus.Processing.ToString();
                contract.ContractTypeId = request.ContractTypeId;
                contract.ContractValue = request.ContractValue;
                contract.Description = request.Description;
                contract.Name = request.Name;
                contract.IsComplianceDueDiligence = request.IsComplianceDueDiligence;
                contract.IsTouchPointVendor = request.IsTouchPointVendor;
                contract.PaymentPlanId = request.PaymentPlanId;
                contract.ProductServiceTypeId = request.ProductServiceTypeId;
                contract.VendorId = request.VendorId;
                contract.VendorTypeId = request.VendorTypeId;
                contract.RoleId = request.InitiatorRoleId;
                contract.RoleName = request.RoleName;
                contract.DocumentTypeDesc = request.DocumentType.ToString();
                contract.DocumentType = request.DocumentType;
                contract.SupplierClass = request.SupplierClass;
                contract.SupplierCode = request.SupplierCode;
                contract.ShortName = request.ShortName;
                contract.Country = request.Country;
                contract.State = request.State;
                contract.Email = request.Email;
                contract.OrganisationId = request.OrganisationId;
                contract.OrganisationName = _authService.Organisation?.Name;
                contract.CreatedBy = request.UserId;
                contract.CreatedDate = DateTime.Now;
                contract.LastModifiedBy = request.UserId;
                contract.LastModifiedDate = DateTime.Now;
                contract.Status = Status.Active;
                contract.StatusDesc = Status.Active.ToString();
                contract.Currency = request.Currency;
                contract.ContractDurationId = request.ContractDurationId;
                contract.UserId = request.UserId;
                contract.CreatedByEmail = user.Entity.Email;
                contract.ContactEmail = request.ContactEmail;
                contract.RCNumber = request.RCNumber;
                contract.TouchPointVendor = request.TouchPointVendor;
                contract.PhoneNumber = request.PhoneNumber;
                contract.ContactPhoneNumber = request.ContactPhoneNumber;
                contract.ContactName = request.ContactName;
                contract.Address = request.Address;
                await _context.BeginTransactionAsync();
                _context.Contracts.Update(contract);
                await _context.SaveChangesAsync(cancellationToken); 

                if (request.ContractRecipients != null && request.ContractRecipients.Count > 0)
                {
                    var contractRecipients = request.ContractRecipients.OrderBy(a => a.RecipientCategory).ToList();

                    var command = new UpdateContractRecipientsCommand
                    {
                        ContractId = contract.Id,
                        ContractRecipients = contractRecipients,
                        OrganisationId = request.OrganisationId,
                        OrganisationName = _authService.Organisation?.Name,
                        UserId = request.UserId
                    };

                    var handler = new UpdateContractRecipientsCommandHandler(_context, _mapper);
                    var recipientsResult = await handler.Handle(command, cancellationToken);

                    //  var recipientsResult = _mediator.Send(command).Result;

                    if (recipientsResult.Succeeded == false)
                    {
                        throw new Exception(recipientsResult.Error + recipientsResult.Message);
                    }
                }
                //Reminder Recipients

                if (request.ReminderRecipients != null && request.ReminderRecipients.Count > 0)
                {
                    
                    var command = new UpdateReminderRecipientsCommand
                    {
                        ContractId = contract.Id,
                        OrganisationId = request.OrganisationId,
                        ReminderRecipients = request.ReminderRecipients,
                        UserId = request.UserId,
                        
                    };

                    var handler = new UpdateReminderRecipientsCommandHandler(_context, _mapper);
                    var reminderRecipientsResult = await handler.Handle(command, cancellationToken);

                    //  var recipientsResult = _mediator.Send(command).Result;

                    if (reminderRecipientsResult.Succeeded == false)
                    {
                        throw new Exception(reminderRecipientsResult.Error + reminderRecipientsResult.Message);
                    }
                }
                #region Supporting Documents 
                if (request.SupportingDocuments != null && request.SupportingDocuments.Count > 0)
                {
                    var list = new List<SupportingDocument>();
                    foreach (var x in request.SupportingDocuments)
                    {
                        // save each of the documents on blob server and create the contract document in the db
                        var document = await this.Convert(x, contract);
                        if (document != null && document.Name != null)
                        {
                            list.Add(document);
                        }
                    }
                    _context.SupportingDocuments.AddRange(list);
                }
                await _context.SaveChangesAsync(cancellationToken);
                #endregion

                if (contract.DocumentType == DocumentType.Contract)
                {
                    //var newValuesEntity = new
                    //{
                    //    contract
                    //};
                    var newValuesEntity = contract;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        LastModifiedBy = request.UserId,
                        UserId = request.UserId,
                        RoleId = request.InitiatorRoleId,
                        RoleName = request.RoleName,
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

                if (contract.DocumentType == DocumentType.Permit)
                {
                    //var newValuesEntity = new
                    //{
                    //    contract
                    //};
                    var newValuesEntity = contract;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        RoleId = request.InitiatorRoleId,
                        UserId = request.UserId,
                        RoleName = request.RoleName,
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

                var result = _mapper.Map<ContractDto>(contract);
                return Result.Success("Contract request updated successfully!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Contract request update failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
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
                OrganisationName =  _authService.Organisation?.Name,
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
