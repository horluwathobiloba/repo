using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Clients.Queries.GetAuditTrail
{
    public class GetAuditTrailQueryByAction : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public AuditAction AuditAction { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class GetAuditTrailQueryByActionHandler : IRequestHandler<GetAuditTrailQueryByAction, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAuditTrailQueryByActionHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAuditTrailQueryByAction request, CancellationToken cancellationToken)
        {
            try
            {

                var response = await _context.AuditTrails.Where(a => a.AuditAction == request.AuditAction && a.SubscriberId == request.SubscriberId)
                                .Skip(request.Skip).Take(request.Take).ToListAsync();
                var auditTrail = _mapper.Map<List<AuditTrailDto>>(response);
                if (auditTrail == null)
                {
                    return Result.Failure($"Audit Trail by action -  {request.AuditAction} does not exist.");
                }
                return Result.Success(auditTrail);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving audit trail. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
