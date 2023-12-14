using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.DeleteUserAvatar;

public record DeleteUserAvatarCommand(Guid UserId) 
	: IRequest<Unit>;
