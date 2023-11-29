using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LibraryApp.Application.Feauters.Books.Commands.UpdateBookCover
{
	public class UpdateBookCoverCommandHandler : IRequestHandler<UpdateBookCoverCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly FilePaths _filePaths;

		public UpdateBookCoverCommandHandler(ILibraryDbContext dbContext, IOptions<FilePaths> filePathsOptions)
		{
			_dbContext = dbContext;
			_filePaths = filePathsOptions.Value;
		}

		public async Task<Unit> Handle(UpdateBookCoverCommand request, CancellationToken cancellationToken)
		{
			var book = await _dbContext.Books
				.FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

			if (string.IsNullOrEmpty(book.CoverPath))
			{
				string newCoverFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.CoverFile.FileName)}";
				book.CoverPath = Path.Combine(_filePaths.CoversPath, newCoverFileName);
			}
			else File.Delete(book.CoverPath);

			using (var stream = new FileStream(book.CoverPath, FileMode.Create))
			{
				await request.CoverFile.CopyToAsync(stream, cancellationToken);
			}

			_dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
