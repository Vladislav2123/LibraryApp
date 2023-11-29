using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create
{
	public class CreateReviewCommandValidation : AbstractValidator<CreateReviewCommand>
	{
        public CreateReviewCommandValidation()
        {
            RuleFor(command => command.UserId)
				.SetValidator(new GuidValidator());

			RuleFor(command => command.BookId)
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
}
