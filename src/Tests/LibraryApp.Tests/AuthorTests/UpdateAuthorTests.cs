using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Feauters.Authors.Commands.Update;
using LibraryApp.Domain.Enteties;
using Moq.EntityFrameworkCore;
using Moq;
using System.Linq;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Tests.AuthorTests;
public class UpdateAuthorTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();

	[Fact]
	public async Task Handle_ExpectedBehavior_ReturnUnit()
	{
		// Arrange
		var author = new Author
		{
			Id = Guid.NewGuid(),
			Name = "Name",
			BirthDate = DateOnly.Parse("0001-01-01")
		};
		
		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		var command = new UpdateAuthorCommand(
			author.Id,
			"NewName",
			DateOnly.Parse("0002-02-02"));

		var handler = new UpdateAuthorCommandHandler(_dbContextMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		var updatedAuthor = _dbContextMock.Object.Authors.FirstOrDefault();

		updatedAuthor.Name.Should().Be(command.Name);
		updatedAuthor.BirthDate.Should().Be(command.BirthDate);

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));
	}

	[Fact]
	public async Task Handle_AlreadyUsingData_ThrowEntityAlreadyExistException()
	{
		// Arrange
		var authors = new Author[]
		{
			new Author
			{
				Id = Guid.NewGuid(),
				Name = "Name",
				BirthDate = DateOnly.Parse("0001-01-01")
			},
			new Author
			{
				Id = Guid.NewGuid(),
				Name = "DestName",
				BirthDate = DateOnly.Parse("0002-02-02")
			}
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(authors);

		var command = new UpdateAuthorCommand(
			authors[0].Id,
			authors[1].Name,
			authors[1].BirthDate);

		var handler = new UpdateAuthorCommandHandler(_dbContextMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityAlreadyExistException>();
	}

	[Fact]
	public async Task Handle_NoChanges_ThrowEntityHasNoChangesException()
	{
		// Arrange
		var author = new Author
		{
			Id = Guid.NewGuid(),
			Name = "Name",
			BirthDate = DateOnly.Parse("0001-01-01"),
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		var command = new UpdateAuthorCommand(
			author.Id,
			author.Name,
			author.BirthDate);

		var handler = new UpdateAuthorCommandHandler(_dbContextMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityHasNoChangesException>();
	}


	[Fact]
	public async Task Handle_NonexistentAuthor_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[0]);

		var command = new UpdateAuthorCommand(
			Guid.NewGuid(),
			"Name",
			DateOnly.Parse("0001-01-01"));

		var handler = new UpdateAuthorCommandHandler(_dbContextMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
