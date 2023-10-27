namespace LibraryApp.Domain.Enteties
{
	public class Review
	{
		public Guid Id { get; set; }
		public DateTime CreationDate {  get; set; }
		public int Rating { get; set; }
		public string? Title { get; set; }
		public string? Text { get; set; }

		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid BookId { get; set; }
		public Book Book { get; set; }
	}
}
