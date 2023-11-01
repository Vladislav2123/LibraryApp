using FluentValidation;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Delete
{
	public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
	{
        public DeleteReviewCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotNull()
                .NotEmpty();

			RuleFor(command => command.ReviewId)
				.NotNull()
				.NotEmpty();
		}
    }
}
