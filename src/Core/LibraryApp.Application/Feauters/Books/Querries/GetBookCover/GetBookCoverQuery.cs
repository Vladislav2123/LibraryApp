using LibraryApp.Domain.Models;
using MediatR;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBookCover
{
	public record GetBookCoverQuery(Guid Id) : IRequest<FileVm>;
}
