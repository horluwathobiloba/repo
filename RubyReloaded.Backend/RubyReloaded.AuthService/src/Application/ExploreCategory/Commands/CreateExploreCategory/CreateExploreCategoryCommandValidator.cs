using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.ExploreCategory.Commands.CreateExploreCategory
{
    public class CreateExploreCategoryCommandValidator:AbstractValidator<CreateExploreCategoryCommand>
    {
        public CreateExploreCategoryCommandValidator()
        {
            RuleFor(v => v.Name)
               .MaximumLength(200)
               .NotEmpty();
        }
    }
}
