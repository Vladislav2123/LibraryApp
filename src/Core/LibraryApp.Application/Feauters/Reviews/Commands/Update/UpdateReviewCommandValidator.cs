using FluentValidation;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Update
{
	public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
	{
        public UpdateReviewCommandValidator()
        {
			RuleFor(command => command.UserId)
				.NotNull()
				.NotEmpty();

			RuleFor(command => command.ReviewId)
				.NotNull()
				.NotEmpty();

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
}
