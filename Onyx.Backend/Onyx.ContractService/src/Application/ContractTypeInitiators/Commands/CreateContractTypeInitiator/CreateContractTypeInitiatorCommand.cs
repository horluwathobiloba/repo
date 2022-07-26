using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.CreateContractTypeInitiator
{
    public class CreateContractTypeInitiatorCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractTypeId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }
        public string UserId { get; set; }
    }


    public class CreateContractTypeInitiatorCommandHandler : IRequestHandler<CreateContractTypeInitiatorCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateContractTypeInitiatorCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateContractTypeInitiatorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId, request.RoleId);

                var exists = await _context.ContractTypeInitiators
                    .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.ContractTypeId == request.ContractTypeId && x.RoleId == request.RoleId);

                if (exists)
                {
                    return Result.Failure($"Contract type Initiator already exists for this Role!");
                }

                var entity = new ContractTypeInitiator
                {
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    ContractTypeId = request.ContractTypeId,
                    RoleId = request.RoleId,
                    RoleName = _authService.Role?.Name,
                    JobFunctionId = request.JobFunctionId,
                    JobFunctionName = request.JobFunctionName,

                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.ContractTypeInitiators.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractTypeInitiatorDto>(entity);
                return Result.Success("Contract type Initiator was created successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract type Initiator creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}
