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

namespace Onyx.ContractService.Application.ContractDuration.Commands.CreateContractDurations
{
    public class CreateContractDurationsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<ContractDurationRequest> Durations { get; set; }
        public string UserId { get; set; }
    }
    public class CreateContractDurationsCommandHandler : IRequestHandler<CreateContractDurationsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateContractDurationsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateContractDurationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = new List<Domain.Entities.ContractDuration>();

                await _context.BeginTransactionAsync();

                foreach (var duration in request.Durations)
                {
                    var exists = await _context.ContractDurations.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Duration == duration.Duration && x.DurationFrequency == duration.DurationFrequency);
                    if (exists)
                    {
                        return Result.Failure($"A record with the duration already exists!");
                    }
                    var entity = new Domain.Entities.ContractDuration
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        DurationFrequency = duration.DurationFrequency,
                        Duration = duration.Duration,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.ContractDurations.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Contract Duration created successfully!",list);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract Duration creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
