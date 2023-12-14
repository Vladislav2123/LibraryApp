using MediatR;

namespace LibraryApp.Application.Feauters.Books.Commands.DeleteBookCover;

public record DeleteBookCoverCommand(Guid BookId) 
	: IRequest<Unit>;
