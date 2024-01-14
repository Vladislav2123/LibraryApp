using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Users.Commands.AddReadBook;

public class AddReadBookCommandValidator : AbstractValidator<AddReadBookCommand>
{
	public AddReadBookCommandValidator()
	{
		RuleFor(command => command.UserId)
			.SetValidator(new GuidValidator());

		RuleFor(command => command.BookId)
			.SetValidator(new GuidValidator());
	}
}
