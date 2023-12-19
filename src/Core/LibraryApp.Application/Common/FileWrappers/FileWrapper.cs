using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Common.FileWrappers;
public class FileWrapper : IFileWrapper
{
	public async Task SaveFileAsync(IFormFile file, string path, CancellationToken cancellationToken)
	{
		using(var stream = new FileStream(path, FileMode.Create))
		{
			await file.CopyToAsync(stream, cancellationToken);
		}
	}

	public void DeleteFile(string path)
	{
		File.Delete(path);
	}
}
