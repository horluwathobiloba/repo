using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using OnyxDoc.AuthService.Application.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.Clients.Queries.GetClients
{
    public class GetClientsQuery : IRequest<Result>
    {
    }

    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetClientsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        }

        public async Task<Result> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var clientList = await _context.Clients.ToListAsync();
                if (clientList == null || clientList.Count == 0)
                {
                    return Result.Failure("No clients exist");
                }
                
                var clientLists = _mapper.Map<List<ClientListDto>>(clientList);
                return Result.Success("Client Lists retrieved successfully", clientLists);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error getting clients", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
