﻿namespace LibraryApp.Domain.Exceptions;

public class UserHasNotReadBookException : Exception
{
	public UserHasNotReadBookException(Guid userId, Guid bookId)
		: base($"User ({userId}) has not read book ({bookId})") { }
    }