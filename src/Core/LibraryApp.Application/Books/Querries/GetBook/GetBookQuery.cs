using LibraryApp.Application.Books.Querries.Dto;
using MediatR;

namespace LibraryApp.Application.Books.Querries.GetBook
{
    public record GetBookQuery(Guid Id) : IRequest<BookDto>;
}
