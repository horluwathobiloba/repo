using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ReminderRecipients.Commands
{
    public class CreateReminderRecipientsCommand : AuthToken, IRequest<Result>
    {
        public string Email { get; set; }
        public int ContractId { get; set; }
        public int OrganisationId { get; set; }
    }

    public class CreateReminderRecipientsCommandHandler : IRequestHandler<CreateReminderRecipientsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateReminderRecipientsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateReminderRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var exists = await _context.ReminderRecipients.AnyAsync(x => x.Email == request.Email && x.ContractId == request.ContractId);
                if (exists)
                {
                    return Result.Failure($"Reminder recipient already exists for({request.Email})!");
                }
                var reminderRecipient = new ReminderRecipient
                {
                    OrganisationId = request.OrganisationId,
                    OrganisationName  = _authService.Organisation.Name,
                    ContractId = request.ContractId,
                    Email = request.Email,
                    CreatedBy = request.Email,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.Email,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.ReminderRecipients.AddAsync(reminderRecipient);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Contract recipient was created successfully", reminderRecipient);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Reminder recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }
}
