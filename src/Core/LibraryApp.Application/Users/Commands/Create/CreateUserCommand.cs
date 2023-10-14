using MediatR;

namespace LibraryApp.Application.Users.Commands.Create
{
	public record CreateUserCommand(
		string Name,
		string Email,
		string Login,
		string Password,
		DateTime BirthDate) : IRequest<Guid>;
}
