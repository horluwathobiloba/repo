using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractDuration.Queries.GetContractDurations
{
    public class GetContractDurationById : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }
    public class GetContractDurationByIdQueryHandler : IRequestHandler<GetContractDurationById, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService; 

        public GetContractDurationByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractDurationById request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }

                var contractDuration = await _context.ContractDurations.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id);
                if (contractDuration == null)
                {
                    throw new NotFoundException(nameof(contractDuration));
                }

                return Result.Success(contractDuration);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving durations. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
