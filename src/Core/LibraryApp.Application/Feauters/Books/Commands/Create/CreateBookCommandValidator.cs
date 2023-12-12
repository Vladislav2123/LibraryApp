using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Books.Commands.Create
{
	public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
	{
        public CreateBookCommandValidator()
        {
			RuleFor(command => command.AuthorId)
				.SetValidator(new GuidValidator());

			RuleFor(command => command.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(command => command.Description)
                .MaximumLength(1000);

            RuleFor(command => command.Year)
                .Must(year => year == 0 || (year > 1000 && year < 2025))
                .WithMessage("Year must be in the range from 1000 to 2025 or 0");

            RuleFor(command => command.ContentFile)
                .NotNull()
                .SetValidator(new PdfFileValidator());
        }
    }
}
