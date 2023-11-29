﻿namespace LibraryApp.Application.Common.Exceptions
{
	public class BookAlreadyHasReviewException : Exception
	{
        public BookAlreadyHasReviewException(object bookId, object userId)
            : base($"Book ({bookId}) already has a User ({userId}) review") { }
    }
}
