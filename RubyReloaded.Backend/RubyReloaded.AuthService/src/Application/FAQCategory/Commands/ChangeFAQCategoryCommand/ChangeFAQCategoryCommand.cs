using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQCategory.Commands.ChangeFAQCategoryCommand
{
    public class ChangeFAQCategoryCommand:IRequest<Result>
    {
        public int FAQCategoryId { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangeFAQCategoryCommandHandler : IRequestHandler<ChangeFAQCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        //   private readonly IIdentityService _identityService;
        public ChangeFAQCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeFAQCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var faqCategory = await _context.FAQCategories.FindAsync(request.FAQCategoryId);
                string message = "";
                switch (faqCategory.Status)
                {
                    case Status.Active:
                        faqCategory.Status = Status.Inactive;
                        message = "FAQ Category deactivation was successful";
                        break;
                    case Status.Inactive:
                        faqCategory.Status = Status.Active;
                        message = "FAQ Category activation was successful";
                        break;
                    case Status.Deactivated:
                        faqCategory.Status = Status.Active;
                        message = "FAQ Category activation was successful";
                        break;
                    default:
                        break;
                }
                _context.FAQCategories.Update(faqCategory);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message, faqCategory);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "FAQ Category status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
