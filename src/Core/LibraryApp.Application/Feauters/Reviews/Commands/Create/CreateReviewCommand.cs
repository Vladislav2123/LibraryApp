using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create
{
    public record CreateReviewCommand(
        Guid BookId,
        double Rating,
        string? Title,
        string? Text) 
        : IRequest<Guid>;
}
