using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Update;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
        public UpdateReviewCommandValidator()
        {
		RuleFor(command => command.ReviewId)
			.SetValidator(new GuidValidator());

		RuleFor(command => command.Rating)
			.NotEmpty()
			.GreaterThanOrEqualTo(1)
			.LessThanOrEqualTo(5);

		RuleFor(command => command.Title)
			.MaximumLength(50);

		RuleFor(command => command.Text)
			.MaximumLength(1000);
	}
    }
