using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Users.Commands.UpdateUserAvatar
{
	public class UpdateUserAvatarCommandValidator : AbstractValidator<UpdateUserAvatarCommand>
	{
        public UpdateUserAvatarCommandValidator()
        {
            RuleFor(command => command.UserId)
                .SetValidator(new GuidValidator());

            RuleFor(command => command.AvatarFile)
                .NotNull()
                .SetValidator(new ImageFileValidator());
        }
    }
}
