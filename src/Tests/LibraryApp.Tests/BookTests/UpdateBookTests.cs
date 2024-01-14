using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using Moq;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Books.Commands.Update;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Tests.BookTests;
public class UpdateBookTests
{
	private Mock<ILibraryDbContext> _dbContextMock = new();
	private Mock<IFileWrapper> _fileWrapperMock = new();
	private Mock<IFormFile> _contentFileMock = new();

	[Fact]
	public async Task Handle_ExcpectedBehaviorWithoutContent_ReturnUnit()
	{
		// Arrange
		var authors = new Author[]
		{
			new Author { Id = Guid.NewGuid() },
			new Author { Id = Guid.NewGuid() },
		};

		var book = new Book
		{
			Id = Guid.NewGuid(),
			AuthorId = authors[0].Id,
			Name = "Name",
			Description = "Description",
			Year = 0001
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(authors);

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new UpdateBookCommand(
			book.Id,
			authors[1].Id,
			"NewName",
			"NewDescription",
			0002,
			null);

		var handler = new UpdateBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		var updatedUser = _dbContextMock.Object.Books.FirstOrDefault();

		updatedUser.Name.Should().Be(command.Name);
		updatedUser.Description.Should().Be(command.Description);
		updatedUser.Year.Should().Be(command.Year);

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));

		_fileWrapperMock.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Handle_OnlyContentFileUpdate_ReturnUnit()
	{
		// Arrange
		var author = new Author { Id = Guid.NewGuid() };

		var book = new Book
		{
			Id = Guid.NewGuid(),
			AuthorId = author.Id,
			Name = "Name",
			Description = "Description",
			Year = 0001,
			ContentPath = "books/content"
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new UpdateBookCommand(
			book.Id,
			book.AuthorId,
			book.Name,
			book.Description,
			book.Year,
			_contentFileMock.Object);

		var handler = new UpdateBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.DeleteFile(book.ContentPath));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));

		_fileWrapperMock.Verify(x =>
			x.SaveFileAsync(_contentFileMock.Object, book.ContentPath, CancellationToken.None));
	}

	[Fact]
	public async Task Handle_NonexistentBook_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[0]);

		var command = new UpdateBookCommand(
			Guid.NewGuid(),
			Guid.NewGuid(),
			"Name",
			"Description",
			0001,
			null);

		var handler = new UpdateBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}

	[Fact]
	public async Task Handle_NonexistentAuthor_ThrowEntityNotFoundException()
	{
		// Arrange
		var author = new Author { Id = Guid.NewGuid() };

		var book = new Book
		{
			Id = Guid.NewGuid(),
			AuthorId = author.Id,
			Name = "Name",
			Description = "Description",
			Year = 0001
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new UpdateBookCommand(
			book.Id,
			Guid.NewGuid(),
			"NewName",
			"NewDescription",
			0002,
			null);

		var handler = new UpdateBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}

	[Fact]
	public async Task Handle_NoChanges_ThrowEntityHasNoChangesException()
	{
		// Arrange
		var author = new Author { Id = Guid.NewGuid() };

		var book = new Book
		{
			Id = Guid.NewGuid(),
			AuthorId = author.Id,
			Name = "Name",
			Description = "Description",
			Year = 0001,
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var command = new UpdateBookCommand(
			book.Id,
			book.AuthorId,
			book.Name,
			book.Description,
			book.Year,
			null);

		var handler = new UpdateBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityHasNoChangesException>();
	}

	[Fact]
	public async Task Handle_AlreadyUsingData_ThrowEntityAlreadyExistException()
	{
		// Arrange
		var author = new Author { Id = Guid.NewGuid() };

		var books = new Book[]
		{
			new Book
			{
				Id = Guid.NewGuid(),
				AuthorId = author.Id,
				Name = "Name",
				Description = "Description",
				Year = 0001
			},
			new Book
			{
				Id = Guid.NewGuid(),
				AuthorId = author.Id,
				Name = "DestName",
				Description = "DestDescription",
				Year = 0002
			}
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(books);

		var command = new UpdateBookCommand(
			books[0].Id,
			author.Id,
			books[1].Name,
			books[1].Description,
			books[1].Year,
			null);

		var handler = new UpdateBookCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityAlreadyExistException>();
	}
}
