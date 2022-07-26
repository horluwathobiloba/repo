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
    public class GetAuditTrailQueryBySubscriberId : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class GetAuditTrailQueryBySubscriberIdHandler : IRequestHandler<GetAuditTrailQueryBySubscriberId, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAuditTrailQueryBySubscriberIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAuditTrailQueryBySubscriberId request, CancellationToken cancellationToken)
        {
            try
            {

                var response = await _context.AuditTrails.Where(a => a.SubscriberId == request.SubscriberId)
                                .Skip(request.Skip).Take(request.Take).ToListAsync();
                var auditTrail = _mapper.Map<List<AuditTrailDto>>(response);
                if (auditTrail == null)
                {
                    return Result.Failure($"Audit Trail by subscriber id {request.SubscriberId} does not exist.");
                }
                return Result.Success(auditTrail);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving audit trail for this subscriber id . Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
