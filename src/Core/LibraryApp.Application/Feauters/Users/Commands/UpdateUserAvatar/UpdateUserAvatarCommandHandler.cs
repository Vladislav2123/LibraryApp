using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Feauters.Users.Commands.UpdateUserAvatar;

public class UpdateUserAvatarCommandHandler : IRequestHandler<UpdateUserAvatarCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly FilePaths _filePaths;

	public UpdateUserAvatarCommandHandler(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper,
		IOptions<FilePaths> filePathsOptions)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
		_filePaths = filePathsOptions.Value;
	}

	public async Task<Unit> Handle(UpdateUserAvatarCommand command, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

		if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

		if (string.IsNullOrEmpty(user.AvatarPath))
		{
			string avatarFileName = 
				$"{Guid.NewGuid()}{Path.GetExtension(command.AvatarFile.FileName)}";
			user.AvatarPath = Path.Combine(_filePaths.AvatarsPath, avatarFileName);
		}
		else _fileWrapper.DeleteFile(user.AvatarPath);

		await _fileWrapper.SaveFileAsync(command.AvatarFile, user.AvatarPath, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
