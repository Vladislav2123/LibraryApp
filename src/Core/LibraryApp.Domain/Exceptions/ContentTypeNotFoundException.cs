namespace LibraryApp.Domain.Exceptions;

public class ContentTypeNotFoundException : Exception
{
	public ContentTypeNotFoundException(string fileType) 
		: base($"{fileType} file content type not found") { }
    }
