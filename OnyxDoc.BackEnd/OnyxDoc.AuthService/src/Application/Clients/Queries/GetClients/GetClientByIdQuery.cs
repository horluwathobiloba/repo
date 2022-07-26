using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.Clients.Queries.GetClients
{
    public class GetClientByIdQuery : IRequest<Result>
    {
        public string  Id { get; set; }
    }

    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetClientByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Clients.FindAsync(request.Id);
                if (result == null)
                {
                    return Result.Failure("No Client not found with this id");
                }
                var clientVm = _mapper.Map<ClientDto>(result);
                return Result.Success("Client details retrieved successfully", clientVm);

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error getting client by Id", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
