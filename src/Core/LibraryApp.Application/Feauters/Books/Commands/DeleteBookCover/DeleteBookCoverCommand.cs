using MediatR;

namespace LibraryApp.Application.Feauters.Books.Commands.DeleteBookCover
{
	public record DeleteBookCoverCommand(
		Guid UserId,
		Guid BookId) : IRequest<Unit>;
}
