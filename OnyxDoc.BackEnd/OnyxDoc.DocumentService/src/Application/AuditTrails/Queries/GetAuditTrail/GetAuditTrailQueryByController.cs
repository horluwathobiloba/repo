using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.AuditTrails.Queries.GetAuditTrail
{
    public class GetAuditTrailQueryByController : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string ControllerName { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class GetAuditTrailQueryByControllerHandler : IRequestHandler<GetAuditTrailQueryByController, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAuditTrailQueryByControllerHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAuditTrailQueryByController request, CancellationToken cancellationToken)
        {
            try
            {

                var response = await _context.AuditTrails.Where(a => a.ControllerName == request.ControllerName && a.SubscriberId == request.SubscriberId)
                                .Skip(request.Skip).Take(request.Take).ToListAsync();
                var auditTrail = _mapper.Map<List<AuditTrailDto>>(response);
                if (auditTrail == null)
                {
                    return Result.Failure($"Audit Trail by controller name {request.ControllerName} does not exist.");
                }
                return Result.Success(auditTrail);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving audit trail . Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
