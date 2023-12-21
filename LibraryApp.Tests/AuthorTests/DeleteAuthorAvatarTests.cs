using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Authors.Commands.DeleteAuthorAvatar;
using LibraryApp.Domain.Enteties;
using LibraryApp.Tests.Common;
using Moq;
using Moq.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.Tests.AuthorTests;
public class DeleteAuthorAvatarTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = 
		new Mock<ILibraryDbContext>();

	private readonly Mock<IFileWrapper> _fileWrapperMock =
		new Mock<IFileWrapper>();

	[Fact]
	public async Task Handler_ExpectedBehavior_ReturnUnit()
	{
		// Arrange
		string avatarPath = TestingHelper.GetTesingFile("avatar.jpeg");

		var author = new Author()
		{
			Id = Guid.NewGuid(),
			AvatarPath = avatarPath
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		var command = new DeleteAuthorAvatarCommand(author.Id);

		var handler = new DeleteAuthorAvatarCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(avatarPath));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));

		File.Delete(avatarPath);
	}

	[Fact]
	public async Task Handle_NonexistingAvatarFile_ThrowFileNotFoundException()
	{
		// Arrange
		var author = new Author() { Id = Guid.NewGuid() };

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		var command = new DeleteAuthorAvatarCommand(author.Id);

		var handler = new DeleteAuthorAvatarCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<FileNotFoundException>();
	}

	[Fact]
	public async Task Handler_NonexistingAuthor_ThrwoEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[0]);

		var command = new DeleteAuthorAvatarCommand(Guid.NewGuid());

		var handler = new DeleteAuthorAvatarCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
