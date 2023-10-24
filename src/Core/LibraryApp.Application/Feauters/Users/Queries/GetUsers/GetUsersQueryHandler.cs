using AutoMapper;
using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryApp.Application.Feauters.Users.Queries.Dto;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUsers
{
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedList<UserLookupDto>>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetUsersQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

		public async Task<PagedList<UserLookupDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			IQueryable<User> usersQuery = _dbContext.Users.Include(user => user.ReadedBooks);

			if(string.IsNullOrWhiteSpace(request.SearchTerms) == false)
			{
				usersQuery = usersQuery.Where(user => user.Name.Contains(request.SearchTerms));
			}

			var sortingColumnPropertyExpression = GetSortingColumnProperty(request);

			if (request.SortOrder?.ToLower() == "asc")
			{
				usersQuery.OrderByDescending(sortingColumnPropertyExpression);
			}
			else usersQuery.OrderBy(sortingColumnPropertyExpression);

			var usersLookupsQuery = _mapper.Map<IQueryable<UserLookupDto>>(usersQuery);
			return await PagedList<UserLookupDto>.CreateAsync(usersLookupsQuery, request.Page, request.PageSize, cancellationToken);
		}

		private Expression<Func<User, object>> GetSortingColumnProperty(GetUsersQuery request)
		{
			Expression<Func<User, object>> expression = request.SortColumn?.ToLower() switch
			{
				"name" => user => user.Name,
				"age" => user => user.BirthDate,
				_ => user => user.ReadedBooks.Count
			};

			return expression;
		}
	}
}
