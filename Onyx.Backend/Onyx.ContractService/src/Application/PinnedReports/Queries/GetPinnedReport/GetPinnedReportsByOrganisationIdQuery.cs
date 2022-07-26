using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractDuration.Queries.GetPinnedReport
{
    public class GetPinnedReportsByOrganisationIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }

    }

    public class GetPinnedReportsByOrganisationIdQueryHandler : IRequestHandler<GetPinnedReportsByOrganisationIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPinnedReportsByOrganisationIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPinnedReportsByOrganisationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.ReportValues.Where(a => a.OrganisationId == request.OrganisationId && a.Status==Status.Active).ToListAsync();
                if (entity==null||entity.Count==0)
                {
                    return Result.Failure("Pinned report for this organization does not exist.");
                }
                return Result.Success($"{entity.Count()} record(s) found", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving pinned reports . Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
