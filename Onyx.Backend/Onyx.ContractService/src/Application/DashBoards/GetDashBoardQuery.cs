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

namespace Onyx.ContractService.Application.DashBoards
{
    public class GetDashBoardQuery : AuthToken, IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public DateTime WorkflowDate { get; set; }
    }

    public class GetDashBoardQueryHandler : IRequestHandler<GetDashBoardQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetDashBoardQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetDashBoardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);
                var entities = await _context.Contracts.Where(x => x.Status == Status.Active && x.OrganisationId == request.OrganizationId).ToListAsync();

                var entity = new
                {
                    Draft = entities.Where(x => !(x.IsAnExecutedDocument) && x.OrganisationId == request.OrganizationId&&x.CreatedDate.Date==request.WorkflowDate.Date).Count(),
                    Request=entities.Where(x => !(x.IsAnExecutedDocument) && x.OrganisationId == request.OrganizationId && x.CreatedDate.Date == request.WorkflowDate.Date).Count(),
                    Generated= entities.Where(x => !(x.IsAnExecutedDocument) && x.NextActorAction?.ToLower() == RecipientCategory.ContractGenerator.ToString().ToLower() && x.OrganisationId == request.OrganizationId && x.CreatedDate.Date == request.WorkflowDate.Date).Count(),
                    Signatory=entities.Where(x => !(x.IsAnExecutedDocument) && (x.NextActorAction?.ToLower() == RecipientCategory.InternalSignatory.ToString().ToLower() || x.NextActorAction?.ToLower() == RecipientCategory.ExternalSignatory.ToString().ToLower()) && x.OrganisationId == request.OrganizationId && x.CreatedDate.Date == request.WorkflowDate.Date).Count(),
                    internalApproval=entities.Where(x => !(x.IsAnExecutedDocument) && x.NextActorAction?.ToLower() == RecipientCategory.Approver.ToString().ToLower() && x.OrganisationId == request.OrganizationId && x.CreatedDate.Date == request.WorkflowDate.Date).Count()
                };
                return Result.Success(entity);
            }
            catch (Exception ex)
            {

                return Result.Failure($"An error occured while retrieving dashboard query. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");

            }            
        }
    }
}
