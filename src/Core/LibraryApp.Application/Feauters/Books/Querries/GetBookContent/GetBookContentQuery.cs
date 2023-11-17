using LibraryApp.Application.Feauters.Books.Querries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBookContent
{
	public record GetBookContentQuery(Guid Id) : IRequest<BookContentVm>;
}
