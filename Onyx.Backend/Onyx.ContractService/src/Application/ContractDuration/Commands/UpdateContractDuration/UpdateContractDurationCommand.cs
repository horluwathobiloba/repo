using AutoMapper;
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

namespace Onyx.ContractService.Application.ContractDuration.Commands.UpdateContractDuration
{
    public class UpdateContractDurationCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
        public DurationFrequency DurationFrequency { get; set; }
    }
    public class UpdateContractDurationCommandHandler : IRequestHandler<UpdateContractDurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateContractDurationCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateContractDurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.ContractDurations.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid ContractDuration specified for update.");
                }

                //check if the name of the service types are

                entity.LastModifiedBy = request.UserId;
                entity.Duration = request.Duration;
                entity.DurationFrequency = request.DurationFrequency;

                _context.ContractDurations.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Contract Duration updated was successful!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract Duration update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }
}
