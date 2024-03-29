﻿namespace LibraryApp.Domain.Exceptions;

public class UserAlreadyReadBookException : Exception
{
	public UserAlreadyReadBookException(Guid userId, Guid bookId)
		: base($"User ({userId}) already read book ({bookId})") { }
    }
