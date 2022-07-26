using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Reports.Command
{
    public class TerminatePinnedReportCommand: AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int PinnedReportId { get; set; }
    }
    public class TerminatePinnedReportHanlder : IRequestHandler<TerminatePinnedReportCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public TerminatePinnedReportHanlder(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(TerminatePinnedReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.IsValidOrganisation(request.AccessToken, request.OrganisationId);
                if (!respo)
                {
                    return Result.Failure("Inavlid OrganisationId");
                }
                var entity = await _context.ReportValues.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.PinnedReportId && a.Status==Domain.Enums.Status.Active);
                if (entity == null)
                {
                    return Result.Failure($"Pinned report for this orgnisationId, {request.OrganisationId} and pinnedReportId {request.PinnedReportId} does not exist, or the status is not active");
                }
                entity.Status = Domain.Enums.Status.Inactive;
                entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                 _context.ReportValues.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Pinned report terminated sucessfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Pinned report termination failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
