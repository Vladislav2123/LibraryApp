using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Feauters.Authors.Commands.UpdateAuthorAvatar;

public record UpdateAuthorAvatarCommand(
	Guid AuthorId,
	IFormFile AvatarFile) 
	: IRequest<Unit>;
