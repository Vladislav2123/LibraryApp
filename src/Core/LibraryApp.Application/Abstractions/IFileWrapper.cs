using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Abstractions;
/// <summary>
/// Wrapper for the "File" class.
/// </summary>
public interface IFileWrapper
{
	Task SaveFileAsync(
		IFormFile file, 
		string path, 
		CancellationToken cancellationToken);
	void DeleteFile(string? path);
}
