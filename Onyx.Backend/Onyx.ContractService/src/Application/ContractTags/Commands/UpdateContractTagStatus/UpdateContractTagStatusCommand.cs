using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTag.Queries.GetContractTags;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTagStatus
{
    public class UpdateContractTagStatusCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractTagStatusCommandHandler : IRequestHandler<UpdateContractTagStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateContractTagStatusCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateContractTagStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractTags.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid ContractTag type!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Task tag is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Task tag was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "ContractTag activation was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractTagDto>(entity);
                return Result.Success(message, result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Task Tag status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
