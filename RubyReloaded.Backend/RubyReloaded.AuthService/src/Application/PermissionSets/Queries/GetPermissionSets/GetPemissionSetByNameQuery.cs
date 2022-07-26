using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System.Collections.Generic;

namespace RubyReloaded.AuthService.Application.PermissionSets.Queries.GetPermissionSets
{
    public class GetPermissionSetByNameQuery : IRequest<Result>
    {
        public string Name { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetPermissionSetByNameQueryHandler : IRequestHandler<GetPermissionSetByNameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPermissionSetByNameQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetPermissionSetByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var feature = await _context.PermissionSets.Where(a => a.Name == request.Name).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync(); ;
                var features = _mapper.Map<List<PermissionSetDto>>(feature);
                return Result.Success(features);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving features by name was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }






    }
}
