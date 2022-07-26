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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiators
{
    public class UpdateContractTypeInitiatorsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractTypeId { get; set; }
        public string UserId { get; set; }
        public List<UpdateContractTypeInitiatorRequest> ContractTypeInitiators { get; set; }

    }

    public class UpdateContractTypeInitiatorsCommandHandler : IRequestHandler<UpdateContractTypeInitiatorsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateContractTypeInitiatorsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateContractTypeInitiatorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<ContractTypeInitiator>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.ContractTypeInitiators)
                {
                    var modfiedEntityExists = await _context.ContractTypeInitiators.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id
                  && x.RoleId == item.RoleId);

                    if (modfiedEntityExists)
                    {
                        return Result.Failure($"Another Contract type Initiator with this role '{item.RoleName}' already exists. Please change the role.");
                    }

                    var entity = await _context.ContractTypeInitiators
                        .Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                        .Include(a => a.ContractType)
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        //return Result.Failure($"Invalid Contract type Initiator specified.");
                        entity = new ContractTypeInitiator
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
                    }
                    else
                    {
                        entity.RoleId = item.RoleId;
                        entity.RoleName = item.RoleName;
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }

                    list.Add(entity);
                }

                _context.ContractTypeInitiators.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ContractTypeInitiatorDto>>(list);
                return Result.Success("Contract type Initiators update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"ContractTypeInitiator update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
