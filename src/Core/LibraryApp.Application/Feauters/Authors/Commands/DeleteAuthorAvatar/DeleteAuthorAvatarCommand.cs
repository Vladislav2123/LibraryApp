using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.DeleteAuthorAvatar
{
	public record DeleteAuthorAvatarCommand(Guid Id) 
		: IRequest<Unit> { }
}
