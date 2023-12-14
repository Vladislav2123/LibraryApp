using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.AddReadedBook;

public record AddReadBookCommand(
	Guid UserId,
	Guid BookId) 
	: IRequest<Unit>;
