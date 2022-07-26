using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExplorePost.Queries.GetExplorePost
{
    public class GetExplorePostById : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetExploresByIdQueryHandler : IRequestHandler<GetExplorePostById, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetExploresByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetExplorePostById request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ExplorePosts.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (entity is null || entity?.Status == Domain.Enums.Status.Deactivated)
                {
                    return Result.Failure("Invalid Id");
                }
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retreival was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
