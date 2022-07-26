using FluentValidation;

namespace OnyxDoc.DocumentService.Application.Dashboard.CreateDashboardDetails
{
    public class CreateDashboardCommandValidator : AbstractValidator<CreateDashboardCommand>
    {
        public CreateDashboardCommandValidator()
        { 
             RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
