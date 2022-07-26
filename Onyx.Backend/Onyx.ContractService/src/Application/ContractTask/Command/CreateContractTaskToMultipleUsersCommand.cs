using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTask.Command
{
    public class CreateContractTaskToMultipleUsersCommand : AuthToken, IRequest<Result>
    {
        public string UserId { get; set; }

        public int OrganisationId { get; set; }

        public string UserEmail { get; set; }

        public List<Domain.ViewModels.ContractTaskVm> ContractTaskVm { get; set; }

        public int ContractId { get; set; }
    }
    public class CreateContractTaskToMultipleUsersCommandHandler : IRequestHandler<CreateContractTaskToMultipleUsersCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CreateContractTaskToMultipleUsersCommandHandler(IEmailService emailService,IApplicationDbContext context,
            IConfiguration configuration, IAuthService authService,IMediator mediator,IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
            _emailService = emailService;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateContractTaskToMultipleUsersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                
                if (request.ContractTaskVm == null || request.ContractTaskVm.Count == 0)
                {
                    return Result.Failure("Multiple users must be assigned");
                }
                var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.Id == request.ContractId);
                if (contract == null)
                {
                    return Result.Failure("Contract does not exist");
                }

                var resultList = new List<Domain.Entities.ContractTask>();
                //var emailList = new List<EmailVm>();
                var inboxes = new List<CreateInboxRequest>();
                foreach (var item in request.ContractTaskVm)
                {
                    //var assignedUser = await _authService.GetUserAsync(request.AccessToken, item.AssignedToUserId);
                    //if (user == null)
                    //{
                    //    return Result.Failure("Assigned userId is not valid");
                    //}
                    var entity = new Domain.Entities.ContractTask
                    {
                        ContractId = request.ContractId,
                        DueDate = item.DueDate,
                        TaskCreatedById = request.UserId,
                        AssignedUserId = item.AssignedToUserId,
                        AssignedUserEmail = item.AssignedToEmail,
                        ContractTaskStatus = Domain.Enums.ContractTaskStatus.Assigned,
                        ContractTaskStatusDesc = ContractTaskStatus.Assigned.ToString(),
                        CreatedBy = request.UserEmail,
                        Name = item.TaskName,
                        OrganisationId = user.Entity.Organization.Id,
                        OrganisationName = user.Entity.Organization.Name,
                        DeviceId = user.Entity.DeviceId,
                        Status = Status.Active,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        StatusDesc = Status.Active.ToString()
                    };
                    resultList.Add(entity);
                    
                    //send mail to the user assigned a task.
                    string webDomain = _configuration["LoginPage"];
                    var message = $"You have been assigned a task '{item.TaskName}' on {contract.Name} contract, by {user.Entity.FirstName} {user.Entity.LastName}. The due date is " + item.DueDate;
                    var messageSender = $"You assigned a task '{item.TaskName}' on {contract.Name} contract, to {item.AssignedToEmail} . The due date is " + item.DueDate;
                    var body = $"You have been assigned a task '{item.TaskName}' on <b>{contract.Name} contract</b>, by <b>{user.Entity.FirstName} {user.Entity.LastName}</b>. The due date is " + item.DueDate;
                    var bodySender = $"You  assigned a task '{item.TaskName}' on <b>{contract.Name} contract</b>, to <b>{item.AssignedToEmail}</b>. The due date is " + item.DueDate;
                    var emailList = new List<EmailVm> {
                    new EmailVm
                    {
                        Subject = "Contract Task Assignment",
                        Text = "Contract Task!",
                        RecipientEmail = item.AssignedToEmail,
                        DisplayButton = "Task Assignment",
                        Body1 = body,
                        ButtonText = "Login",
                        ButtonLink = webDomain
                    },
                    new EmailVm
                    {
                        Subject = "Contract Task Assignment",
                        Text = "Contract Task!",
                        RecipientEmail = user.Entity.Email,
                        DisplayButton = "Task Assignment",
                        Body1 =bodySender,
                        ButtonText = "Login",
                        ButtonLink = webDomain
                    }

                    };
                    //var email = new EmailVm
                    //{
                    //    Subject = "Contract Task Assignment",
                    //    Text = "Contract Task!",
                    //    RecipientEmail = item.AssignedToEmail,
                    //    DisplayButton = "Task Assignment",
                    //    Body1 = body,
                    //    ButtonText = "Login",
                    //    ButtonLink = webDomain
                    //};
                    //var emailSender = new EmailVm
                    //{
                    //    Subject = "Contract Task Assignment",
                    //    Text = "Contract Task!",
                    //    RecipientEmail = user.Entity.Email,
                    //    DisplayButton = "Task Assignment",
                    //    Body1 = $"You  assigned a task '{item.TaskName}' on <b>{contract.Name} contract</b>, to <b>{item.AssignedToEmail}</b>. The due date is " + item.DueDate,
                    //    ButtonText = "Login",
                    //    ButtonLink = webDomain
                    //};
                    await _emailService.SendBulkEmail(emailList);

                    inboxes.Add(new CreateInboxRequest
                    {
                        Name = "Contract Task Assignment",
                        ReciepientEmail = item.AssignedToEmail,
                        Body = message,
                        EmailAction = EmailAction.Received,
                        Delivered = true,
                        OrganizationId = request.OrganisationId,
                        UserId = request.UserId,
                        Email=user.Entity.Email
                    });

                    inboxes.Add(new CreateInboxRequest
                    {
                        Name = "Contract Task Assignment",
                        ReciepientEmail = request.UserEmail,
                        Body = messageSender,
                        EmailAction = EmailAction.Sent,
                        Delivered = true,
                        OrganizationId = request.OrganisationId,
                        UserId = request.UserId,
                        Email=user.Entity.Email
                    });
                }
                var handler = new CreateInboxesCommandHandler(_context, _mapper, _authService);
                var command = new CreateInboxesCommand
                {
                    AccessToken = request.AccessToken,
                    //OrganisationName = request.OrganisationName,
                    OrganisationId = request.OrganisationId,
                    Inboxes = inboxes,
                    UserId = request.UserId

                };
                var resp = await handler.Handle(command, cancellationToken);
                await _context.ContractTasks.AddRangeAsync(resultList);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Task assigned successfully", resultList);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Create multiple contract task failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
            
        }
    }
}
