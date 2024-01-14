using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Features.Authors.Commands.UpdateAuthorAvatar;

public record UpdateAuthorAvatarCommand(
	Guid AuthorId,
	IFormFile AvatarFile) 
	: IRequest<Unit>;
