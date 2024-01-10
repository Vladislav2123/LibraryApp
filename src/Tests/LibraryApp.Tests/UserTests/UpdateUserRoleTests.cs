using LibraryApp.Application.Feauters.Users.Commands.UpdateUserRole;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Enteties;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using Moq;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Tests.UserTests;
public class UpdateUserRoleTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();

	[Fact]
	public async Task Handle_UpdateRole_ReturnUnit()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Role = UserRole.Default
		};

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new User[] { user });

		var command = new UpdateUserRoleCommand(user.Id, "Admin");
		var handler = new UpdateUserRoleCommandHandler(_dbContextMock.Object);
		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		user.Role.Should().Be(UserRole.Admin);
		_dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None));
	}

	[Fact]
	public async Task Handle_NonexistentUser_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock.Setup(x => x.Users)
			.ReturnsDbSet(new User[0]);

		var command = new UpdateUserRoleCommand(Guid.NewGuid(), "Admin");
		var handler = new UpdateUserRoleCommandHandler(_dbContextMock.Object);

		// Act
		var action = async () => await handler.Handle(command, CancellationToken.None);


		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
