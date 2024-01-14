using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Reviews.Commands.Update;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
	public UpdateReviewCommandValidator()
	{
		RuleFor(command => command.ReviewId)
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
