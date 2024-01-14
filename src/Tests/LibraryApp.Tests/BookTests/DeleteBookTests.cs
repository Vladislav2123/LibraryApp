using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Feauters.Books.Commands.Delete;
using LibraryApp.Domain.Enteties;
using MediatR;
using Moq;
using Moq.EntityFrameworkCore;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Books.Commands.Delete;

namespace LibraryApp.Tests.BookTests;
public class DeleteBookTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new ();
	private readonly Mock<IFileWrapper> _fileWrapperMock = new ();

	[Fact]
	public async Task Handle_ExpectedBehavior_ReturnUnit()
	{
		// Arrange
		var book = new Book
		{
			Id = Guid.NewGuid(),
			ContentPath = "content",
			CoverPath = "cover"
		};

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new DeleteBookCommand(book.Id);

		var handler = new DeleteBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		Unit result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(book.ContentPath));

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(book.CoverPath));

		_dbContextMock.Verify(x =>
			x.Books.Remove(It.IsAny<Book>()));

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

		var command = new DeleteBookCommand(Guid.NewGuid());

		var handler = new DeleteBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
