using FluentAssertions;
using LibraryApp.Application.Common.FileWrappers;
using Xunit;

namespace LibraryApp.Tests.FileWrapperTests;
public class DeleteFileTests
{
	[Fact]
	public void DeleteFile_ExpectedData_FileDeleting()
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


	[Fact]
	public void DeleteFile_NullPath_ThrowNothing()
	{
		// Arrange
		var fileWrapper = new FileWrapper();

		// Act
		var action = () => fileWrapper.DeleteFile(string.Empty);

		// Assert
		action.Should().NotThrow();
	}
}
