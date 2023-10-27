using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.Create
{
	public record CreateUserCommand(
		string Name,
		string Email,
		string Password,
		DateOnly BirthDate) : IRequest<Guid>;
}
