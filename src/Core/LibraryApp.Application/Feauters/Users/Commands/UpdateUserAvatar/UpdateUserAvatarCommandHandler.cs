using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LibraryApp.Application.Feauters.Users.Commands.UpdateUserAvatar
{
	public class UpdateUserAvatarCommandHandler : IRequestHandler<UpdateUserAvatarCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly FilePaths _filePaths;

		public UpdateUserAvatarCommandHandler(ILibraryDbContext dbContext, IOptions<FilePaths> filePathsOptions)
		{
			_dbContext = dbContext;
			_filePaths = filePathsOptions.Value;
		}

		public async Task<Unit> Handle(UpdateUserAvatarCommand command, CancellationToken cancellationToken)
		{
			var user = await _dbContext.Users
				.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

			if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

			if (string.IsNullOrEmpty(user.AvatarPath))
			{
				string avatarFileName = $"{Guid.NewGuid()}{Path.GetExtension(command.AvatarFile.FileName)}";
				user.AvatarPath = Path.Combine(_filePaths.AvatarsPath, avatarFileName);
			}
			else File.Delete(user.AvatarPath);

			using (var stream = new FileStream(user.AvatarPath, FileMode.Create))
			{
				await command.AvatarFile.CopyToAsync(stream, cancellationToken);
			}

			_dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
