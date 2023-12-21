using FluentAssertions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Users.Commands.UpdateUserAvatar;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;

namespace LibraryApp.Tests.UserTests;
public class UpdateUserAvatarTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();

	private readonly Mock<IOptions<FilePaths>> _filePathOptionsMock =
		new Mock<IOptions<FilePaths>>();

	private readonly Mock<IFormFile> _avatarFileMock =
		new Mock<IFormFile>();

	private readonly Mock<IFileWrapper> _fileWrapperMock =
		new Mock<IFileWrapper>();

	[Fact]
	public async Task Handle_CreateAvatar_ReturnUnit()
	{
		// Arrange
		var user = new User() { Id = Guid.NewGuid() };

		var avatarFile = _avatarFileMock.Object;

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>() { user });

		_filePathOptionsMock
			.Setup(x => x.Value)
			.Returns(new FilePaths() { AvatarsPath = "avatars" });

		var command = new UpdateUserAvatarCommand(user.Id, avatarFile);

		var handler = new UpdateUserAvatarCommandHandler(
			_dbContextMock.Object,
			_fileWrapperMock.Object,
			_filePathOptionsMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_fileWrapperMock.Verify(x =>
			x.SaveFileAsync(avatarFile, It.IsAny<string>(), CancellationToken.None));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));
	}

	[Fact]
	public async Task Handle_NonexistentUser_ThrowEntityNotFoundExcepiton()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new List<User>());

		var command = new UpdateUserAvatarCommand(
			Guid.NewGuid(), 
			_avatarFileMock.Object);

		var handler = new UpdateUserAvatarCommandHandler(
			_dbContextMock.Object, 
			_fileWrapperMock.Object, 
			_filePathOptionsMock.Object);

		// Act
		var action = async () => 
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}