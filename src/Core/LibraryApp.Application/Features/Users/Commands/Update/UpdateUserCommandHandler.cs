using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Users.Commands.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public UpdateUserCommandHandler(
		ILibraryDbContext dbContext, 
		ICacheService cache, 
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.User}{command.UserId}";

		var user = await _cache
			.GetAsync(
				cacheKey, 
				UserQuery, 
				cancellationToken
			);

		if (user.Name == command.Name &&
			user.Email == command.Email &&
			user.BirthDate == command.BirthDate)
		{
			throw new EntityHasNoChangesException(nameof(User), command.UserId);
		}

		if (await _dbContext.Users
			.AnyAsync(user =>
				user.Id != command.UserId &&
				user.Email == command.Email, cancellationToken))
		{
			throw new EmailAlreadyInUseException(command.Email);
		}

		user.Name = command.Name;
		user.Email = command.Email;
		user.BirthDate = command.BirthDate;

		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.SetAsync(cacheKey, user, cancellationToken);

		return Unit.Value;

		async Task<User> UserQuery()
		{
			User user = await _dbContext.Users
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

			if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

			return user;
		}
	}
}
