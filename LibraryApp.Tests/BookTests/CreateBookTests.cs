using FluentAssertions;
using iTextSharp.text;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Books.Commands.Create;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using LibraryApp.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using System.Security.Claims;
using Xunit;

namespace LibraryApp.Tests.BookTests;
public class CreateBookTests
{
	private Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();
	
	private Mock<IFileWrapper> _fileWrapperMock =
		new Mock<IFileWrapper>();

	private Mock<IHttpContextAccessor> _httpContextAccessorMock =
		new Mock<IHttpContextAccessor>();

	private Mock<IOptions<FilePaths>> _filePathOptionsMock =
		new Mock<IOptions<FilePaths>>();

	private Mock<IFormFile> _contentFileMock = 
		new Mock<IFormFile>();

	[Fact]
	public async Task Handle_ExpectedData_ReturnBookId()
	{
		// Arrange
		_httpContextAccessorMock
			.Setup(x => x.HttpContext)
			.Returns(TestingHelper.GetHttpContextWithActorClaim(
				Guid.NewGuid().ToString()));

		var author = new Author() { Id = Guid.NewGuid() };

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new List<Author>() { author });

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new List<Book>());

		_filePathOptionsMock
			.Setup(x => x.Value)
			.Returns(new FilePaths() { BooksPath = "books" });

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
			.ReturnsDbSet(new List<Author>());

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
			.ReturnsDbSet(new List<Author>() { author });

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new List<Book>() { book });

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
