using AutoMapper;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryApp.Application.Feauters.Users.Queries.Dto;
using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserLookupDto>>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllUsersQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PagedList<UserLookupDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		IQueryable<User> usersQuery = _dbContext.Users
			.Include(user => user.ReadBooks);

		if (string.IsNullOrWhiteSpace(request.SearchTerms) == false)
		{
			usersQuery = usersQuery
				.Where(user => user.Name.Contains(request.SearchTerms));
		}

		var sortingColumnPropertyExpression = GetSortingColumnProperty(request);

		if (request.SortOrder?.ToLower() == "asc")
		{
			usersQuery.OrderByDescending(sortingColumnPropertyExpression);
		}
		else usersQuery.OrderBy(sortingColumnPropertyExpression);

		var usersLookups = _mapper.Map<List<UserLookupDto>>(await usersQuery.ToListAsync(cancellationToken));
		return PagedList<UserLookupDto>.Create(usersLookups, request.Page);
	}

	private Expression<Func<User, object>> GetSortingColumnProperty(GetAllUsersQuery request) =>
		request.SortColumn?.ToLower() switch
		{
			"name" => user => user.Name,
			"age" => user => user.BirthDate,
			_ => user => user.ReadBooks.Count
		};
}
