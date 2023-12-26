using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Feauters.Users.Commands.Update;
using LibraryApp.Domain.Enteties;
using MediatR;
using Moq;
using Moq.EntityFrameworkCore;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Tests.UserTests;
public class UpdateUserTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();

	[Fact]
	public async Task Handle_ExpectedBehavior_UpdateEntityAndReturnUnit()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Name = "Name",
			Email = "email@email.com",
			BirthDate = DateOnly.Parse("2000-01-01"),
		};

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new User[] { user });

		var command = new UpdateUserCommand(
			user.Id,
			"NewName",
			"newEmail@email.com",
			DateOnly.Parse("2001-02-02"));

		var handler = new UpdateUserCommandHandler(_dbContextMock.Object);

		// Act
		Unit result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(It.IsAny<CancellationToken>()));
	}


	[Fact]
	public async Task Handle_UnknownId_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new User[0]);

		var command = new UpdateUserCommand(
			Guid.NewGuid(),
			"Name",
			"email@email.com",
			DateOnly.Parse("2000-01-01"));

		var handler = new UpdateUserCommandHandler(_dbContextMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}


	[Fact]
	public async Task Handle_SameData_ThrowEntityHasNoChangesExceptions()
	{
		// Arrange
		User user = new User
		{
			Id = Guid.NewGuid(),
			Name = "Name",
			Email = "email@email.com",
			BirthDate = DateOnly.Parse("2000-01-01")
		};

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new User[] { user });

		var command = new UpdateUserCommand(
			user.Id,
			user.Name,
			user.Email,
			user.BirthDate);

		var handler = new UpdateUserCommandHandler(_dbContextMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityHasNoChangesException>();
	}

	[Fact]
	public async Task Handle_DataWithUsedEmail_ThrowEmailAlreadyInUseExeption()
	{
		// Arrange
		Guid updatingUserId = Guid.NewGuid();
		string usedEmail = "usedEmail@email.com";

		var users = new User[]
		{
			new User { Id = updatingUserId },
			new User { Email = usedEmail }
		};

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(users);

		var command = new UpdateUserCommand(
			updatingUserId,
			"Name",
			usedEmail,
			DateOnly.Parse("2000-01-01"));

		var handler = new UpdateUserCommandHandler(_dbContextMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EmailAlreadyInUseException>();
	}
}
