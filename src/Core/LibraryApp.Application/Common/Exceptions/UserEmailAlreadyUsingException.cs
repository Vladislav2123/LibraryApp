namespace LibraryApp.Application.Common.Exceptions;

public class UserEmailAlreadyUsingException : Exception
{
        public UserEmailAlreadyUsingException(string email)
            : base($"Email ({email}) already in use by another User") { }
    }
