using FluentAssertions;
using LibraryApp.Application.Common.FileWrappers;

namespace LibraryApp.Tests.FileWrapperTests;
public class DeleteFileTests
{
	[Fact]
	public async Task DeleteFile_ExpectedData_Success()
	{
		// Arrange
		var fileWrapper = new FileWrapper();
		string directory = Path.GetTempPath();
		string path = Path.Combine(directory, "file.txt");

		File.Create(path).Close();

		// Act
		fileWrapper.DeleteFile(path);

		// Assert
		Path.Exists(path).Should().BeFalse();
	}
}
