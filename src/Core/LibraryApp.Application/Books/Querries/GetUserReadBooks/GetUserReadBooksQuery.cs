using LibraryApp.Application.Books.Querries.Dto;
using MediatR;

namespace LibraryApp.Application.Books.Querries.GetUserReadBooks
{
    public record GetUserReadBooksQuery(Guid UserId) : IRequest<BooksListVm>;
}
