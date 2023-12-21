using FluentAssertions;
using LibraryApp.Application.Common.FileWrappers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace LibraryApp.Tests.FileWrapperTests;
public class SaveFileTests
{
	private readonly FileWrapper _fileWrapper = 
		new FileWrapper();

	private readonly Mock<IFormFile> _fileMock =
		new Mock<IFormFile>();

	[Fact]
	public async Task SaveFileAsync_ExpectedData_Success()
	{
		// Arrange
		string directory = Path.GetTempPath();
		string path = Path.Combine(directory, "file.txt");

		// Act
		await _fileWrapper.SaveFileAsync(
			_fileMock.Object, 
			path, 
			CancellationToken.None);

		// Assert
		Path.Exists(path).Should().BeTrue();

		File.Delete(path);
	}
}
