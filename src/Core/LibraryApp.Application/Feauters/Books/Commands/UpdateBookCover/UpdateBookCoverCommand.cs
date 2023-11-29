using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Feauters.Books.Commands.UpdateBookCover
{
	public record UpdateBookCoverCommand(
		Guid UserId,
		Guid BookId,
		IFormFile CoverFile) : IRequest<Unit>;
}
