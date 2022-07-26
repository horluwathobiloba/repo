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

namespace Onyx.ContractService.Application.ContractComments.Queries
{
    public class GetContractCommentsByOrgIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetContractCommentsByOrgIdQueryHandler : IRequestHandler<GetContractCommentsByOrgIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetContractCommentsByOrgIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetContractCommentsByOrgIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
               // var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.ContractComments.Where(x=> x.OrganisationId == request.OrganisationId
                                                               && x.Status == Domain.Enums.Status.Active).ToListAsync();
                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No contract comments available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get contract comments failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
