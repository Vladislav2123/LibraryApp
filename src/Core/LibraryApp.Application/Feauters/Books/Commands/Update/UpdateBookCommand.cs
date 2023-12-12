using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Feauters.Books.Commands.Update
{
	public record UpdateBookCommand(
		Guid BookId,
		Guid AuthorId,
		string Name,
		string Description,
		int Year,
		IFormFile? ContentFile) 
		: IRequest<Unit>;
}
