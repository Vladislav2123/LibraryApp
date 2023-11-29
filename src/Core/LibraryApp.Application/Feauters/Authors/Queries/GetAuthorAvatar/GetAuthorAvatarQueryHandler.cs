﻿using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthorAvatar
{
	public class GetAuthorAvatarQueryHandler : IRequestHandler<GetAuthorAvatarQuery, FileVm>
	{
		private readonly ILibraryDbContext _libraryDbContext;
		private readonly IContentTypeProvider _contentTypeProvider;

		public GetAuthorAvatarQueryHandler(ILibraryDbContext libraryDbContext, IContentTypeProvider contentTypeProvider)
		{
			_libraryDbContext = libraryDbContext;
			_contentTypeProvider = contentTypeProvider;
		}

		public async Task<FileVm> Handle(GetAuthorAvatarQuery request, CancellationToken cancellationToken)
		{
			var author = await _libraryDbContext.Authors
				.FirstOrDefaultAsync(author => author.Id == request.Id, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), request.Id);

			if (string.IsNullOrEmpty(author.AvatarPath) ||
				Path.Exists(author.AvatarPath) == false)
				throw new FileNotFoundException("Author avatar");

			string contentType;
			if (_contentTypeProvider.TryGetContentType(author.AvatarPath, out contentType) == false)
				throw new ContentTypeNotFoundException("Author avatar");

			return new FileVm()
			{
				FileName = $"{author.Name}_avatar{Path.GetExtension(author.AvatarPath)}",
				ContentType = contentType,
				Bytes = await File.ReadAllBytesAsync(author.AvatarPath, cancellationToken)
			};
		}
	}
}