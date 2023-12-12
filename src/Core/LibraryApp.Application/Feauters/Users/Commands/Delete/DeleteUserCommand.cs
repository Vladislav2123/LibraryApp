using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.Delete
{
	public record DeleteUserCommand(Guid UserId) 
		: IRequest<Unit>;
}
