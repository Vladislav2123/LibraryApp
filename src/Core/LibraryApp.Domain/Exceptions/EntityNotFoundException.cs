namespace LibraryApp.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
	public EntityNotFoundException(string name, object id) 
		: base($"Entity {name} ({id}) not found") { }
}
