using MediatR;

namespace LibraryApp.Application.Users.Commands.Delete
{
	public record DeleteUserCommand(Guid Id) : IRequest<Unit>;
}
