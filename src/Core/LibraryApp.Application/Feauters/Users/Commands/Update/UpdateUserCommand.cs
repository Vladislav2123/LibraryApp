using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.Update
{
	public record UpdateUserCommand(
		Guid Id,
		string Name,
		string Email,
		string Login,
		DateOnly BirthDate) : IRequest<Unit>;
}
