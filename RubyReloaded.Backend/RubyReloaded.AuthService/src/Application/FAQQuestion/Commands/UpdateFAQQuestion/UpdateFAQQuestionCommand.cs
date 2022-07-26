using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQQuestion.Commands.UpdateFAQQuestion
{
    public class UpdateFAQQuestionCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class UpdateFAQQuestionCommandHandler : IRequestHandler<UpdateFAQQuestionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateFAQQuestionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateFAQQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var questionToUpdate = await _context.FAQQuestions.FindAsync(request.Id);
                if (questionToUpdate is null)
                {
                    return Result.Failure("Question could not be found");
                }
                questionToUpdate.Question = request.Question;
                questionToUpdate.Answer = request.Answer;
                _context.FAQQuestions.Update(questionToUpdate);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(questionToUpdate);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "FAQ Category creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
