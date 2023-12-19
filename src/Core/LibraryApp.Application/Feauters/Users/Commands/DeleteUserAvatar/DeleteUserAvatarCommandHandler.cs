using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Users.Commands.DeleteUserAvatar;

public class DeleteUserAvatarCommandHandler : IRequestHandler<DeleteUserAvatarCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;

	public DeleteUserAvatarCommandHandler(ILibraryDbContext dbContext, IFileWrapper fileWrapper)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
	}

	public async Task<Unit> Handle(DeleteUserAvatarCommand command, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

		if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

		if (string.IsNullOrEmpty(user.AvatarPath) ||
			Path.Exists(user.AvatarPath) == false)
			throw new FileNotFoundException("User avatar");

		_fileWrapper.DeleteFile(user.AvatarPath);
		user.AvatarPath = string.Empty;

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
