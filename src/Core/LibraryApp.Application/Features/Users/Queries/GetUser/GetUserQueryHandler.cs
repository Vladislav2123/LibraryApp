using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Users.Queries.Dto;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDetailsDto>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public GetUserQueryHandler(
		ILibraryDbContext dbContext,
		IMapper mapper,
		ICacheService cacheService,
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_cache = cacheService;
		_cacheKeys = cacheKeys;
	}

	public async Task<UserDetailsDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
	{
		var user = await _cache.GetAndSetAsync(
			$"{_cacheKeys.User}{request.UserId}",
			UserQuery,
			cancellationToken
		);

		return _mapper.Map<UserDetailsDto>(user);

		async Task<User> UserQuery()
		{
			var user = await _dbContext.Users
				.AsNoTracking()
				.Select(user => new User
				{
					Id = user.Id,
					Name = user.Name,
					Email = user.Email,
					BirthDate = user.BirthDate,
					Role = user.Role
				})
				.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

			if (user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

			return user;
		}
	}
}
