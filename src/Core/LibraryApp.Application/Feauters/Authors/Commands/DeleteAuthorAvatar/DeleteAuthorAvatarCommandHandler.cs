using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.Application.Feauters.Authors.Commands.DeleteAuthorAvatar
{
	public class DeleteAuthorAvatarCommandHandler : IRequestHandler<DeleteAuthorAvatarCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public DeleteAuthorAvatarCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(DeleteAuthorAvatarCommand command, CancellationToken cancellationToken)
		{
			var author = await _dbContext.Authors
				.FirstOrDefaultAsync(author => author.Id == command.Id, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), command.Id);

			if (string.IsNullOrEmpty(author.AvatarPath) ||
				Path.Exists(author.AvatarPath) == false)
				throw new FileNotFoundException("Author avatar");

			File.Delete(author.AvatarPath);
			author.AvatarPath = string.Empty;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
