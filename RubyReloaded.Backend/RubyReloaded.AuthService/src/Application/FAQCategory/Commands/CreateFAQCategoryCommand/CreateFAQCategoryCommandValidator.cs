using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.FAQCategory.Commands.CreateFAQCategoryCommand
{
    public class CreateFAQCategoryCommandValidator:AbstractValidator<CreateFAQCategoryCommand>
    {
        public CreateFAQCategoryCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
