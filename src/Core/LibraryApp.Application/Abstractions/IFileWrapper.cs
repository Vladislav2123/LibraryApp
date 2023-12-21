using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Abstractions;
public interface IFileWrapper
{
	Task SaveFileAsync(
		IFormFile file, 
		string path, 
		CancellationToken cancellationToken);
	void DeleteFile(string? path);
}
