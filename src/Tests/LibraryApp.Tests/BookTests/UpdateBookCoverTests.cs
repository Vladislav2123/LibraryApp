using LibraryApp.Application.Feauters.Books.Commands.UpdateBookCover;
using LibraryApp.Application.Abstractions;
using Microsoft.Extensions.Options;
using LibraryApp.Domain.Enteties;
using Microsoft.AspNetCore.Http;
using LibraryApp.Domain.Models;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using Moq;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Tests.BookTests;
public class UpdateBookCoverTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();
	private readonly Mock<IFileWrapper> _fileWrapperMock = new();
	private readonly Mock<IOptions<FilePaths>> _filePathOptionsMock = new();
	private readonly Mock<IFormFile> _coverFileMock = new();

	[Fact]
	public async Task Handle_CreateCover_ReturnUnit()
	{
		// Arrange
		var book = new Book
		{
			Id = Guid.NewGuid(),
			ContentPath = "content",
			CoverPath = "cover",
		};

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new UpdateBookCoverCommand(
			book.Id,
			_coverFileMock.Object);

		var handler = new UpdateBookCoverCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object,
			_filePathOptionsMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(book.CoverPath));

		_fileWrapperMock.Verify(x =>
			x.SaveFileAsync(
				_coverFileMock.Object, 
				It.IsAny<string>(), 
				CancellationToken.None));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));
	}

	[Fact]
	public async Task Handle_NonexistentBook_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[0]);

		var command = new UpdateBookCoverCommand(
			Guid.NewGuid(),
			_coverFileMock.Object);

		var handler = new UpdateBookCoverCommandHandler(
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
