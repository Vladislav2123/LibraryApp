using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Features.Books.Commands.UpdateBookCover;

public record UpdateBookCoverCommand(
	Guid BookId,
	IFormFile CoverFile) 
	: IRequest<Unit>;
