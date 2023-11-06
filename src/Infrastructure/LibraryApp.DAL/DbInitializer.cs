namespace LibraryApp.DAL
{
	public class DbInitializer
	{
		public static void Initialize(LibraryDbContext dbContext)
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
		}
	}
}
