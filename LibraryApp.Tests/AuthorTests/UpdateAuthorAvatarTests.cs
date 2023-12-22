using LibraryApp.Application.Feauters.Authors.Commands.UpdateAuthorAvatar;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Abstractions;
using Microsoft.Extensions.Options;
using LibraryApp.Domain.Enteties;
using Microsoft.AspNetCore.Http;
using LibraryApp.Domain.Models;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using Moq;

namespace LibraryApp.Tests.AuthorTests;
public class UpdateAuthorAvatarTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();
	private readonly Mock<IFileWrapper> _fileWrapperMock = new();
	private readonly Mock<IOptions<FilePaths>> _filePathOptionsMock = new();
	private readonly Mock<IFormFile> _avatarFileMock = new();

	[Fact]
	public async Task Handle_CreateAvatar_ReturnUnit()
	{
		// Arrange
		var author = new Author
		{
			Id = Guid.NewGuid(),
			AvatarPath = "avatar"
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		var command = new UpdateAuthorAvatarCommand(
			author.Id,
			_avatarFileMock.Object);

		var handler = new UpdateAuthorAvatarCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object,
			_filePathOptionsMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(author.AvatarPath));

		_fileWrapperMock.Verify(x =>
			x.SaveFileAsync(
				_avatarFileMock.Object, 
				author.AvatarPath, 
				CancellationToken.None));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));
	}

	[Fact]
	public async Task Handle_NonexistentAuthor_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[0]);

		var command = new UpdateAuthorAvatarCommand(
			Guid.NewGuid(),
			_avatarFileMock.Object);

		var handler = new UpdateAuthorAvatarCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object,
			_filePathOptionsMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
