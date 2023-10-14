using LibraryApp.Application.Books.Querries.Dto;
using LibraryApp.Application.Common.Helpers;
using MediatR;

namespace LibraryApp.Application.Books.Querries.GetBooksList
{
    public record GetBooksListQuery(
        string? SearchTerms,
        //Guid? AuthorId, 
        string? SortColumn, 
        string? SortOrder,
        int Page,
        int PageSize) : IRequest<PagedList<BookLookupDto>>;
}
