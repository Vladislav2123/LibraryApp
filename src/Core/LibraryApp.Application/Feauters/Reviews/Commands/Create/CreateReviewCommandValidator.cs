using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
	public CreateReviewCommandValidator()
	{
		RuleFor(command => command.BookId)
			.SetValidator(new GuidValidator());

		RuleFor(command => command.Rating)
			.NotNull()
			.GreaterThanOrEqualTo((byte)1)
			.LessThanOrEqualTo((byte)5);

		RuleFor(command => command.Title)
			.MaximumLength(50);

		RuleFor(command => command.Comment)
			.MaximumLength(1000);
	}
}
