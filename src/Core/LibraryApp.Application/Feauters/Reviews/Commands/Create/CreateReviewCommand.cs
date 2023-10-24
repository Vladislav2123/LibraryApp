using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create
{
    public record CreateReviewCommand(
        Guid UserId,
        Guid BookId,
        int Rating,
        string? Title,
        string? Text) : IRequest<Guid>;
}
