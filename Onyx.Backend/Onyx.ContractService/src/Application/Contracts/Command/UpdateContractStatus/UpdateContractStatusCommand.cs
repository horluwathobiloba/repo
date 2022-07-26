using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Application.ContractComments.Commands.CreateContractComment;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Commands.UpdateContractStatus
{
    public class UpdateContractStatusCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public string TerminationReason { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractStatusCommandHandler : IRequestHandler<UpdateContractStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private string accessToken;
        private IMediator _mediator;

        public UpdateContractStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IEmailService emailService,IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _emailService = emailService;
            _mediator = mediator;
        }

        public async Task<Result> Handle(UpdateContractStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var result = await UpdateContractStatus(request, cancellationToken);



                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract status update failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
        internal async Task<Result> CreateComment(UpdateContractStatusCommand request)
        {
            var handler = new CreateContractCommentCommandHandler(_context, _mapper, _authService);
            var contractCommand = new CreateContractCommentCommand
            {
                AccessToken = request.AccessToken,
                UserId = request.UserId,
                Comment = request.TerminationReason,
                ContractId = request.Id,
                OrganisationId = request.OrganisationId
            };
            var result = await handler.Handle(contractCommand, new CancellationToken());
            return result;
        }

        internal async Task<Result> UpdateContractStatus(UpdateContractStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                this.accessToken = request.AccessToken;

                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user==null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (contract == null)
                {
                    return Result.Failure("Invalid contract!");
                }

                //to get the old values of the contract before updating.
                //var oldValuesEntity = new
                //{
                //    contractEntity = contract
                //};
                var oldValuesEntity = contract;
                await _context.BeginTransactionAsync();
                string message = "";
                switch (request.ContractStatus)
                {
                    case ContractStatus.Active:
                        message = "Contract is now active!";
                        break;
                    case ContractStatus.Approved:
                        contract.Status = Status.Inactive;
                        message = "Contract has been approved!";
                        break;
                    case ContractStatus.Cancelled:
                        contract.Status = Status.Active;
                        message = "Contract has been cancelled!";
                        break;
                    case ContractStatus.Expired:
                        message = "Contract is now expired!";
                        break;
                    case ContractStatus.PendingApproval:
                        message = "Contract is pending approval!";
                        break;
                    case ContractStatus.PendingSignatories:
                        message = "Contract is pending signatories!";
                        break;
                    case ContractStatus.Processing:
                        message = "Contract is now processing!";
                        break;
                    case ContractStatus.Rejected:
                        message = "Contract has been rejected!";

                        break;
                    case ContractStatus.Terminated:
                        message = "Contract has been terminated!";
                        contract.TerminationReason = request.TerminationReason;
                        //add comment
                        var entity = new Domain.Entities.ContractComment
                        {
                            ContractId = contract.Id,
                            Comment = contract.TerminationReason,
                            OrganisationId = request.OrganisationId,
                            OrganisationName = contract.OrganisationName,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString(),
                            CommentById = request.UserId,
                            ContractCommentType = ContractCommentType.Internal
                        };
                        await _context.ContractComments.AddAsync(entity);
                        await SendTerminationEmail(contract, _authService.User?.Email,request,cancellationToken);
                        break;
                    default:
                        break;
                }
                if (contract.ContractExpirationDate >= DateTime.Now)
                {
                    contract.ContractStatus = Domain.Enums.ContractStatus.Expired;
                }
                contract.ContractStatus = request.ContractStatus;
                contract.ContractStatusDesc = request.ContractStatus.ToString();
                contract.StatusDesc = request.ContractStatus.ToString();
                contract.LastModifiedBy = request.UserId;
                contract.LastModifiedDate = DateTime.Now;
                _context.Contracts.UpdateRange(contract);
                //if (contract.ContractStatus == ContractStatus.Rejected || contract.ContractStatus == ContractStatus.Terminated)
                //{
                //    await CreateComment(request);
                //}
                await _context.SaveChangesAsync(cancellationToken);

                //create audit log
                if (contract.DocumentType == DocumentType.Contract)
                {
                    var newValuesEntity = contract;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = contract.OrganisationName,
                        LastModifiedBy = request.UserId,
                        RoleId = contract.RoleId,
                        UserId = request.UserId,
                        RoleName = contract.RoleName,
                        FirstName = user.Entity.FirstName,
                        LastName = user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Contract.ToString(),
                        OldValue = oldValuesEntity,
                        NewValue = newValuesEntity,
                        Action = request.ContractStatus.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }

                if (contract.DocumentType == DocumentType.Permit)
                {
                    var newValuesEntity = contract;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = contract.OrganisationName,
                        RoleId = contract.RoleId,
                        RoleName = contract.RoleName,
                        UserId = request.UserId,
                        FirstName = user.Entity.FirstName,
                        LastName = user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Permit.ToString(),
                        OldValue = oldValuesEntity,
                        NewValue = newValuesEntity,
                        Action = request.ContractStatus.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }
                await _context.CommitTransactionAsync();
                var result = _mapper.Map<ContractDto>(contract);
                return Result.Success(message, contract);
            }
            catch (Exception ex)
            {

                 _context.RollbackTransaction();
                return Result.Failure($"Contract status update failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        internal async Task SendTerminationEmail(Domain.Entities.Contract contract, string rejectActor,UpdateContractStatusCommand request,CancellationToken cancellationToken)
        {
            try
            {
                var subject = "Contract Termination Completed";
                var body = $"The document \"{contract.Name}\" has been terminated by ({rejectActor}). You will no longer be able to access and perform any action on the document.";
                var buttonText = "View Document";

                var recipients = contract.ContractRecipients.Where(a => a.Status == Status.Active).ToList();
                var list = new List<EmailVm>();
                var inboxes = new List<CreateInboxRequest>();
                foreach (var item in recipients) //Send email to all the contract recipients
                {
                    list.Add(new EmailVm
                    {
                        Subject = subject,
                        Body = body,
                        ButtonText = buttonText,
                        RecipientEmail = item.Email,
                        ButtonLink = contract.ExecutedContract
                    });
                    inboxes.Add(new CreateInboxRequest
                    {
                        Name = subject,
                        ReciepientEmail = item.Email,
                        Body = body,
                        EmailAction = EmailAction.Received,
                        Delivered = true,
                        OrganizationId = request.OrganisationId,
                        UserId = request.UserId
                    });
                }
                //add initiator to the email list
                list.Add(await this.ComposeEmailToInitiator(contract, subject, body, buttonText));
                var bb = await _emailService.SendBulkEmail(list);
                var handler = new CreateInboxesCommandHandler(_context,_mapper,_authService);
                var command = new CreateInboxesCommand
                {
                    AccessToken = request.AccessToken,
                    //OrganisationName = request.OrganisationName,
                    OrganisationId = request.OrganisationId,
                    Inboxes = inboxes,
                    UserId = request.UserId
                };
                var resp = await handler.Handle(command, cancellationToken);
            }
            catch (Exception ex)
            {
                //email sending failed
            }
        }

        internal async Task<EmailVm> ComposeEmailToInitiator(Domain.Entities.Contract contract, string subject, string body, string buttonText = "")
        {
            try
            {
                var initiator = await _authService.GetUserAsync(this.accessToken, contract.UserId);
                var email = new EmailVm
                {
                    Subject = subject,
                    Body = body,
                    ButtonText = buttonText,
                    RecipientName = initiator.Entity?.Name,
                    RecipientEmail = initiator.Entity?.Email
                };

                return email;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
