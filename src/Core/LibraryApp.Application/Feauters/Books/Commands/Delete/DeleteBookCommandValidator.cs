using FluentValidation;

namespace LibraryApp.Application.Feauters.Books.Commands.Delete
{
	public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
	{
        public DeleteBookCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.BookId)
                .NotNull()
                .NotEmpty();
        }
    }
}
