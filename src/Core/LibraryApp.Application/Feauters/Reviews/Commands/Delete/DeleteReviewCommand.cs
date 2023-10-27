using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Delete
{
	public record DeleteReviewCommand(
		Guid UserId,
		Guid ReviewId) : IRequest<Unit>;
}
