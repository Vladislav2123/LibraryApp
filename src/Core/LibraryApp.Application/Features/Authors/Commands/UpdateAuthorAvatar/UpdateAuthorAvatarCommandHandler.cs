using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Authors.Commands.UpdateAuthorAvatar;

public class UpdateAuthorAvatarCommandHandler : IRequestHandler<UpdateAuthorAvatarCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly FilePaths _filePaths;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public UpdateAuthorAvatarCommandHandler(
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

	public async Task<Unit> Handle(UpdateAuthorAvatarCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Author}{command.AuthorId}";

		var author = await _cache
			.GetAsync(
				cacheKey,
				AuthorQuery,
				cancellationToken
			);

		if (string.IsNullOrEmpty(author.AvatarPath))
		{
			string avatarFileName = $"{Guid.NewGuid()}{Path.GetExtension(command.AvatarFile.FileName)}";
			author.AvatarPath = Path.Combine(_filePaths.AvatarsPath, avatarFileName);

			await _cache.SetAsync(cacheKey, author, cancellationToken);
		}
		else _fileWrapper.DeleteFile(author.AvatarPath);

		await _fileWrapper.SaveFileAsync(command.AvatarFile, author.AvatarPath, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;

		async Task<Author> AuthorQuery()
		{
			var author = await _dbContext.Authors
			.FirstOrDefaultAsync(author => author.Id == command.AuthorId, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), command.AuthorId);

			return author;
		}
	}
}
