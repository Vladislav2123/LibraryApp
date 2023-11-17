using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBookContent
{
	public class GetBookContentQueryHandler : IRequestHandler<GetBookContentQuery, BookContentVm>
	{
		private readonly ILibraryDbContext _dbContext;

		public GetBookContentQueryHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<BookContentVm> Handle(GetBookContentQuery request, CancellationToken cancellationToken)
		{
			var book = await _dbContext.Books
				.FirstOrDefaultAsync(book => book.Id == request.Id, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), request.Id);

			return new BookContentVm
			{
				FileName = $"{book.Name}.pdf",
				Bytes = await File.ReadAllBytesAsync(book.ContentPath, cancellationToken)
			};
		}
	}
}
