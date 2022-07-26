using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.CreateContractTypeInitiators
{
    public class CreateContractTypeInitiatorsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractTypeId { get; set; }
        public List<CreateContractTypeInitiatorRequest> ContractTypeInitiators { get; set; }
        public string UserId { get; set; }
    }

    public class CreateContractTypeInitiatorsCommandHandler : IRequestHandler<CreateContractTypeInitiatorsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateContractTypeInitiatorsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateContractTypeInitiatorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var roles = await _authService.GetRolesAsync(request.AccessToken, request.OrganisationId);

                var list = new List<ContractTypeInitiator>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.ContractTypeInitiators)
                {
                   if(!roles.Entity.Any(r => r.Id == item.RoleId))
                    {
                        throw new Exception("Invalid role specified in the list");
                    }

                    var exists = await _context.ContractTypeInitiators
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.ContractTypeId == request.ContractTypeId && x.RoleId == item.RoleId);

                    if (exists)
                    {
                        throw new Exception($"Contract type Initiator already exists for {item.RoleName} !");
                    }

                    var entity = new ContractTypeInitiator
                    {
                        ContractTypeId = request.ContractTypeId,
                        RoleId = item.RoleId,

                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.ContractTypeInitiators.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ContractTypeInitiatorDto>>(list);
                return Result.Success("Contract type Initiators created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract type Initiators creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
