using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Books.Commands.DeleteBookCover;

public class DeleteBookCoverCommandValidator : AbstractValidator<DeleteBookCoverCommand>
{
	public DeleteBookCoverCommandValidator()
	{
		RuleFor(command => command.BookId)
			.SetValidator(new GuidValidator());
	}
}
