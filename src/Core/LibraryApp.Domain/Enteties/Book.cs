namespace LibraryApp.Domain.Enteties
{
	public class Book
	{
		public Guid Id { get; set; }
		public Guid AuthorId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Year { get; set; }
		public string Text { get; set; }

		public ICollection<User> ReadUsers { get; set; }

		public ICollection<Review> Reviews { get; set; }
		public float Rating { get; set; }
	}
}
