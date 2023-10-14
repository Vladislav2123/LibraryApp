using MediatR;

namespace LibraryApp.Application.Books.Commands.Delete
{
	public record DeleteBookCommand(Guid Id) : IRequest<Unit>;
}
