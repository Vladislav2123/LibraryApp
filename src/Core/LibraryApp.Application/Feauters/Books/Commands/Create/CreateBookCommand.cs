using MediatR;

namespace LibraryApp.Application.Feauters.Books.Commands.Create
{
	public record CreateBookCommand(
		Guid AuthorId,
		string Name,
		string Description,
		int Year,
		string Text) : IRequest<Guid>;
}
