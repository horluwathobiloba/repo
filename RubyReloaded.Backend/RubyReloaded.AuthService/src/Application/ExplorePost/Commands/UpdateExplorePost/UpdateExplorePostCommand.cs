using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExplorePost.Commands.UpdateExplorePost
{
    public class UpdateExplorePostCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public ExploreImageType ExploreImageType { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class UpdateExplorePostCommandHandler : IRequestHandler<UpdateExplorePostCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateExplorePostCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateExplorePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var explorePost = await _context.ExplorePosts.FindAsync(request.Id);
                if (explorePost is null)
                {
                    return Result.Failure("Post could not be found");
                }
                explorePost.Body = request.Body;
               
                explorePost.Header = request.Header;
                explorePost.SubHeader = request.SubHeader;
                _context.ExplorePosts.Update(explorePost);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(explorePost);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Post update failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
