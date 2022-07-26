using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTask.Queries;
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
    public class UpdateContractTaskCommand: AuthToken, IRequest<Result>
    {
        public int ContractTaskId { get; set; }
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignedUserId { get; set; }
        public int ContractId { get; set; }
        public string AssignedUserEmail { get; set; }
        public string UserEmail { get; set; }
        public string TaskName { get; set; }
    }
    public class UpdateContractTaskCommandHandler : IRequestHandler<UpdateContractTaskCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        public UpdateContractTaskCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator,INotificationService notificationService,IAuthService authService,IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Result> Handle(UpdateContractTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.Id == request.ContractId);
                if (contract == null)
                {
                    return Result.Failure("Contract does not exist");
                }
                var contractTask = await _context.ContractTasks.FirstOrDefaultAsync(a=>a.Id==request.ContractTaskId && a.OrganisationId==request.OrganizationId && a.Status==Domain.Enums.Status.Active);
                if (contractTask==null)
                {
                    return Result.Failure("Task does not exist!");
                }

                if (DateTime.Now>contractTask.DueDate)
                {
                    contractTask.ContractTaskStatusDesc = ContractTaskStatus.Expired.ToString();
                    contractTask.ContractTaskStatus = ContractTaskStatus.Expired;
                    _context.ContractTasks.Update(contractTask);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                contractTask.LastModifiedBy = request.UserId;
                contractTask.LastModifiedDate = DateTime.Now;
                contractTask.DueDate = request.DueDate;
                contractTask.AssignedUserId = request.AssignedUserId;
                contractTask.AssignedUserEmail = request.AssignedUserEmail;
                contractTask.CreatedByEmail = request.UserEmail;
                contractTask.ContractId = request.ContractId;
                contractTask.Name = request.TaskName;
                _context.ContractTasks.Update(contractTask);
                await _context.SaveChangesAsync(cancellationToken);
                             
                string webDomain = _configuration["LoginPage"];
                var message = $"Your task '{request.TaskName}' on {contract.Name} contract has been edited by {request.UserEmail}. The due date is  " + request.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt.");
                var email = new EmailVm
                {
                    Subject = "Contract Task Updated",
                    RecipientEmail = request.AssignedUserEmail,
                    //RecipientName = request.AssignedUserName,
                    DisplayButton = "Task Assignment",
                    Body = $"Your task '{request.TaskName}' on <b>{contract.Name} document</b> has been edited by <b>{user.Entity.FirstName} {user.Entity.LastName}</b>. The due date is  " + request.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt."),
                    ButtonText = "Login",
                    ButtonLink = webDomain
                };

                await _emailService.SendEmail(email);
                var handler = new CreateInboxCommandHandler(_context, _mapper, _authService);
                var command = new CreateInboxCommand
                {
                    AccessToken = request.AccessToken,
                    // OrganisationName = request.OrganisationName,
                    OrganisationId = request.OrganizationId,
                    Body =message,
                    Name = email.Subject,
                    Delivered = false,
                    RecipeintEmail = email.RecipientEmail,
                    UserId = request.UserId,
                    Email=request.UserEmail
                };
                var resp = await handler.Handle(command, cancellationToken);
                var result = _mapper.Map<ContractTaskDto>(contractTask);

                return Result.Success($"Task updated successfully, email sent to {request.AssignedUserEmail}!!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Update Contract Tasks  failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
