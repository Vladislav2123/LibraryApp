using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Users.Commands.Delete
{
	public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
	{
        public DeleteUserCommandValidator()
        {
            RuleFor(command => command.Id)
				.SetValidator(new GuidValidator());
		}
    }
}
