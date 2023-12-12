using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Books.Commands.Update
{
	public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
	{
        public UpdateBookCommandValidator()
        {
			RuleFor(command => command.BookId)
				.SetValidator(new GuidValidator());

			RuleFor(command => command.AuthorId)
				.NotNull()
				.NotEmpty();

			RuleFor(command => command.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(command => command.Description)
				.MaximumLength(1000);

			RuleFor(command => command.Year)
				.Must(year => year == 0 || (year > 1000 && year < 2025))
				.WithMessage("Year must be in the range from 1000 to 2025 or 0");

			RuleFor(command => command.ContentFile)
				.SetValidator(new PdfFileValidator());
		}
    }
}
