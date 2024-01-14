using MediatR;

namespace LibraryApp.Application.Features.Users.Commands.DeleteUserAvatar;

public record DeleteUserAvatarCommand(Guid UserId) 
	: IRequest<Unit>;
