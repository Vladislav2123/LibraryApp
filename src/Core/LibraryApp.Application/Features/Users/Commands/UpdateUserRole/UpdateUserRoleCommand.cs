using MediatR;

namespace LibraryApp.Application.Features.Users.Commands.UpdateUserRole;
public record UpdateUserRoleCommand(
	Guid UserId,
	string Role) 
	: IRequest<Unit>;
