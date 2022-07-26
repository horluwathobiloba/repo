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

namespace Onyx.ContractService.Application.ContractDuration.Commands.CreateContractDuration
{
    public class CreateContractDurationCommand : AuthToken, IRequest<Result>
    {
        public int Duration { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public int OrganisationId { get; set; }
        public DurationFrequency DurationFrequency { get; set; }
    }
    public class CreateContractDurationCommandHandler : IRequestHandler<CreateContractDurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateContractDurationCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;

        }
        public async Task<Result> Handle(CreateContractDurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var exists = await _context.ContractDurations.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.DurationFrequency == request.DurationFrequency && x.Duration == request.Duration);
                if (exists)
                {
                    return Result.Failure($"A record with the duration '{request.Duration}' '{request.DurationFrequency}' already exists!");
                }
                var entity = new Domain.Entities.ContractDuration
                {
                    OrganisationId = request.OrganisationId,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.CreatedBy,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    DurationFrequency = request.DurationFrequency,
                    Duration = request.Duration,
                    StatusDesc = Status.Active.ToString()
                };
                await _context.ContractDurations.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Contract duration created successfully!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract duration creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
