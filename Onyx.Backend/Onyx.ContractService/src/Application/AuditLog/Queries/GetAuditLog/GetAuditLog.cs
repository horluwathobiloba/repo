using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractDuration.Queries.GetAuditLog
{
    public class GetAuditLog : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetAuditLogHandler : IRequestHandler<GetAuditLog, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService; 

        public GetAuditLogHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetAuditLog request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }

                var auditLog = await _context.AuditLogs.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                if (auditLog == null)
                {
                    return Result.Failure("Audit Log for this organization does not exist.");
                }

                //var result = _mapper.Map<AuditLogDto>(auditLog);
                return Result.Success(auditLog);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving Audits. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
