using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Update
{
	public record UpdateReviewCommand(Guid Id, int Rating, string? Title, string? Text) : IRequest<Unit>;
}
