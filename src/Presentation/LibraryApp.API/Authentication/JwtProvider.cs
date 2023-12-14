using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Enteties;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryApp.API.Authentication;

public class JwtProvider : IJwtProvider
{
	private readonly AuthenticationConfig _authConfig;

	public JwtProvider(IOptions<AuthenticationConfig> authConfigOptions)
	{
		_authConfig = authConfigOptions.Value;
	}

	public string GetJwtToken(User user)
	{
		var claims = new Claim[]
		{
			new Claim(ClaimTypes.Actor, user.Id.ToString()),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role.ToString()),
		};

		var credentials = new SigningCredentials(
			new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_authConfig.Key)),
			SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			_authConfig.Issuer,
			_authConfig.Audience,
			claims,
			null,
			DateTime.UtcNow.AddHours(_authConfig.Validity),
			credentials
		);

		string result = new JwtSecurityTokenHandler()
			.WriteToken(token);

		return result;
	}
}
