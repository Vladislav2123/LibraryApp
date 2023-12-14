using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Authors.Commands.UpdateAuthorAvatar;

public class UpdateAuthorAvatarCommandHandler : IRequestHandler<UpdateAuthorAvatarCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly FilePaths _filePaths;

	public UpdateAuthorAvatarCommandHandler(ILibraryDbContext dbContext, IOptions<FilePaths> filePathsOptions)
	{
		_dbContext = dbContext;
		_filePaths = filePathsOptions.Value;
	}

	public async Task<Unit> Handle(UpdateAuthorAvatarCommand command, CancellationToken cancellationToken)
	{
		var author = await _dbContext.Authors
			.FirstOrDefaultAsync(author => author.Id == command.AuthorId, cancellationToken);

		if (author == null) throw new EntityNotFoundException(nameof(Author), command.AuthorId);

		if (string.IsNullOrEmpty(author.AvatarPath))
		{
			string avatarFileName = $"{Guid.NewGuid()}{Path.GetExtension(command.AvatarFile.FileName)}";
			author.AvatarPath = Path.Combine(_filePaths.AvatarsPath, avatarFileName);
		}
		else File.Delete(author.AvatarPath);

		using (var stream = new FileStream(author.AvatarPath, FileMode.Create))
		{
			await command.AvatarFile.CopyToAsync(stream, cancellationToken);
		}

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
