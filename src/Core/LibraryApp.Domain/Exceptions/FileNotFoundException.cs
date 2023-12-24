namespace LibraryApp.Domain.Exceptions;

public class FileNotFoundException : Exception
{
        public FileNotFoundException(string fileType)
            : base($"{fileType} file not found") { }
    }
