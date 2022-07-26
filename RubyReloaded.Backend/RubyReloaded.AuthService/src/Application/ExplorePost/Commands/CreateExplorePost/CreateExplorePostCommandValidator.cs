using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.ExplorePost.Commands.CreateExplorePost
{
    public class CreateExplorePostCommandValidator: AbstractValidator<CreateExplorePostCommand>
    {
        public CreateExplorePostCommandValidator()
        {
            RuleFor(v => v.CategoryId)
             .NotEmpty();

            RuleFor(v => v.Header)
              .NotEmpty();
        
            RuleFor(v => v.UserId)
              .NotEmpty();

            RuleFor(v => v.ExploreImageType)
             .NotEmpty();

            RuleFor(v => v.TagVms)
              .NotEmpty();
        }
    }
}
