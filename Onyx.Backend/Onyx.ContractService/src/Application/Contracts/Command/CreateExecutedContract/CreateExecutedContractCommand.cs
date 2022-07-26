using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
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

namespace Onyx.ContractService.Application.Contracts.Commands.CreateContract
{
    public class CreateExecutedContractCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int InitiatorRoleId { get; set; }
        public string RoleName { get; set; }
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
        //public int ContractDuration { get; set; }
        public FileUploadRequest ExecutedContractFile { get; set; } = new FileUploadRequest();
        public List<string> ReminderRecipients { get; set; }
        public FileUploadRequest InitiatorSignatureFile { get; set; } = new FileUploadRequest();
        public List<FileUploadRequest> SupportingDocuments { get; set; }
        public string Currency { get; set; }
        public List<ContractCommentRequest> Comments { get; set; }

        public List<ContractTagRequest> Tags { get; set; }

        public List<ContractTaskRequest> Tasks { get; set; }

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

    public class CreateExecutedContractCommandHandler : IRequestHandler<CreateExecutedContractCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private IEmailService _emailService;

        public CreateExecutedContractCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService,
            IConfiguration configuration,IAuthService authService,IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _authService = authService;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<Result> Handle(CreateExecutedContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user==null)
                {
                    return Result.Failure("User not found");
                }

                var exists = await (from x in _context.Contracts
                                    where x.OrganisationId == request.OrganisationId
                                    && x.Name.ToLower().Trim() == request.Name.ToLower().Trim()
                                    select x).AnyAsync();
                if (exists)
                {
                    return Result.Failure($"An executed document with record name ' {request.Name}' already exists!");
                }

                if (request.ContractStartDate!=null)
                {
                    if (request?.ContractStartDate.Value.DaysBetween(request.ContractExpirationDate.Value) <= 0)
                    {
                        return Result.Failure($"Contract Expiration Date: {request.ContractExpirationDate.Value} cannot be less than Contract Start Date {request.ContractStartDate.Value}");
                    }

                }

