using MediatR;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Users.Queries.Dto;

namespace LibraryApp.Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(
	string? SearchTerms,
	string? SortColumn,
	string? SortOrder,
	Page Page)
	: IRequest<PagedList<UserLookupDto>>;
