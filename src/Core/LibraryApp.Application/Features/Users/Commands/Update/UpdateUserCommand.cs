using MediatR;

namespace LibraryApp.Application.Features.Users.Commands.Update;

public record UpdateUserCommand(
	Guid UserId,
	string Name,
	string Email,
	DateOnly BirthDate) 
	: IRequest<Unit>;
