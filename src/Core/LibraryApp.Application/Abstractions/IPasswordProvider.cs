namespace LibraryApp.Application.Abstractions;

/// <summary>
/// Interface for the passwords hashing.
/// </summary>
public interface IPasswordProvider
{
	string HashPassword(string password, string salt);
	string GenerateSalt();
}
