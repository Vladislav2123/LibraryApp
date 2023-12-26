using LibraryApp.Application.Feauters.Books.Commands.Create;
using LibraryApp.Application.Abstractions;
using Microsoft.Extensions.Options;
using LibraryApp.Domain.Enteties;
using Microsoft.AspNetCore.Http;
using LibraryApp.Domain.Models;
using LibraryApp.Tests.Common;
using FluentAssertions;
using Moq.EntityFrameworkCore;
using Moq;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Tests.BookTests;
public class CreateBookTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();
	private readonly Mock<IFileWrapper> _fileWrapperMock = new();
	private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
	private readonly Mock<IOptions<FilePaths>> _filePathOptionsMock = new();
	private readonly Mock<IFormFile> _contentFileMock = new();

	[Fact]
	public async Task Handle_ExpectedBehavior_ReturnBookId()
	{
		// Arrange
		_httpContextAccessorMock
			.Setup(x => x.HttpContext)
			.Returns(TestingHelper.GetHttpContextWithActorClaim(
				Guid.NewGuid().ToString()));

		var author = new Author { Id = Guid.NewGuid() };

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[0]);

		_filePathOptionsMock
			.Setup(x => x.Value)
			.Returns(new FilePaths { BooksPath = "books" });

		var command = new CreateBookCommand(
			author.Id,
			"Name",
			"Description",
			0001,
			_contentFileMock.Object);

		var handler = new CreateBookCommandHandler(
			_dbContextMock.Object,
			_httpContextAccessorMock.Object,
			_fileWrapperMock.Object,
			_filePathOptionsMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeEmpty();

		_fileWrapperMock.Verify(x => 
			x.SaveFileAsync(
				command.ContentFile, 
				It.IsAny<string>(),
				CancellationToken.None));

		_dbContextMock.Verify(x => 
			x.Books.AddAsync(It.IsAny<Book>(), CancellationToken.None));

		_dbContextMock.Verify(x => 
			x.SaveChangesAsync(CancellationToken.None));
	}


	[Fact]
	public async Task Handle_NonexistentAuhor_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[0]);

		var command = new CreateBookCommand(
			Guid.NewGuid(),
			"Name",
			"Description",
			0001,
			_contentFileMock.Object);

		var handler = new CreateBookCommandHandler(
			_dbContextMock.Object,
			_httpContextAccessorMock.Object,
			_fileWrapperMock.Object,
			_filePathOptionsMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}


	[Fact]
	public async Task Handle_AlreadyExistingBook_ThrowEntityAlreadyExistException()
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

		var command = new CreateBookCommand(
			author.Id,
			book.Name,
			book.Description,
			book.Year,
			_contentFileMock.Object);

		var handler = new CreateBookCommandHandler(
			_dbContextMock.Object,
			_httpContextAccessorMock.Object,
			_fileWrapperMock.Object,
			_filePathOptionsMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityAlreadyExistException>();
	}
}
