using MediatR;
using LibraryApp.Application.Feauters.Users.Queries.Dto;
using LibraryApp.Application.Pagination;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUsers;

public record GetAllUsersQuery(
	string? SearchTerms,
	string? SortColumn,
	string? SortOrder,
	Page Page)
	: IRequest<PagedList<UserLookupDto>>;
