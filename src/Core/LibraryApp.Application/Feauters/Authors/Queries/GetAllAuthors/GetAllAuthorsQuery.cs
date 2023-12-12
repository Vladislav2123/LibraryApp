using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthors
{
    public record GetAllAuthorsQuery(
		string? SearchTerms,
		Page Page) 
		: IRequest<PagedList<AuthorLookupDto>>;
}
