using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.Delete
{
	public record DeleteUserCommand(Guid Id) : IRequest<Unit>;
}
