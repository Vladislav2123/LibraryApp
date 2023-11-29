﻿using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBookContent
{
	public class GetBookContentQueryHandler : IRequestHandler<GetBookContentQuery, FileVm>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IContentTypeProvider _contentTypeProvider;

		public GetBookContentQueryHandler(ILibraryDbContext dbContext, IContentTypeProvider contentTypeProvider)
		{
			_dbContext = dbContext;
			_contentTypeProvider = contentTypeProvider;
		}

		public async Task<FileVm> Handle(GetBookContentQuery request, CancellationToken cancellationToken)
		{
			var book = await _dbContext.Books
				.FirstOrDefaultAsync(book => book.Id == request.Id, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), request.Id);

			string contentType;
			if (_contentTypeProvider.TryGetContentType(book.ContentPath, out contentType) == false)
				throw new ContentTypeNotFoundException($"Book Content");

			return new FileVm
			{
				FileName = $"{book.Name}{Path.GetExtension(book.ContentPath)}",
				ContentType = contentType,
				Bytes = await File.ReadAllBytesAsync(book.ContentPath, cancellationToken)
			};
		}
	}
}