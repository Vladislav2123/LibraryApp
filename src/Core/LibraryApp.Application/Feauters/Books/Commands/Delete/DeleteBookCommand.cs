using MediatR;

namespace LibraryApp.Application.Feauters.Books.Commands.Delete
{
	public record DeleteBookCommand(
		Guid UserId,
		Guid BookId) : IRequest<Unit>;
}
