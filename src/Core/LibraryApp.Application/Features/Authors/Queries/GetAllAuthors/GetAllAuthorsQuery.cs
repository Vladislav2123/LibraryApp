using LibraryApp.Application.Pagination;
using MediatR;
using LibraryApp.Application.Features.Authors.Queries.Dto;

namespace LibraryApp.Application.Features.Authors.Queries.GetAllAuthors;

public record GetAllAuthorsQuery(
	string? SearchTerms,
	Page Page)
	: IRequest<PagedList<AuthorLookupDto>>;
