using LibraryApp.Application.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using LibraryApp.Domain.Models;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using Moq;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Users.Commands.UpdateUserAvatar;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Tests.UserTests;
public class UpdateUserAvatarTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();
	private readonly Mock<IOptions<FilePaths>> _filePathOptionsMock = new();
	private readonly Mock<IFormFile> _avatarFileMock = new();
	private readonly Mock<IFileWrapper> _fileWrapperMock = new();

	[Fact]
	public async Task Handle_CreateAvatar_ReturnUnit()
	{
		// Arrange
		var user = new User { Id = Guid.NewGuid() };

		var avatarFile = _avatarFileMock.Object;

		_dbContextMock
			.Setup(x => x.Users)
			.ReturnsDbSet(new User[] { user });

		_filePathOptionsMock
			.Setup(x => x.Value)
			.Returns(new FilePaths { AvatarsPath = "avatars" });

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
			.ReturnsDbSet(new User[0]);

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