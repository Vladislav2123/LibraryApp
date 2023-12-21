using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Users.Commands.Delete;
using LibraryApp.Domain.Enteties;
using MediatR;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace LibraryApp.Tests.UserTests;
public class DeleteUserTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock
		= new Mock<ILibraryDbContext>();

	private readonly Mock<IFileWrapper> _fileWrapperMock
		= new Mock<IFileWrapper>();


	[Fact]
	public async Task Handle_FullExpectedData_ReturnUnit()
	{
		// Arrange
		var user = new User()
		{
			Id = Guid.NewGuid(),
			AvatarPath = "avatarPath",
			CreatedAuthors = new List<Author>()
		};

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User> { user });

		_dbContextMock
			.Setup(x => x.Authors)
			.ReturnsDbSet(new List<Author>());

		var mockObj = _dbContextMock.Object;

		var command = new DeleteUserCommand(user.Id);

		var handler = new DeleteUserCommandHandler(
			mockObj,
			_fileWrapperMock.Object);

		// Act
		Unit result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_dbContextMock.Verify(x => 
			x.Authors.RemoveRange(user.CreatedAuthors));
		_fileWrapperMock.Verify(x => 
			x.DeleteFile(user.AvatarPath));
		_dbContextMock.Verify(x => 
			x.SaveChangesAsync(CancellationToken.None));
	}


	[Fact]
	public async Task Handle_NonexistentUser_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>());

		var command = new DeleteUserCommand(Guid.NewGuid());

		var handler = new DeleteUserCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
