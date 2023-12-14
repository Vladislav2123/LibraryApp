using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Feauters.Users.Commands.UpdateUserAvatar;

public record UpdateUserAvatarCommand(
	Guid UserId,
	IFormFile AvatarFile) 
	: IRequest<Unit>;
