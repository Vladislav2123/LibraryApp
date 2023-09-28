namespace LibraryApp.Domain.Enteties
{
	public class Review
	{
		public Guid Id { get; set; }
		public Guid UsetId { get; set; }
		public Guid BookId { get; set; }
		public DateTime Date {  get; set; }
		public int Rating { get; set; }
		public string? Title { get; set; }
		public string? Text { get; set; }
	}
}
