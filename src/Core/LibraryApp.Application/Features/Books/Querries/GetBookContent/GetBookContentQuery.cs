using LibraryApp.Domain.Models;
using MediatR;

namespace LibraryApp.Application.Features.Books.Querries.GetBookContent;

public record GetBookContentQuery(Guid BookId) 
	: IRequest<FileVm>;
