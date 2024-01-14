using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Users.Commands.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
	public DeleteUserCommandValidator()
	{
		RuleFor(command => command.UserId)
		.SetValidator(new GuidValidator());
	}
}
