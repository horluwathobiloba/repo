using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypes.Commands.FinalizeContractTypeConfiguration
{
    public class FinalizeContractTypeConfigurationCommand : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int Id { get; set; } 
        public string UserId { get; set; }
    }

    public class FinalizeContractTypeConfigurationCommandHandler : IRequestHandler<FinalizeContractTypeConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public FinalizeContractTypeConfigurationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(FinalizeContractTypeConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractTypes.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid Contract type!");
                }


                entity.ContractTypeStatus = ContractTypeStatus.Completed.ToString(); 
                entity.Status = Status.Active;
                entity.StatusDesc = Status.Active.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now; 

                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Contract type was successfully activated!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract type status update failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
