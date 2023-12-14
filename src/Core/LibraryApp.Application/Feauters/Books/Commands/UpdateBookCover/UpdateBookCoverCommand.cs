using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Feauters.Books.Commands.UpdateBookCover;

public record UpdateBookCoverCommand(
	Guid BookId,
	IFormFile CoverFile) 
	: IRequest<Unit>;
