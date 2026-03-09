using BookHub.Core.DTOs.Auth;
using FluentValidation;

namespace BookHub.Core.Validators
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username can't be longer than 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required when changing password")
                .When(x => x.NewPassword != null);

            RuleFor(x => x.NewPassword)
                .MinimumLength(6).WithMessage("New password must be at least 6 characters")
                .When(x => x.NewPassword != null);
        }
    }
}
