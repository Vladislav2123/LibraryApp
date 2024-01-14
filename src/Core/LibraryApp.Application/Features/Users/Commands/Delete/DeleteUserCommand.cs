using MediatR;

namespace LibraryApp.Application.Features.Users.Commands.Delete;

public record DeleteUserCommand(Guid UserId) 
	: IRequest<Unit>;
