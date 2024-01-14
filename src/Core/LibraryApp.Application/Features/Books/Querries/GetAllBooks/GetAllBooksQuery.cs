using MediatR;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Books.Querries.Dto;

namespace LibraryApp.Application.Features.Books.Querries.GetAllBooks;

public record GetAllBooksQuery(
	string? SearchTerms,
	Guid? AuthorId,
	string? SortColumn,
	string? SortOrder,
	Page Page)
	: IRequest<PagedList<BookLookupDto>>;
