using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Users.Commands.UpdateUserAvatar;

public class UpdateUserAvatarCommandHandler : IRequestHandler<UpdateUserAvatarCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly FilePaths _filePaths;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public UpdateUserAvatarCommandHandler(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper,
		IOptions<FilePaths> filePathsOptions,
		ICacheService cache,
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
		_filePaths = filePathsOptions.Value;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<Unit> Handle(UpdateUserAvatarCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.User}{command.UserId}";

		var user = await _cache
			.GetAsync(
				cacheKey,
				UserQuery,
				cancellationToken
			);

		if (string.IsNullOrEmpty(user.AvatarPath))
		{
			string avatarFileName =
				$"{Guid.NewGuid()}{Path.GetExtension(command.AvatarFile.FileName)}";
			user.AvatarPath = Path.Combine(_filePaths.AvatarsPath, avatarFileName);
			
			await _cache.SetAsync(cacheKey, user, cancellationToken);
		}
		else _fileWrapper.DeleteFile(user.AvatarPath);

		await _fileWrapper.SaveFileAsync(command.AvatarFile, user.AvatarPath, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

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
