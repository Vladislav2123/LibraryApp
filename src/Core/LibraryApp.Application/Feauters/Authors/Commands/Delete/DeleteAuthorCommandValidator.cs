using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Authors.Commands.Delete;

public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
{
        public DeleteAuthorCommandValidator()
        {
		RuleFor(command => command.AuthorId)
			.SetValidator(new GuidValidator());
	}
    }
