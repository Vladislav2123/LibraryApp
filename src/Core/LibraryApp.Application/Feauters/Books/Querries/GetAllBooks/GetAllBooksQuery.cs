using MediatR;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Common.Pagination;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBooks;

    public record GetAllBooksQuery(
        string? SearchTerms,
        Guid? AuthorId, 
        string? SortColumn, 
        string? SortOrder,
        Page Page) 
        : IRequest<PagedList<BookLookupDto>>;
