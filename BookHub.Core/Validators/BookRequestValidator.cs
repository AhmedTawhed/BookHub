using BookHub.Core.DTOs.BookDtos;
using FluentValidation;

namespace BookHub.Core.Validators
{
    public class BookRequestValidator : AbstractValidator<BookRequestDto>
    {
        public BookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title can't be longer than 100 characters");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required")
                .MaximumLength(100).WithMessage("Author can't be longer than 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description can't be longer than 1000 characters");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId is required");
        }
    }
}
