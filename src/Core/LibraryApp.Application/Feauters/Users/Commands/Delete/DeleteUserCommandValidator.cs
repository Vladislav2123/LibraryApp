using FluentValidation;

namespace LibraryApp.Application.Feauters.Users.Commands.Delete
{
	public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
	{
        public DeleteUserCommandValidator()
        {
            RuleFor(command => command)
                .NotNull()
                .NotEmpty();
        }
    }
}
