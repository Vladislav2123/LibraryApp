using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Authors.Commands.UpdateAuthorAvatar;

public class UpdateAuthorAvatarCommandValidator : AbstractValidator<UpdateAuthorAvatarCommand>
{
        public UpdateAuthorAvatarCommandValidator()
        {
            RuleFor(command => command.AuthorId)
                .SetValidator(new GuidValidator());

            RuleFor(command => command.AvatarFile)
                .NotNull()
                .SetValidator(new ImageFileValidator());
        }
    }
