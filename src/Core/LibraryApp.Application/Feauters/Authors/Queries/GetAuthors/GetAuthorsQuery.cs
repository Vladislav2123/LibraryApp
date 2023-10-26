using LibraryApp.Application.Common.Helpers.Pagination;
using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthors
{
    public record GetAuthorsQuery(
		string? SearchTerms,
		Page Page) : IRequest<PagedList<AuthorLookupDto>>;
}
