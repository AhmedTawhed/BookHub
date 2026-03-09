using BookHub.Core.DTOs.ReviewDtos;
using FluentValidation;

namespace BookHub.Core.Validators
{
    public class ReviewRequestValidator : AbstractValidator<ReviewRequestDto>
    {
        public ReviewRequestValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("BookId is required");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment can't exceed 500 characters")
                .When(x => x.Comment != null);
        }
    }
}
