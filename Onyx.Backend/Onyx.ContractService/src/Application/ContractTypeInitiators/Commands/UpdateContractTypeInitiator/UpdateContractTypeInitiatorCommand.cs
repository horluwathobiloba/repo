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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiator
{
    public class UpdateContractTypeInitiatorCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public int ContractTypeId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractTypeInitiatorCommandHandler : IRequestHandler<UpdateContractTypeInitiatorCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateContractTypeInitiatorCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateContractTypeInitiatorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId, request.RoleId);
                var modfiedEntityExists = await _context.ContractTypeInitiators.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.RoleId == request.RoleId);

                if (modfiedEntityExists)
                {
                    return Result.Failure($"Another Contract type Initiator with this role '{request.RoleName}' already exists. Please change the role.");
                }

                var entity = await _context.ContractTypeInitiators
                    .Where(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id)
                    .Include(a => a.ContractType)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid Contract type Initiator specified.");
                }

                entity.RoleId = request.RoleId;
                entity.RoleName = _authService.Role.Name;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.ContractTypeInitiators.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractTypeInitiatorDto>(entity);
                return Result.Success("Contract type Initiator was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract type Initiator update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }

}
