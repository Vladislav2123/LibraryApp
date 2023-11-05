using FluentValidation;

namespace LibraryApp.Application.Feauters.Users.Commands.DeleteReadBook
{
	public class DeleteReadBookCommandValidator : AbstractValidator<DeleteReadBookCommand>
	{
        public DeleteReadBookCommandValidator()
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
