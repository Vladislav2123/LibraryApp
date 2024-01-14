using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Users.Commands.DeleteReadBook;

public class DeleteReadBookCommandValidator : AbstractValidator<DeleteReadBookCommand>
{
	public DeleteReadBookCommandValidator()
	{
		RuleFor(command => command.UserId)
			.SetValidator(new GuidValidator());

		RuleFor(command => command.BookId)
			.SetValidator(new GuidValidator());
	}
}
