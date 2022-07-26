using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients; 
using Onyx.ContractService.Domain.Enums;
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipientStatus
{
    public class UpdateContractRecipientStatusCommand : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractRecipientStatusCommandHandler : IRequestHandler<UpdateContractRecipientStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateContractRecipientStatusCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateContractRecipientStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractRecipients.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid Contract recipient!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Status.Inactive;
                        message = "Contract recipient is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Status.Active;
                        message = "Contract recipient was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Contract recipient was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractRecipientDto>(entity);
                return Result.Success(message, entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipient status update failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
