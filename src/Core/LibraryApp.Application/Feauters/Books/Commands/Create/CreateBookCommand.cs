using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Feauters.Books.Commands.Create
{
	public record CreateBookCommand(
		Guid UserId,
		Guid AuthorId,
		string Name,
		string Description,
		int Year,
		IFormFile ContentFile) : IRequest<Guid>;
}
