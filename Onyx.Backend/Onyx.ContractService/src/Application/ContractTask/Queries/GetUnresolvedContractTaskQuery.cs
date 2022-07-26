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

namespace Onyx.ContractService.Application.ContractTask.Queries
{
    public class GetUnresolvedContractTaskQuery :AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetUnresolvedContractTaskQueryHandler : IRequestHandler<GetUnresolvedContractTaskQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetUnresolvedContractTaskQueryHandler(IApplicationDbContext context,IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetUnresolvedContractTaskQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.ContractTasks.Where(x=>x.OrganisationId == request.OrganisationId && x.ContractTaskStatus == Domain.Enums.ContractTaskStatus.Unresolved && x.Status == Domain.Enums.Status.Active).ToListAsync();
                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No unresolved contract task available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract task creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
