using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Update
{
	public record UpdateReviewCommand(
		Guid ReviewId,
		int Rating,
		string? Title,
		string? Text) : IRequest<Unit>;
}
