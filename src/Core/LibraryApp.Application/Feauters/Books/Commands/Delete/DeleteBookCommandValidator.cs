using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Books.Commands.Delete
{
	public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
	{
        public DeleteBookCommandValidator()
        {
			RuleFor(command => command.BookId)
				.SetValidator(new GuidValidator());
		}
    }
}
