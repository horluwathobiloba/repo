using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.ExplorePost.Commands.ChangeExplorePostStatus
{
    public class ChangeExplorePostStatusCommandValidator: AbstractValidator<ChangeExplorePostStatusCommand>
    {
        public ChangeExplorePostStatusCommandValidator()
        {
            RuleFor(v => v.Id)
               .NotEmpty();

            RuleFor(v => v.LoggedInUserId)
              .NotEmpty();
        }
    }
}
