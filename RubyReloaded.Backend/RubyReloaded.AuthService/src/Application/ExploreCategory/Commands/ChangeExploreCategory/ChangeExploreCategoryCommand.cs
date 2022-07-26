using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExploreCategory.Commands.ChangeExploreCategory
{
    public class ChangeExploreCategoryCommand : IRequest<Result>
    {
        public int ExploreCategoryId { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangeExploreCategoryCommandHandler : IRequestHandler<ChangeExploreCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        //   private readonly IIdentityService _identityService;
        public ChangeExploreCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeExploreCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ExploreCategory = await _context.ExploreCategories.FindAsync(request.ExploreCategoryId);
                string message = "";
                switch (ExploreCategory.Status)
                {
                    case Status.Active:
                        ExploreCategory.Status = Status.Inactive;
                        message = "Explore Category deactivation was successful";
                        break;
                    case Status.Inactive:
                        ExploreCategory.Status = Status.Active;
                        message = "Explore Category activation was successful";
                        break;
                    case Status.Deactivated:
                        ExploreCategory.Status = Status.Active;
                        message = "Explore Category activation was successful";
                        break;
                    default:
                        break;
                }
                _context.ExploreCategories.Update(ExploreCategory);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message, ExploreCategory);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Explore Category status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
