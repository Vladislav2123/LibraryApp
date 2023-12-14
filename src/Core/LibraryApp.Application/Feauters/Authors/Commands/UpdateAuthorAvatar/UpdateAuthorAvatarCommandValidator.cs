using FluentValidation;
using LibraryApp.Application.Common.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
