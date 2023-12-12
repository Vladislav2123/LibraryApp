using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;
using LibraryApp.Application.Abstractions;

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
				.FirstOrDefaultAsync(author => author.Id == command.AuthorId, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), command.AuthorId);

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
