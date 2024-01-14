using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Features.Users.Commands.UpdateUserAvatar;

public record UpdateUserAvatarCommand(
	Guid UserId,
	IFormFile AvatarFile) 
	: IRequest<Unit>;
