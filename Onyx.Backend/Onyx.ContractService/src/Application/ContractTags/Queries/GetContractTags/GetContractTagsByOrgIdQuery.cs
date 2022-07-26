using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTags.Queries
{
    public class GetContractTagsByOrgIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetContractTagsByOrgIdQueryHandler : IRequestHandler<GetContractTagsByOrgIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetContractTagsByOrgIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetContractTagsByOrgIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.ContractTags.Where(x=> x.OrganisationId == request.OrganisationId
                                                               && x.Status == Domain.Enums.Status.Active).ToListAsync();
                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No Contract Tags available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get Contract Tags failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
