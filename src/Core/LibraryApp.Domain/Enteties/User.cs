namespace LibraryApp.Domain.Enteties
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public DateOnly BirthDate { get; set; }
		public DateTime CreationDate { get; set; }

		public ICollection<Author> CreatedAuthors { get; set; }
		public ICollection<Book> CreatedBooks { get; set; }
		public ICollection<Book> ReadBooks { get; set; }
		public ICollection<Review> Reviews { get; set; }
	}
}
