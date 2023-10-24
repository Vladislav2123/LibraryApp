namespace LibraryApp.Domain.Enteties
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public DateOnly BirthDate { get; set; }

		public ICollection<Book> ReadedBooks { get; set; }
		public ICollection<Review> Reviews { get; set; }
	}
}
