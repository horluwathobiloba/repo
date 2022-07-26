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

namespace Onyx.ContractService.Application.PinnedReports.Commands.CreatePinnedReportsCommand
{
    public class UpdatePinnedReportsCommand: AuthToken, IRequest< Result>
    {
        public int OrganisationId { get; set; }
        public int PinnedReportId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ContractStatus? ContractStatus { get; set; }
        public int? ProductTypeId { get; set; }
        public int? JobFunctionId { get; set; }
        public int? ContractTypeId { get; set; }
        public string? ContractReportName { get; set; }
        public DocumentType ModuleName { get; set; }
    }
    public class UpdatePinnedReportsCommandHanlder : IRequestHandler<UpdatePinnedReportsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public UpdatePinnedReportsCommandHanlder(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePinnedReportsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.IsValidOrganisation(request.AccessToken, request.OrganisationId);
                if (!respo)
                {
                    return Result.Failure("Inavlid OrganisationId");
                }
                var entity = await _context.ReportValues.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.PinnedReportId&&a.Status==Status.Active);
                if (entity==null)
                {
                    return Result.Failure($"Pinned report for this organisationId {request.OrganisationId} and pinnedReportId{request.PinnedReportId} does not exist or the status is not active");
                }
                entity.StartDate = request.StartDate;
                entity.EndDate = request.EndDate;
                entity.ContractStatus = request.ContractStatus;
                entity.JobFunctionId = request.JobFunctionId;
                entity.ProductTypeId = request.ProductTypeId;
                entity.ContractReportName = request.ContractReportName;
                entity.ContractTypeId = request.ContractTypeId;
                entity.ModuleName = request.ModuleName;
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedDate = DateTime.Now;
                _context.ReportValues.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Pinned report updated successfully!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Pinned report update failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
