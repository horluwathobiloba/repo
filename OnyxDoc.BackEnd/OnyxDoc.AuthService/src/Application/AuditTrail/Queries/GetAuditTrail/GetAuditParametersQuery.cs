using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Clients.Queries.GetAuditTrail
{
    public class GetAuditParametersQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
       
    }
    public class GetAuditParametersQueryHandler : IRequestHandler<GetAuditParametersQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAuditParametersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAuditParametersQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var parameters = await _context.AuditTrails.Select(x => new { Controller = x.ControllerName, MicroserviceName = x.MicroserviceName }).Distinct().ToListAsync();

                if (parameters == null)
                {
                    return Result.Failure("Parameter and Service Name does not exist.");
                }
                return Result.Success(parameters);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving parameter and service name. Error: {ex?.Message + " " + ex?.InnerException?.Message}");
            }
        }
    }
}