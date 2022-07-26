using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTask.Queries;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Application.Inboxs.Commands.CreateInbox;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTask.Command
{
    public class CreateContractTaskCommand : AuthToken, IRequest<Result>
    {
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignedUserId { get; set; }
        public int ContractId { get; set; }
        public string AssignedUserEmail { get; set; }

        //  public string AssignedUserName { get; set; } // remove it

        public string UserEmail { get; set; }

        public string TaskName { get; set; }

    }

    public class CreateContractTaskCommandHandler : IRequestHandler<CreateContractTaskCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IAPIClientService _aPIClient;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        public CreateContractTaskCommandHandler(IApplicationDbContext context, IMapper mapper,
            IEmailService emailService, IAPIClientService aPIClient,
            IConfiguration configuration, IAuthService authService, IMediator mediator,
            INotificationService notificationService)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
            _aPIClient = aPIClient;
            _mapper = mapper;
            _emailService = emailService;
            _mediator = mediator;
            _notificationService = notificationService;
        }
        public async Task<Result> Handle(CreateContractTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);

                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                //get assigned user email
                //var assignedUser = await _authService.GetUserAsync(request.AccessToken, request.AssignedUserId);
                //if (assignedUser==null)
                //{
                //    return Result.Failure("Assigned UserId is not valid");
                //}
                var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.Id == request.ContractId);
                if (contract == null)
                {
                    return Result.Failure("Contract does not exist");
                }

                //var assignedUser = await _authService.GetUserAsync(request.AccessToken, request.AssignedUserId);

                var entity = new Domain.Entities.ContractTask
                {
                    ContractId = request.ContractId,
                    DueDate = request.DueDate,
                    TaskCreatedById = request.UserId,
                    AssignedUserId = request.AssignedUserId,
                    AssignedUserEmail = request.AssignedUserEmail,
                    ContractTaskStatus = Domain.Enums.ContractTaskStatus.Assigned,
                    ContractTaskStatusDesc = ContractTaskStatus.Assigned.ToString(),
                    CreatedBy = request.UserEmail,
                    CreatedByEmail = request.UserEmail,
                    Name = request.TaskName,
                    OrganisationId = user.Entity.Organization.Id,
                    OrganisationName = user.Entity.Organization.Name,
                    DeviceId = user.Entity.DeviceId,
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    StatusDesc = Status.Active.ToString()
                };
                await _context.ContractTasks.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                string webDomain = _configuration["LoginPage"];
                var message = $"Hi, you have been assigned a task '{request.TaskName}' by {user.Entity.FirstName} {user.Entity.LastName} on {contract.Name} contract. The due date is " + request.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt.");
                var messageSender = $"Hi, you assigned a task '{request.TaskName}' to {request.AssignedUserEmail} on {contract.Name} contract. The due date is " + request.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt.");
                var body = $"Hi, you have been assigned a task '{request.TaskName}' by <b>{user.Entity.FirstName} {user.Entity.LastName}</b> on <b>{contract.Name} contract</b>. The due date is " + request.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt.");
                var bodySender = $"Hi, you assigned a task '{request.TaskName}' to {request.AssignedUserEmail} on <b>{contract.Name} contract</b>. The due date is " + request.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt.");
                //var email = new EmailVm
                //{
                //    Subject = "Contract Task Assignment",
                //    RecipientEmail = request.AssignedUserEmail,

                //    DisplayButton = "Task Assignment",
                //    Body =body,
                //    ButtonText = "Login",
                //    ButtonLink = webDomain
                //}; 
                //var emailSender = new EmailVm
                //{
                //    Subject = "Contract Task Assignment",
                //    RecipientEmail = request.AssignedUserEmail,

                //    DisplayButton = "Task Assignment",
                //    Body =body,
                //    ButtonText = "Login",
                //    ButtonLink = webDomain
                //};
                var emailList = new List<EmailVm> {
                new EmailVm
                {
                    Subject = "Contract Task Assignment",
                    RecipientEmail = request.AssignedUserEmail,

                    DisplayButton = "Task Assignment",
                    Body =body,
                    ButtonText = "Login",
                    ButtonLink = webDomain,

                },
                new EmailVm
                {
                    Subject = "Contract Task Assignment",
                    RecipientEmail = user.Entity.Email,

                    DisplayButton = "Task Assignment",
                    Body =bodySender,
                    ButtonText = "Login",
                    ButtonLink = webDomain
                }
                };


                await _emailService.SendBulkEmail(emailList);
                var inboxRequests = new List<CreateInboxRequest> {
                new CreateInboxRequest
                {
                    Name= "Contract Task Assignment",
                    ReciepientEmail=request.AssignedUserEmail,
                    Body=message,
                    Delivered=true,
                    EmailAction = EmailAction.Received,
                    OrganizationId = request.OrganizationId,
                    UserId = request.UserId,
                    Email=user.Entity.Email
                    
                },

                new CreateInboxRequest
                {
                    Name= "Contract Task Assignment",
                    ReciepientEmail=user.Entity.Email,
                    Body=messageSender,
                    Delivered=true,
                    EmailAction = EmailAction.Sent,
                    OrganizationId = request.OrganizationId,
                    UserId = request.UserId,
                    Email=user.Entity.Email
                }
              };

                var handler = new CreateInboxesCommandHandler(_context, _mapper, _authService);

                var command = new CreateInboxesCommand
                {
                   AccessToken=request.AccessToken,
                   OrganisationId=request.OrganizationId,
                   Inboxes=inboxRequests,
                   UserId=request.UserId,
                   OrganisationName="",
                };
                var resp = await handler.Handle(command, cancellationToken);
                var result = _mapper.Map<ContractTaskDto>(entity);

                return Result.Success($"Task created and assigned successfully to {request.AssignedUserEmail}!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract task creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
