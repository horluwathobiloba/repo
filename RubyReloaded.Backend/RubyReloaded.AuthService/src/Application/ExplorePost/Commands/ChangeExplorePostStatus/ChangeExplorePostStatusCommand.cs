using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExplorePost.Commands.ChangeExplorePostStatus
{
    public class ChangeExplorePostStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangeExplorePostStatusStatusCommandHandler : IRequestHandler<ChangeExplorePostStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeExplorePostStatusStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeExplorePostStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _context.ExplorePosts.FindAsync(request.Id);
                string message = "";
                switch (question.Status)
                {
                    case Status.Active:
                        question.Status = Status.Inactive;
                        message = "Post deactivation was successful";
                        break;
                    case Status.Inactive:
                        question.Status = Status.Active;
                        message = "Post activation was successful";
                        break;
                    case Status.Deactivated:
                        question.Status = Status.Active;
                        message = "Post activation was successful";
                        break;
                    default:
                        break;
                }
                _context.ExplorePosts.Update(question);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Post status updated successfully", question);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}
