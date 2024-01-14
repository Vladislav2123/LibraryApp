using MediatR;

namespace LibraryApp.Application.Features.Authors.Commands.DeleteAuthorAvatar;

public record DeleteAuthorAvatarCommand(Guid AuthorId) 
	: IRequest<Unit>;
