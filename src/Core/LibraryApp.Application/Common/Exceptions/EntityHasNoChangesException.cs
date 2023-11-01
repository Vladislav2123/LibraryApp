namespace LibraryApp.Application.Common.Exceptions
{
	public class EntityHasNoChangesException : Exception
	{
        public EntityHasNoChangesException(string name, object id)
            : base($"Entity {name} ({id}) already contain the same values.") { }
    }
}
