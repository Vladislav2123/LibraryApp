using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Users.Commands.DeleteUserAvatar;
using LibraryApp.Domain.Enteties;
using Moq;
using Moq.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.Tests.UserTests;
public class DeleteUserAvatarTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = 
		new Mock<ILibraryDbContext>();

	private readonly Mock<IFileWrapper> _fileWrapperMock = 
		new Mock<IFileWrapper>();

	[Fact]
	public async Task Handle_ExpectedData_ReturnUnit()
	{
		// Arrange
		string tempDirectory = Path.GetTempPath();
		string avatarPath = Path.Combine(tempDirectory, "avatar.jpeg");

		File.Create(avatarPath).Close();

		var user = new User()
		{
			Id = Guid.NewGuid(),
			AvatarPath = avatarPath
		};

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>() { user });

		var command = new DeleteUserAvatarCommand(user.Id);

		var handler = new DeleteUserAvatarCommandHandler(
			_dbContextMock.Object, 
			_fileWrapperMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x => x.DeleteFile(avatarPath));
		_dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None));

		File.Delete(avatarPath);
	}

	[Fact]
	public async Task Handle_NonexistentUser_ThowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>());

		var command = new DeleteUserAvatarCommand(Guid.NewGuid());

		var handler = new DeleteUserAvatarCommandHandler(
			_dbContextMock.Object, 
			_fileWrapperMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}

	[Fact]
	public async Task Handle_NonexistentAvatarFile_ThrowFileNotFoundException()
	{
		// Arrange
		var user = new User() { Id = Guid.NewGuid() };

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>() { user });

		var command = new DeleteUserAvatarCommand(user.Id);

		var handler = new DeleteUserAvatarCommandHandler(
			_dbContextMock.Object, 
			_fileWrapperMock.Object);

		// Act

		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<FileNotFoundException>();
	}
}
