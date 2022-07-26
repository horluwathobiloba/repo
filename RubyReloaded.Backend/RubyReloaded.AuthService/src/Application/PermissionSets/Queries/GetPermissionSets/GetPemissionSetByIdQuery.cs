using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;

namespace RubyReloaded.AuthService.Application.PermissionSets.Queries.GetPermissionSets
{
    public class GetPermissionSetByIdQuery : IRequest<Result>
    {
        public int  Id { get; set; }
    }

    public class GetPermissionSetByIdQueryHandler : IRequestHandler<GetPermissionSetByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPermissionSetByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetPermissionSetByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var feature = await _context.PermissionSets.FirstOrDefaultAsync(a=>a.Id == request.Id);
                var features = _mapper.Map<PermissionSetDto>(feature);
                return Result.Success(features);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
