using MediatR;

namespace LibraryApp.Application.Features.Users.Commands.AddReadBook;

public record AddReadBookCommand(
	Guid UserId,
	Guid BookId) 
	: IRequest<Unit>;
