using LibraryApp.Application.Features.Users.Queries.Dto;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Pagination;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Entities;
using System.Linq.Expressions;
using AutoMapper;
using MediatR;

namespace LibraryApp.Application.Features.Users.Queries.GetAllUsers;

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
			.AsNoTracking()
			.Include(user => user.ReadBooks);

		// Filtering
		if (string.IsNullOrWhiteSpace(request.SearchTerms) == false)
		{
			usersQuery = usersQuery
				.Where(user => user.Name.Contains(request.SearchTerms));
		}

		// Sorting
		var sortingColumnPropertyExpression = GetSortingColumnProperty(request);
		if (request.SortOrder?.ToLower() == "asc") usersQuery.OrderByDescending(sortingColumnPropertyExpression);
		else usersQuery.OrderBy(sortingColumnPropertyExpression);

		// Response
		var totalAmount = usersQuery.Count();
		var usersList = usersQuery
		.Skip((request.Page.number - 1) * request.Page.size)
		.Take(request.Page.size)
		.ToList();

		var mappedUsers = _mapper.Map<List<UserLookupDto>>(usersList);
		return new PagedList<UserLookupDto>(mappedUsers, totalAmount, request.Page);
	}

	private Expression<Func<User, object>> GetSortingColumnProperty(GetAllUsersQuery request) =>
		request.SortColumn?.ToLower() switch
		{
			"name" => user => user.Name,
			"age" => user => user.BirthDate,
			_ => user => user.ReadBooks.Count
		};
}
