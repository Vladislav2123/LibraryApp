using MediatR;

namespace LibraryApp.Application.Features.Books.Commands.DeleteBookCover;

public record DeleteBookCoverCommand(Guid BookId) 
	: IRequest<Unit>;
