using MediatR;

namespace LibraryApp.Application.Features.Users.Commands.Create;

public record CreateUserCommand(
	string Name,
	string Email,
	string Password,
	DateOnly BirthDate) 
	: IRequest<Guid>;
