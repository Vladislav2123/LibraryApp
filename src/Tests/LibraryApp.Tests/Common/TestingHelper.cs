using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LibraryApp.Tests.Common;
public class TestingHelper
{
	public static string GetTesingFile(string fileName)
	{
		string tempDirectory = Path.GetTempPath();
		string path = Path.Combine(tempDirectory, fileName);

		File.Create(path).Close();

		return path;
	}

	public static HttpContext GetHttpContextWithActorClaim(string actorClaimValue)
	{
		var claims = new List<Claim>()
		{
			new Claim(ClaimTypes.Actor, actorClaimValue)
		};

		var identity = new ClaimsIdentity(claims);
		var claimsPrincipal = new ClaimsPrincipal(identity);

		HttpContext httpContext = new DefaultHttpContext();
		httpContext.User = claimsPrincipal;

		return httpContext;
	}
}
