using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using LibraryApp.Application.Pagination;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthors;

public record GetAllAuthorsQuery(
	string? SearchTerms,
	Page Page)
	: IRequest<PagedList<AuthorLookupDto>>;
