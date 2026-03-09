using BookHub.Core.DTOs.CategoryDtos;
using FluentValidation;

namespace BookHub.Core.Validators
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name can't be longer than 50 characters");
        }
    }
}
