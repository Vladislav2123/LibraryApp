﻿using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Authors.Commands.DeleteAuthorAvatar;

public class DeleteAuthorAvatarCommandValidator : AbstractValidator<DeleteAuthorAvatarCommand>
{
	public DeleteAuthorAvatarCommandValidator()
	{
		RuleFor(command => command.AuthorId)
			.SetValidator(new GuidValidator());
	}
}