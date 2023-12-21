using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Users.Commands.Create;
using LibraryApp.Domain.Enteties;
using Moq;
using Moq.EntityFrameworkCore;

namespace LibraryApp.Tests.UserTests;

public class CreateUserTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();

	private readonly Mock<IPasswordProvider> _passwordProviderMock =
		new Mock<IPasswordProvider>();

	[Fact]
	public async Task Handle_ExpectedData_ReturnCreatedUserGuid()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>());

		_passwordProviderMock
			.Setup(x => x.HashPassword(It.IsAny<string>(), It.IsAny<string>()))
			.Returns("PasswordHash");

		var command = new CreateUserCommand(
			"name",
			"email@email.com",
			"SomePassword",
			DateOnly.Parse("2000-01-01"));

		var handler = new CreateUserCommandHandler(
			_dbContextMock.Object,
			_passwordProviderMock.Object);

		// Act
		Guid result = await handler.Handle(command, CancellationToken.None);

		// Assert
		_dbContextMock.Verify(x =>
			x.Users.AddAsync(It.IsAny<User>(), CancellationToken.None));
		
		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));

		result.Should().NotBeEmpty();
	}


	[Fact]
	public async Task Handle_AlredyUsingEmail_ThrowEmailAlreadyInUserException()
	{
		// Arrange
			var user = new User() { Email = "email@email.com" };

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>() { user });

		_passwordProviderMock
			.Setup(x => x.HashPassword(It.IsAny<string>(), It.IsAny<string>()))
			.Returns("PasswordHash");

		var command = new CreateUserCommand(
			"Name",
			user.Email,
			"SomePassword",
			DateOnly.Parse("2000-01-01"));

		var handler = new CreateUserCommandHandler(
			_dbContextMock.Object, 
			_passwordProviderMock.Object);
		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EmailAlreadyInUseException>();
	}
}
