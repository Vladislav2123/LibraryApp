using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using FileNotFoundException = LibraryApp.Domain.Exceptions.FileNotFoundException;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Users.Commands.DeleteUserAvatar;

public class DeleteUserAvatarCommandHandler : IRequestHandler<DeleteUserAvatarCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public DeleteUserAvatarCommandHandler(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper,
		ICacheService cache,
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<Unit> Handle(DeleteUserAvatarCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.User}{command.UserId}";

		var user = await _cache
			.GetAsync(
				cacheKey,
				UserQuery,
				cancellationToken
			);

		if (string.IsNullOrEmpty(user.AvatarPath) ||
			Path.Exists(user.AvatarPath) == false)
			throw new FileNotFoundException("User avatar");

		_fileWrapper.DeleteFile(user.AvatarPath);
		user.AvatarPath = string.Empty;

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
