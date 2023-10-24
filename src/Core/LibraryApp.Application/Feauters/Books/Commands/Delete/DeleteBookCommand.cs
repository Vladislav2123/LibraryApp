using MediatR;

namespace LibraryApp.Application.Feauters.Books.Commands.Delete
{
	public record DeleteBookCommand(Guid Id) : IRequest<Unit>;
}
