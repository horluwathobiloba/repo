using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExplorePost.Queries.GetExplorePost
{
    public class GetExplorePostByTag : IRequest<Result>
    {
        public int TagId { get; set; }
    }

    public class GetExplorePostByTagQueryHandler : IRequestHandler<GetExplorePostByTag, Result>
    {
        private readonly IApplicationDbContext _context;
        
        public GetExplorePostByTagQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetExplorePostByTag request, CancellationToken cancellationToken)
        {
            try
            {
                var explorePosts = await _context.ExploreTagPosts
                    .Where(x => x.ExploreTagId == request.TagId)
                    .Include(x =>x.ExplorePost)
                    .ToListAsync();
                return Result.Success(explorePosts);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }

}
