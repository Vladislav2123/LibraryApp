using MediatR;
using LibraryApp.Application.Features.Books.Querries.Dto;

namespace LibraryApp.Application.Features.Books.Querries.GetBook;

public record GetBookQuery(Guid BookId)
	: IRequest<BookDto>;
