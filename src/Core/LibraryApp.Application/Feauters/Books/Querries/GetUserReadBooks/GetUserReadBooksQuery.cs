using MediatR;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Common.Pagination;

namespace LibraryApp.Application.Feauters.Books.Querries.GetUserReadBooks;

public record GetUserReadBooksQuery(
	Guid UserId,
	Page Page)
	: IRequest<PagedList<BookLookupDto>>;
