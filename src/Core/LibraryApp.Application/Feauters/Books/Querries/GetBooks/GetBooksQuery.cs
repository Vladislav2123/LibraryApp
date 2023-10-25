using LibraryApp.Application.Common.Helpers;
using MediatR;
using LibraryApp.Application.Feauters.Books.Querries.Dto;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBooks
{
    public record GetBooksQuery(
        string? SearchTerms,
        Guid? AuthorId, 
        string? SortColumn, 
        string? SortOrder,
        int Page,
        int Limit) : IRequest<PagedList<BookLookupDto>>;
}
