using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Common.FileWrappers;
/// <summary>
/// Wrapper for the "File" class. Working with file system.
/// </summary>
public class FileWrapper : IFileWrapper
{
	/// <summary>
	/// Saving file to the file system.
	/// </summary>
	public async Task SaveFileAsync(IFormFile file, string path, CancellationToken cancellationToken)
	{
		using(var stream = new FileStream(path, FileMode.Create))
		{
			await file.CopyToAsync(stream, cancellationToken);
		}
	}

	/// <summary>
	/// Deleting file from the file system.
	/// </summary>
	public void DeleteFile(string? path)
	{
		if (string.IsNullOrEmpty(path)) return;

		File.Delete(path);
	}
}
