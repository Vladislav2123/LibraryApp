namespace LibraryApp.Application.Abstractions;

public interface IPasswordProvider
{
	string HashPassword(string password, string salt);
	string GenerateSalt();
}
