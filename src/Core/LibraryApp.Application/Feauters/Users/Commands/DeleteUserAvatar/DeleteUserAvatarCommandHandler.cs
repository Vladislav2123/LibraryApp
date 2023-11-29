﻿using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.Application.Feauters.Users.Commands.DeleteUserAvatar
{
	public class DeleteUserAvatarCommandHandler : IRequestHandler<DeleteUserAvatarCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public DeleteUserAvatarCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(DeleteUserAvatarCommand command, CancellationToken cancellationToken)
		{
			var user = await _dbContext.Users
				.FirstOrDefaultAsync(user => user.Id == command.Id, cancellationToken);

			if (user == null) throw new EntityNotFoundException(nameof(User), command.Id);

			if (string.IsNullOrEmpty(user.AvatarPath) ||
				Path.Exists(user.AvatarPath) == false)
				throw new FileNotFoundException("User avatar");

			File.Delete(user.AvatarPath);
			user.AvatarPath = string.Empty;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
