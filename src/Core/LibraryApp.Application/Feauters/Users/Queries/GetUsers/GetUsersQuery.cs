using LibraryApp.Application.Common.Helpers;
using MediatR;
using LibraryApp.Application.Feauters.Users.Queries.Dto;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUsers
{
	public record GetUsersQuery(
		string? SearchTerms,
		string? SortColumn,
		string? SortOrder,
		int Page,
		int PageSize) : IRequest<PagedList<UserLookupDto>>;
}
