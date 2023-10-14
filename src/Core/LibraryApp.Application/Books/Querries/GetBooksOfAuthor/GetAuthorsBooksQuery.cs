using LibraryApp.Application.Books.Querries.Dto;
using MediatR;

namespace LibraryApp.Application.Books.Querries.GetBooksOfAuthor
{
	public record GetAuthorsBooksQuery(Guid AuthorId) : IRequest<BooksListVm>;
}
