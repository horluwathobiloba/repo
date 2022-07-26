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
    public class GetContractTaskByIdQuery : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
    }
    public class GetContractTaskByIdQueryHandler : IRequestHandler<GetContractTaskByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetContractTaskByIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetContractTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.ContractTasks.FirstOrDefaultAsync(x=> x.Id == request.Id && x.OrganisationId==request.OrganisationId&&x.Status==Domain.Enums.Status.Active);
                if (entity == null)
                {
                    return Result.Failure("No  contract task available");
                }
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract task creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
