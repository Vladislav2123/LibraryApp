using MediatR;

namespace LibraryApp.Application.Feauters.Books.Commands.Update
{
	public record UpdateBookCommand(
		Guid UserId,
		Guid BookId,
		Guid AuthorId,
		string Name,
		string Description,
		int Year) : IRequest<Unit>;
}
