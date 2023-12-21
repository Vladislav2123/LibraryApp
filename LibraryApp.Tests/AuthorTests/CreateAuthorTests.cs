using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Authors.Commands.Create;
using LibraryApp.Domain.Enteties;
using LibraryApp.Tests.Common;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.EntityFrameworkCore;

namespace LibraryApp.Tests.AuthorTests;
public class CreateAuthorTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();

	private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock =
		new Mock<IHttpContextAccessor>();

	[Fact]
	public async Task Handle_ExpectedData_ReturnCreatedAuthorGuid()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[0]);

		_httpContextAccessorMock
			.Setup(x => x.HttpContext)
			.Returns(TestingHelper.GetHttpContextWithActorClaim(
				Guid.NewGuid().ToString()));

		var command = new CreateAuthorCommand(
			"Name",
			DateOnly.Parse("0001-01-01"));

		var handler = new CreateAuthorCommandHandler(
			_dbContextMock.Object,
			_httpContextAccessorMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeEmpty();

		_dbContextMock.Verify(x =>
			x.Authors.AddAsync(It.IsAny<Author>(), CancellationToken.None));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));
	}


	[Fact]
	public async Task Handle_AlreadyExistingAuthor_ThrowEntityAlreadyExistException()
	{
		// Arrange
		var author = new Author()
		{
			Id = Guid.NewGuid(),
			Name = "Name",
			BirthDate = DateOnly.Parse("0001-01-01")
		};

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new Author[] { author });

		var command = new CreateAuthorCommand(
			author.Name,
			author.BirthDate);

		var handler = new CreateAuthorCommandHandler(
			_dbContextMock.Object,
			_httpContextAccessorMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityAlreadyExistException>();
	}
}
