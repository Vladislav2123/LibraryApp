using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Books.Commands.DeleteBookCover;
using LibraryApp.Domain.Enteties;
using LibraryApp.Tests.Common;
using Moq.EntityFrameworkCore;
using Moq;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.Tests.BookTests;
public class DeleteBookCoverTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();

	private readonly Mock<IFileWrapper> _fileWrapperMock =
		new Mock<IFileWrapper>();

	[Fact]
	public async Task Handle_ExpectedBahavior_ReturnUnit()
	{
		string coverPath = TestingHelper.CreateTesingFile("cover.jpeg");

		// Arrange
		var book = new Book()
		{
			Id = Guid.NewGuid(),
			CoverPath = coverPath
		};

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new DeleteBookCoverCommand(book.Id);

		var handler = new DeleteBookCoverCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(coverPath));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));

		File.Delete(coverPath);
	}

	[Fact]
	public async Task Handle_NonexitingBook_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[0]);

		var command = new DeleteBookCoverCommand(Guid.NewGuid());
		
		var handler = new DeleteBookCoverCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}

	[Fact]
	public async Task Handle_NonexistentCoverFile_ThrowFileNotFoundExeption()
	{
		// Arrange
		var book = new Book() { Id = Guid.NewGuid() }; 
	
		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new DeleteBookCoverCommand(book.Id);

		var handler = new DeleteBookCoverCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<FileNotFoundException>();
	}
}
