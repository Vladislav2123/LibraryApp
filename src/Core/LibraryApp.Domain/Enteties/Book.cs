namespace LibraryApp.Domain.Enteties
{
	public class Book
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public double Rating { get; set; }
		public string Description { get; set; }
		public int Year { get; set; }
		public string Text { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid CreatedUserId { get; set; }
		public User CreatedUser { get; set; }

		public Guid AuthorId { get; set; }
		public Author Author { get; set; }

		public ICollection<User> Readers { get; set; }

		public ICollection<Review> Reviews { get; set; }
	}
}
