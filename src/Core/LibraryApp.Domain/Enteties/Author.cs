namespace LibraryApp.Domain.Enteties
{
	public class Author
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime BirthDate { get; set; }
		public ICollection<Book> Books { get; set; }
	}
}