                var entity = new Domain.Entities.Contract
                {
                    //save the blob file to azure blob server
                    InitiatorSignature = request.InitiatorSignatureFile == null ? "" : await request.InitiatorSignatureFile.SaveBlobFile(request.Name, _blobService),
                    InitiatorSignatureFileExtension = request.InitiatorSignatureFile?.FileExtension,
                    InitiatorSignatureMimeType = request.InitiatorSignatureFile?.FileMimeType,

                    ExecutedContract = request.ExecutedContractFile == null ? "" : await request.ExecutedContractFile.SaveBlobFile(request.Name, _blobService),
                    ExecutedContractFileExtension = request.ExecutedContractFile?.FileExtension,
                    ExecutedContractMimeType = request.ExecutedContractFile?.FileMimeType,

                    ContractExpirationDate = request.ContractExpirationDate,
                    ContractStartDate = request.ContractStartDate,
                    ContractStatus = request.ContractExpirationDate < DateTime.Now ? ContractStatus.Expired : ContractStatus.Active,
                    ContractTypeId = request.ContractTypeId,
                    ContractValue = request.ContractValue,
                    Description = request.Description,
                    Name = request.Name,
                    PermitTypeId = request.PermitTypeId,
                    IsComplianceDueDiligence = request.IsComplianceDueDiligence,
                    IsTouchPointVendor = request.IsTouchPointVendor,
                    PaymentPlanId = request.PaymentPlanId,
                    Currency = request.Currency,

                    ProductServiceTypeId = request.ProductServiceTypeId,
                    VendorId = request.VendorId,
                    VendorTypeId = request.VendorTypeId,
                    RoleId = request.InitiatorRoleId,
                    RoleName = request.RoleName,
                    DocumentType = request.DocumentType,
                    DocumentTypeDesc = request.DocumentType.ToString(),
                    SupplierClass = request.SupplierClass,
                    SupplierCode = request.SupplierCode,
                    ShortName = request.ShortName,
                    Country = request.Country,
                    State = request.State,
                    Email = request.Email,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    IsAnExecutedDocument = true,
                    CreatedByEmail = user.Entity.Email,
                    UserId=request.UserId,
                    ContactEmail=request.ContactEmail,
                    ContactName = request.ContactName,
                    RCNumber=request.RCNumber,
                    TouchPointVendor=request.TouchPointVendor,
                    PhoneNumber=request.PhoneNumber,
                    ContactPhoneNumber=request.ContactPhoneNumber,
                    Address=request.Address,
                    
                };
                //placeholder for duration
                //if (!entity.ContractExpirationDate.HasValue)
                //{
                //    entity.ContractExpirationDate = DateTime.Now.AddDays(request.ContractDuration);
                //}
                entity.ContractStatusDesc = entity.ContractStatus.ToString();
                await _context.BeginTransactionAsync();
                await _context.Contracts.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken); //save the contract

                var reminderRecipients = new List<ReminderRecipient>();
                if (request.ReminderRecipients != null && request.ReminderRecipients.Count > 0)
                {
                    foreach (var recipient in request.ReminderRecipients)
                    {
                        if (string.IsNullOrWhiteSpace(recipient)|| (!recipient.Contains("@")))
                        {
                            continue;
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

               // create comment, tags or comments
                var comments = new List<ContractComment>();
                if (request.Comments != null && request.Comments.Count > 0)
                {
                    foreach (var comment in request.Comments)
                    {

                        comments.Add(new ContractComment
                        {
                            ContractId = entity.Id,
                            OrganisationId = entity.OrganisationId,
                            OrganisationName = entity.OrganisationName,
                            Comment = comment.Comment,
                            StatusDesc = Status.Active.ToString(),
                            Status = Status.Active,
                            CreatedBy = entity.CreatedBy,
                            CreatedDate = entity.CreatedDate,
                            ContractCommentType = comment.ContractCommentType
                        });
                    }
                    await _context.ContractComments.AddRangeAsync(comments);
                }

               // tags
                var tags = new List<Domain.Entities.ContractTag>();
                if (request.Tags != null && request.Tags.Count > 0)
                {
                    foreach (var tag in request.Tags)
                    {
                        tags.Add(new Domain.Entities.ContractTag
                        {
                            ContractId = entity.Id,
                            OrganisationId = entity.OrganisationId,
                            OrganisationName = entity.OrganisationName,
                            Name = tag.Name,
                            StatusDesc = Status.Active.ToString(),
                            Status = Status.Active,
                            CreatedBy = entity.CreatedBy,
                            CreatedDate = entity.CreatedDate
                        });
                    }
                    await _context.ContractTags.AddRangeAsync(tags);
                }
                //tasks
                var tasks = new List<Domain.Entities.ContractTask>();
                var inboxes = new List<Inbox>();
                var emailList = new List<EmailVm>();
                string webDomain = _configuration["LoginPage"];
                if (request.Tasks != null && request.Tasks.Count > 0)
                {
                    foreach (var task in request.Tasks)
                    {
                        tasks.Add(new Domain.Entities.ContractTask
                        {
                            ContractId = entity.Id,
                            DueDate = task.DueDate,
                            TaskCreatedById = request.UserId,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString(),
                            AssignedUserId = task.AssignedUserId,
                            AssignedUserEmail = task.AssignedUserEmail,
                            ContractTaskStatus = Domain.Enums.ContractTaskStatus.Assigned,
                            ContractTaskStatusDesc = Domain.Enums.ContractTaskStatus.Assigned.ToString(),
                            CreatedBy = request.UserId,
                            OrganisationId = entity.OrganisationId,
                            OrganisationName = entity.OrganisationName,
                            CreatedDate = DateTime.Now,
                            Name = task.Name,
                        });
                        emailList.Add(new EmailVm
                        {
                            Subject = "Contract Task Assignment",
                            RecipientEmail = task.AssignedUserEmail,
                            //RecipientName = request.AssignedUserName,
                            DisplayButton = "Task Assignment",
                            Body = $"You have been assigned a task '{task.Name}' on <b>{entity.Name} document</b>, by <b>{user.Entity.FirstName} {user.Entity.LastName}</b>. The due date is " + task.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt."),
                            ButtonText = "Login",
                            ButtonLink = webDomain

                        });
                        //send an Email to the sender
                        emailList.Add(new EmailVm
                        {
                            Subject = "Contract Task Assignment",
                            RecipientEmail = user.Entity.Email,
                            //RecipientName = request.AssignedUserName,
                            DisplayButton = "Task Assignment",
                            Body = $"Hi, you assigned a task '{task.Name}' to {task.AssignedUserEmail} on <b>{request.Name} {request.DocumentType}</b>. The due date is " + task.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt."),
                            ButtonText = "Login",
                            ButtonLink = webDomain

                        });

                        inboxes.Add(new Inbox
                        {
                            Name = "Contract Task Assignment",
                            Body = $"Hi, You have been assigned a task '{task.Name}' on {entity.Name} document, by {user.Entity.FirstName} {user.Entity.LastName}. The due date is " + task.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt."),
                            Delivered = true,
                            ReciepientEmail = task.AssignedUserEmail,
                            OrganisationId = request.OrganisationId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            Read = false,
                            EmailAction = EmailAction.Received,
                            StatusDesc = Status.Active.ToString(),
                            CreatedByEmail = request.Email
                        });
                        //send the sender an inbox
                        inboxes.Add(new Inbox
                        {
                            Name = "Contract Task Assignment",
                            Body = $"Hi, you assigned a task '{task.Name}' to {task.AssignedUserEmail} on {request.Name} {request.DocumentType}. The due date is " + task.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt."),
                            Delivered = true,
                            ReciepientEmail = user.Entity.Email,
                            OrganisationId = request.OrganisationId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            Read = false,
                            EmailAction = EmailAction.Sent,
                            StatusDesc = Status.Active.ToString(),
                            CreatedByEmail = request.Email,
                            Sender = user.Entity.FirstName + " " + user.Entity.LastName
                        });
                    }
                    await _context.ContractTasks.AddRangeAsync(tasks);
                    await _context.Inboxes.AddRangeAsync(inboxes);

                    await _context.SaveChangesAsync(cancellationToken);
                }
                #region
                var supportingDocuments = new List<SupportingDocument>();
                if (request.SupportingDocuments != null && request.SupportingDocuments.Count > 0)
                {
                
                    foreach (var x in request.SupportingDocuments)
                    {
                        // save each of the documents on blob server and create the contract document in the db
                        var val = await this.Convert(x, entity);
                        if (val != null)
                        {
                            supportingDocuments.Add(val);
                        }
                    }
                    _context.SupportingDocuments.AddRange(supportingDocuments);
                }
                #endregion


                if (entity.DocumentType == DocumentType.Contract)
                {
                    var newValue = new
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        ContractStatus = entity.ContractStatus.ToString(),
                    };

                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        UserId = request.UserId,
                        LastModifiedBy = request.UserId,
                        RoleId = request.InitiatorRoleId,
                        RoleName = request.RoleName,
                        FirstName=user.Entity.FirstName,
                        LastName=user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Contract.ToString(),
                        NewValue = newValue,
                        Action = AuditType.Create.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }

                if (entity.DocumentType == DocumentType.Permit)
                {
                    var newValue = new
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        ContractStatus = entity.ContractStatus.ToString(),
                        IsAnExecutedDocument = entity.IsAnExecutedDocument
                    };

                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        UserId = request.UserId,
                        LastModifiedBy = request.UserId,
                        RoleId = request.InitiatorRoleId,
                        RoleName = request.RoleName,
                        FirstName=user.Entity.FirstName,
                        LastName=user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Permit.ToString(),
                        NewValue = newValue,
                        Action = AuditType.Create.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                if (emailList!=null && emailList.Count > 0)
                {
                    var response = await _emailService.SendBulkEmail(emailList);
                }

                var result = _mapper.Map<ContractDto>(entity);
                result.SupportingDocuments = _mapper.Map<List<SupportingDocumentDto>>(supportingDocuments);
                return Result.Success($"Executed {entity.DocumentTypeDesc} request created successfully!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Executed Contract request creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
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
