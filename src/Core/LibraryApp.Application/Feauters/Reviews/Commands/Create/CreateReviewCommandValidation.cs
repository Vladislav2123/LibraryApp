using FluentValidation;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create
{
	public class CreateReviewCommandValidation : AbstractValidator<CreateReviewCommand>
	{
        public CreateReviewCommandValidation()
        {
            RuleFor(command => command.UserId)
                .NotNull()
                .NotEmpty();

			RuleFor(command => command.BookId)
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
