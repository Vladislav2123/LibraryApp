namespace LibraryApp.Domain.Entities;

public class User
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public string PasswordSalt{ get; set; }
	public DateOnly BirthDate { get; set; }
	public DateTime CreationDate { get; set; }
	public UserRole Role { get; set; }

	public string? AvatarPath { get; set; }
	public string AvatarUrl => $"/api/users/{Id}/avatar";

	public ICollection<Author> CreatedAuthors { get; set; }
	public ICollection<Book> CreatedBooks { get; set; }
	public ICollection<Book> ReadBooks { get; set; }
	public ICollection<Review> Reviews { get; set; }
}

public enum UserRole : byte
{
	Default,
	Admin
}
