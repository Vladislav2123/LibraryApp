using LibraryApp.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace LibraryApp.Application.Common.PasswordProviders;

public class BcryptPasswordProvider : IPasswordProvider
{
	private readonly IConfiguration _config;

	public BcryptPasswordProvider(IConfiguration config)
	{
		_config = config;
	}

	public string HashPassword(string password, string salt)
	{
		string? pepper = _config["Passwords:Pepper"];
		string hash = BCrypt.Net.BCrypt.HashPassword(password + pepper, salt);

		return hash;
	}

	public string GenerateSalt() => BCrypt.Net.BCrypt.GenerateSalt();
}
