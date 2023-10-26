using MediatR;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Common.Helpers.Pagination;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBooks
{
    public record GetBooksQuery(
        string? SearchTerms,
        Guid? AuthorId, 
        string? SortColumn, 
        string? SortOrder,
        Page Page) : IRequest<PagedList<BookLookupDto>>;
}
