using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQQuestion.Commands.ChangeFAQQuestionStatus
{
    public class ChangeFAQQuestionStatusCommand:IRequest<Result>
    {
        public int Id { get; set; }
        public string LoggedInUserId { get; set; }
    }
    public class ChangeFAQQuestionStatusCommandHandler : IRequestHandler<ChangeFAQQuestionStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeFAQQuestionStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeFAQQuestionStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _context.FAQQuestions.FindAsync(request.Id);
                string message = "";
                switch (question.Status)
                {
                    case Status.Active:
                        question.Status = Status.Inactive;
                        message = "FAQ Question deactivation was successful";
                        break;
                    case Status.Inactive:
                        question.Status = Status.Active;
                        message = "FAQ Question activation was successful";
                        break;
                    case Status.Deactivated:
                        question.Status = Status.Active;
                        message = "FAQ Question activation was successful";
                        break;
                    default:
                        break;
                }
                _context.FAQQuestions.Update(question);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("FAQ Category status updated successfully", question);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "FAQ Question status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
