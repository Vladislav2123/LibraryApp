namespace LibraryApp.Domain.Enteties
{
	public class Author
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateOnly? BirthDate { get; set; }
		public DateTime CreationDate { get; set; }

		public string? AvatarPath { get; set; }
		public string AvatarUrl => $"/api/authors/{Id}/avatar";

		public Guid CreatedUserId { get; set; }
		public User CreatedUser { get; set; }

		public ICollection<Book> Books { get; set; }

	}
}
