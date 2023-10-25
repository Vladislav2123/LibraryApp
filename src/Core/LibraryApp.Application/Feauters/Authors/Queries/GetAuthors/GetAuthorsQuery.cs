using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthors
{
	public record GetAuthorsQuery(string? SearchTerms,
		int Page,
		int Limit) : IRequest<PagedList<AuthorLookupDto>>;
}
