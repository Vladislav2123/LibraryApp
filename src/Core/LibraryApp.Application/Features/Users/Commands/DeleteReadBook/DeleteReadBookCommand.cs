using MediatR;

namespace LibraryApp.Application.Features.Users.Commands.DeleteReadBook;

public record DeleteReadBookCommand(
	Guid UserId,
	Guid BookId) 
	: IRequest<Unit>;
