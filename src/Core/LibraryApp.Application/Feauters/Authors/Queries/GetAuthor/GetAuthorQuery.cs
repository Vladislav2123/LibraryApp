using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthor
{
	public record GetAuthorQuery(Guid AuthorId) 
		: IRequest<AuthorDto>;
}
