using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Application.ContractComments.Commands.CreateContractComment;
using Onyx.ContractService.Application.Contracts.Commands.UpdateContractStatus;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Commands.TerminateContract
{
    public class TerminateContractCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public string TerminationReason { get; set; }
        public string UserId { get; set; }
    }

    public class TerminateContractCommandHandler : IRequestHandler<TerminateContractCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        public TerminateContractCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, 
        IEmailService emailService,IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _emailService = emailService;
            _mediator = mediator;
        }

        public async Task<Result> Handle(TerminateContractCommand request, CancellationToken cancellationToken)
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
                var entity = await _context.Contracts.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id
                && x.Status == Status.Active
                && x.ContractStatus == ContractStatus.Active);

                if (entity == null)
                {
                    return Result.Failure("Invalid contract!");
                }

                var oldValue = new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    ContractStatus = entity.ContractStatus.ToString(),
                };

                var updateContractStatusCommand = new UpdateContractStatusCommand
                {
                    OrganisationId = request.OrganisationId,
                    Id = request.Id,
                    AccessToken = request.AccessToken,
                    ContractStatus = ContractStatus.Terminated,
                    TerminationReason = request.TerminationReason,
                    UserId = request.UserId
                };
                var handler = new CreateContractCommentCommandHandler(_context,_mapper,_authService);
                var contractCommand = new CreateContractCommentCommand
                {
                    AccessToken=request.AccessToken,
                    UserId=request.UserId,
                    Comment=request.TerminationReason,
                    //ContractId=request.
                    OrganisationId=request.OrganisationId,
                    
                };
                var comment = handler.Handle(contractCommand, cancellationToken);

                var result = await new UpdateContractStatusCommandHandler(_context, _mapper, _authService, _emailService,_mediator).UpdateContractStatus(updateContractStatusCommand, cancellationToken);
                
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract termnination failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }
}
