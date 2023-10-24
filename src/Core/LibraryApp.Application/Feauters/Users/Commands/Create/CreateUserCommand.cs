using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.Create
{
	public record CreateUserCommand(
		string Name,
		string Email,
		string Login,
		string Password,
		DateOnly BirthDate) : IRequest<Guid>;
}
