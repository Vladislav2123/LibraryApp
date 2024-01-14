using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Users.Commands.DeleteUserAvatar;

public class DeleteUserAvaterCommandValidator : AbstractValidator<DeleteUserAvatarCommand>
{
	public DeleteUserAvaterCommandValidator()
	{
		RuleFor(command => command.UserId)
			.SetValidator(new GuidValidator());
	}
}
