using MediatR;
using LibraryApp.Application.Feauters.Books.Querries.Dto;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBook
{
    public record GetBookQuery(Guid Id) : IRequest<BookDto>;
}
