using MediatR;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Common.Helpers;

namespace LibraryApp.Application.Feauters.Books.Querries.GetUserReadBooks
{
    public record GetUserReadBooksQuery(
        Guid UserId, 
        int Page, 
        int PageSize) : IRequest<PagedList<BookLookupDto>>;
}
