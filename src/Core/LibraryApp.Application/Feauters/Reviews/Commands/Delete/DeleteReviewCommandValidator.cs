using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Delete
{
	public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
	{
        public DeleteReviewCommandValidator()
        {
            RuleFor(command => command.UserId)
				.SetValidator(new GuidValidator());

			RuleFor(command => command.ReviewId)
				.SetValidator(new GuidValidator());
		}
    }
}
