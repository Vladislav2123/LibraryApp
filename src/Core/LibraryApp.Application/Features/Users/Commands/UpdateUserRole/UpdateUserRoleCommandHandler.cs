using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using MediatR;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Users.Commands.UpdateUserRole;
public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public UpdateUserRoleCommandHandler(
		ILibraryDbContext dbContext, 
		ICacheService cache, 
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<Unit> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
	{
		string cacheKey = $"{_cacheKeys.User}{command.UserId}";

		var user = await _cache
			.GetAsync(
				cacheKey,
				UserQuery,
				cancellationToken
			);

		user.Role = Enum.Parse<UserRole>(command.Role);
		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.SetAsync(cacheKey, user, cancellationToken);

		return Unit.Value;

		async Task<User> UserQuery()
		{
			var user = await _dbContext.Users
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

			if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

			return user;
		}
	}
}
