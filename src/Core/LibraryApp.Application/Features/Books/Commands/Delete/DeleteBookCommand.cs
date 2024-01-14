using MediatR;

namespace LibraryApp.Application.Features.Books.Commands.Delete;

public record DeleteBookCommand(Guid BookId) 
	: IRequest<Unit>;
