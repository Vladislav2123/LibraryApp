using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Books.Commands.UpdateBookCover;

public class UpdateBookCoverValidator : AbstractValidator<UpdateBookCoverCommand>
{
        public UpdateBookCoverValidator()
        {
		RuleFor(command => command.BookId)
			.SetValidator(new GuidValidator());

		RuleFor(command => command.CoverFile)
                .NotNull()
                .SetValidator(new ImageFileValidator());
        }
    }
