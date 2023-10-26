﻿namespace LibraryApp.Application.Common.Exceptions
{
	public class EntityAlreadyExistException : Exception
	{
		public EntityAlreadyExistException(string name)
			: base($"Entity {name} with the same parameters already exist.") { }
	}
}
