using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Inboxs.Commands.CreateInbox;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTask.Command
{
    public class CreateResolvedTask : AuthToken,IRequest<Result>
    {
        public bool IsResolved { get; set; }
        public string UserId { get; set; }
        public string AssignedUserId { get; set; }
        public int OrganizationId { get; set; }
        public int Id { get; set; }

    }
    public class CreateResolvedTaskHandler : IRequestHandler<CreateResolvedTask, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CreateResolvedTaskHandler(IApplicationDbContext context, IConfiguration configuration, IEmailService emailService,IMediator mediator,IAuthService authService,IMapper mapper)
        {
            _context = context;
            _emailService = emailService;
            _mediator = mediator;
            _configuration = configuration;
            _mapper = mapper;
            _authService=authService;
        }
        public async Task<Result> Handle(CreateResolvedTask request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var assignedUser = await _authService.GetUserAsync(request.AccessToken, request.AssignedUserId);
                if (user == null)
                {
                    return Result.Failure("Assigned userId is not valid");
                }
                var contractTask = await _context.ContractTasks.FirstOrDefaultAsync(x => x.OrganisationId==request.OrganizationId && x.Id==request.Id && x.ContractTaskStatus != ContractTaskStatus.Resolved);
                if (contractTask == null)
                {
                    return Result.Failure("Contract Task for this user does not exist or task has been resolved.");
                }
                if (!request.IsResolved)
                {
                    contractTask.ContractTaskStatus = Domain.Enums.ContractTaskStatus.Unresolved;
                    contractTask.ContractTaskStatusDesc = ContractTaskStatus.Unresolved.ToString();
                    _context.ContractTasks.Update(contractTask);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Failure("Task is not resolved");
                }
                //check if task has expired.
                if (DateTime.Now > contractTask.DueDate)
                {

                    //call email service to inform the user that assigned task.
                    string domain = _configuration["LoginPage"];
                    var message = $"The due date for the task '{contractTask.Name}', assigned to {assignedUser.Entity.FirstName} {assignedUser.Entity.LastName} has elapsed.";
                    var messageVm = $"Your task has been resolved by {user.Entity.FirstName} {user.Entity.LastName}.";
                    var userEmail = new EmailVm
                    {
                        Subject = "Contract Task Overdue",
                        Text = "Contract Task!",
                        RecipientEmail = contractTask.CreatedBy,
                        DisplayButton = "display:none;",
                        Body1 = $"The due date for the task '{contractTask.Name}', assigned to <b>{assignedUser.Entity.FirstName} {assignedUser.Entity.LastName}</b> has elapsed.",
                        ButtonText = "Login",
                        ButtonLink = domain
                    };

                    //resolve the task
                    contractTask.AssignedUserId = request.AssignedUserId;
                    contractTask.ContractTaskStatus = Domain.Enums.ContractTaskStatus.Resolved;
                    contractTask.ContractTaskStatusDesc = ContractTaskStatus.Resolved.ToString();
                    _context.ContractTasks.Update(contractTask);
                    await _context.SaveChangesAsync(cancellationToken);

                    //send mail to the user that task is resolved.
                    string webDomainEmail = _configuration["LoginPage"];

                    var emailVm = new EmailVm
                    {
                        Subject = "Contract Task Resolve",
                        Text = "Contract Task!",
                        RecipientEmail = contractTask.AssignedUserEmail,
                        RecipientName = contractTask.AssignedUserEmail,
                        DisplayButton = "display:none;",
                        Body1 = $"Your task has been resolved by {user.Entity.FirstName} {user.Entity.LastName}.",
                        ButtonText = "Login",
                        ButtonLink = webDomainEmail
                    };

                    await _emailService.SendEmail(userEmail);
                    await _emailService.SendEmail(emailVm);

                    //send inbox for userEmail
                    var handlerScope = new CreateInboxCommandHandler(_context, _mapper, _authService);
                    var commandScope = new CreateInboxCommand
                    {
                        AccessToken = request.AccessToken,
                        UserId=request.UserId,
                        OrganisationId = request.OrganizationId,
                        Body =message,
                        Name = userEmail.Subject,
                        Delivered = false,
                        RecipeintEmail = userEmail.RecipientEmail,
                        Email=userEmail.RecipientEmail
                    };
                    var res = await handlerScope.Handle(commandScope, cancellationToken);

                    //send inbox for emailVm
                    var newHandlerScope = new CreateInboxCommandHandler(_context, _mapper, _authService);
                    var newCommandScope = new CreateInboxCommand
                    {
                        AccessToken = request.AccessToken,
                        UserId = request.UserId,
                        OrganisationId = request.OrganizationId,
                        Body = messageVm,
                        Name = emailVm.Body1,
                        Delivered = false,
                        RecipeintEmail = emailVm.RecipientEmail,
                        Email=emailVm.RecipientEmail
                    };
                    var newResponse = await newHandlerScope.Handle(newCommandScope, cancellationToken);

                    contractTask.ContractTaskStatus = ContractTaskStatus.Expired;
                    contractTask.ContractTaskStatusDesc = ContractTaskStatus.Expired.ToString();
                    contractTask.ContractTaskStatus = Domain.Enums.ContractTaskStatus.Resolved;
                    contractTask.ContractTaskStatusDesc = ContractTaskStatus.Resolved.ToString();
                    _context.ContractTasks.Update(contractTask);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("The task has expired, task has been resolved!, email sent to the task assigner.");

                }

                //task has not expired
                contractTask.AssignedUserId = request.AssignedUserId;
                contractTask.ContractTaskStatus = Domain.Enums.ContractTaskStatus.Resolved;
                contractTask.ContractTaskStatusDesc = ContractTaskStatus.Resolved.ToString();
                _context.ContractTasks.Update(contractTask);
                await _context.SaveChangesAsync(cancellationToken);

                //send mail to the user that is task is resolved.
                string webDomain = _configuration["LoginPage"];

                var email = new EmailVm
                {
                    Subject = "Contract Task Overdue",
                    Text = "Contract Task!",
                    RecipientEmail = contractTask.AssignedUserEmail,
                    RecipientName = contractTask.AssignedUserEmail,
                    DisplayButton = "display:none;",
                    Body1 = $"Your task '{contractTask.Name}' has been resolved by {user.Entity.FirstName} {user.Entity.LastName}.",
                    ButtonText = "Login",
                    ButtonLink = webDomain
                };

                await _emailService.SendEmail(email);
                var handler = new CreateInboxCommandHandler(_context, _mapper, _authService);
                var command = new CreateInboxCommand
                {
                    AccessToken = request.AccessToken,
                    UserId=request.UserId,
                    OrganisationId = request.OrganizationId,
                    Body = email.Body1,
                    Name = email.Subject,
                    Delivered = false,
                    RecipeintEmail = email.RecipientEmail,
                    Email=email.RecipientEmail
                };
                var result = await handler.Handle(command, cancellationToken);

                return Result.Success($"Contract Task has been resolved, {contractTask.AssignedUserEmail} has been notified.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract task resolution failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
