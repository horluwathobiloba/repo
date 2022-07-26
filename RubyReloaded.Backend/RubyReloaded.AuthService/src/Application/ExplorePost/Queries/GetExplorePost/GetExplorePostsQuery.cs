using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExplorePost.Queries.GetExplorePost
{
    public class GetExplorePostsQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetExplorePostsQueryHandler : IRequestHandler<GetExplorePostsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetExplorePostsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetExplorePostsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _context.ExplorePosts.Where(a => a.Status != Status.Deactivated)
                    .Include(x=>x.ExploreCategory)
                    .Include(x=>x.ExplorePostFile)
                   .Skip(request.Skip)
                   .Take(request.Take)
                     .ToListAsync();

                if (entities == null && entities.Count() <= 0)
                {
                    throw new NotFoundException(nameof(Domain.Entities.ExplorePost), request);
                }
                return Result.Success(entities);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
