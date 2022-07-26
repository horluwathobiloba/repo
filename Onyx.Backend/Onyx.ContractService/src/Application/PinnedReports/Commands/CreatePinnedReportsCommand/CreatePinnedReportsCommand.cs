using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Commands.CreateContract;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contractaudit.Commands.CreatePinnedReportsCommand
{
    public class CreatePinnedReportsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ContractStatus? ContractStatus { get; set; }
        public int? ProductTypeId { get; set; }
        public int? JobFunctionId { get; set; }
        public int? ContractTypeId { get; set; }
        public string? ContractReportName { get; set; }
        public DocumentType ModuleName { get; set; }

    }
    public class CreatePinnedReportsCommandHandler : IRequestHandler<CreatePinnedReportsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePinnedReportsCommandHandler(IApplicationDbContext context,IAuthService authService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePinnedReportsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var report = await _context.ReportValues.FirstOrDefaultAsync(a => a.ContractReportName == request.ContractReportName);
                if (report != null)
                {
                    return Result.Failure("Contract report name already exist!!");
                }
                //create pinned report
                var pinnedReport = new ReportValue
                {
                    OrganisationId = request.OrganisationId,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    ContractStatus = request.ContractStatus,
                    JobFunctionId = request.JobFunctionId,
                    ProductTypeId = request.ProductTypeId,
                    ContractReportName = request.ContractReportName,
                    ContractTypeId = request.ContractTypeId,
                    ModuleName = request.ModuleName,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };
                await _context.ReportValues.AddAsync(pinnedReport);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Pinned report created successfully!", pinnedReport);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Pinned report creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
