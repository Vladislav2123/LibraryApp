namespace LibraryApp.Domain.Exceptions;

public class EntityAlreadyExistException : Exception
{
	public EntityAlreadyExistException(string name)
		: base($"Entity {name} with the same values already exist") { }
}
