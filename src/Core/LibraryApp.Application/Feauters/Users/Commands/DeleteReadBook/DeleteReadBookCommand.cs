using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.DeleteReadBook
{
	public record DeleteReadBookCommand(
		Guid UserId, 
		Guid BookId) : IRequest<Unit>;
}
