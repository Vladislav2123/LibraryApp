namespace LibraryApp.Domain.Entities;

public class Review
{
	public Guid Id { get; set; }
	public DateTime CreationDate {  get; set; }
	public byte Rating { get; set; }
	public string? Title { get; set; }
	public string? Comment { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid BookId { get; set; }
	public Book Book { get; set; }
}
