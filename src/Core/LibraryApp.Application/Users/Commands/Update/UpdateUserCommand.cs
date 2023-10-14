using MediatR;

namespace LibraryApp.Application.Users.Commands.Update
{
	public record UpdateUserCommand(
		Guid Id,
		string Name,
		string Email,
		string Login,
		DateTime BirthDate) : IRequest<Unit>;
}
