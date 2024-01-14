using MediatR;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Books.Querries.Dto;

namespace LibraryApp.Application.Features.Books.Querries.GetUserReadBooks;

public record GetUserReadBooksQuery(
	Guid UserId,
	Page Page)
	: IRequest<PagedList<BookLookupDto>>;
