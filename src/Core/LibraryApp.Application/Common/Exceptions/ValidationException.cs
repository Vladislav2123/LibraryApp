namespace LibraryApp.Application.Common.Exceptions
{
	public class ValidationException : Exception
	{
        public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary) : base("Validation Failure")
        {
            ErrorsDictionary = errorsDictionary;
        }

        public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
	}
}
