namespace LibraryApp.Domain.Exceptions;

public class ValidationException : Exception
{
        public ValidationException(string entityName, IReadOnlyDictionary<string, string[]> errorsDictionary) 
            : base($"{entityName} validation failed")
        {
            ErrorsDictionary = errorsDictionary;
        }

        public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}
