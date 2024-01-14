using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Reviews.Commands.Delete;

public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
	public DeleteReviewCommandValidator()
	{
		RuleFor(command => command.ReviewId)
			.SetValidator(new GuidValidator());
	}
}
