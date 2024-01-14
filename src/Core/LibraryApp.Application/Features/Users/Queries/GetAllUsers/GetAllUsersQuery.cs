using MediatR;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Users.Queries.Dto;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUsers;

public record GetAllUsersQuery(
	string? SearchTerms,
	string? SortColumn,
	string? SortOrder,
	Page Page)
	: IRequest<PagedList<UserLookupDto>>;
