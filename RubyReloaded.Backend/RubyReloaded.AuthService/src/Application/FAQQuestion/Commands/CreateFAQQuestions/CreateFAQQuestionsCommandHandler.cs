using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.FAQQuestion.Commands.CreateFAQQuestions
{
    public class CreateFAQQuestionsCommandValidator:AbstractValidator<CreateFAQQuestionsCommand>
    {
        public CreateFAQQuestionsCommandValidator()
        {
            RuleFor(v => v.Answer)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.CategoryId)
                .NotNull()
                .NotNull();
            RuleFor(v => v.TagVms)
                .NotEmpty();

        }
    }
}
