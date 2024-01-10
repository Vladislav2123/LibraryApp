using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.UpdateUserRole;
public record UpdateUserRoleCommand(
	Guid UserId,
	string Role) 
	: IRequest<Unit>;
