using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.Update;

public record UpdateUserCommand(
	Guid UserId,
	string Name,
	string Email,
	DateOnly BirthDate) 
	: IRequest<Unit>;
