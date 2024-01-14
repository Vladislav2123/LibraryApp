using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Feauters.Authors.Commands.Delete;
using LibraryApp.Domain.Enteties;
using Moq.EntityFrameworkCore;
using Moq;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Authors.Commands.Delete;

namespace LibraryApp.Tests.AuthorTests;
public class DeleteAuthorTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();
	private readonly Mock<IFileWrapper> _fileWrapperMock = new();

	[Fact]
	public async Task Handle_ExpectedBehavior_ReturnUnit()
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

		var command = new DeleteAuthorCommand(author.Id);

		var handler = new DeleteAuthorCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(author.AvatarPath));

		_dbContextMock.Verify(x =>
			x.Authors.Remove(It.IsAny<Author>()));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));
	}

	[Fact]
	public async Task Handle_NonexistingAuthor_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[0]);

		var command = new DeleteAuthorCommand(Guid.NewGuid());

		var handler = new DeleteAuthorCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
