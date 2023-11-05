using FluentValidation;

namespace LibraryApp.Application.Feauters.Users.Commands.AddReadedBook
{
	public class AddReadBookCommandValidator : AbstractValidator<AddReadBookCommand>
	{
        public AddReadBookCommandValidator()
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
