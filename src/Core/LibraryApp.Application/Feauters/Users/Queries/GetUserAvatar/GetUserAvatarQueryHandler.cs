using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUserAvatar;

public class GetUserAvatarQueryHandler : IRequestHandler<GetUserAvatarQuery, FileVm>
{
	private readonly ILibraryDbContext _libraryDbContext;
	private readonly IContentTypeProvider _contentTypeProvider;

	private const string FileType = "User avatar";

	public GetUserAvatarQueryHandler(ILibraryDbContext libraryDbContext, IContentTypeProvider contentTypeProvider)
	{
		_libraryDbContext = libraryDbContext;
		_contentTypeProvider = contentTypeProvider;
	}

	public async Task<FileVm> Handle(GetUserAvatarQuery request, CancellationToken cancellationToken)
	{
		var user = await _libraryDbContext.Users
			.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

		if (user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

		if (string.IsNullOrEmpty(user.AvatarPath) ||
			Path.Exists(user.AvatarPath) == false) 
			throw new FileNotFoundException(FileType);

		string contentType;
		if (_contentTypeProvider.TryGetContentType(user.AvatarPath, out contentType) == false)
			throw new ContentTypeNotFoundException(FileType);

		return new FileVm()
		{
			FileName = $"{user.Name}_avatar{Path.GetExtension(user.AvatarPath)}",
			ContentType = contentType,
			Bytes = await File.ReadAllBytesAsync(user.AvatarPath, cancellationToken)
		};
	}
}
