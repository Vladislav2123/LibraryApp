using LibraryApp.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace LibraryApp.API.Authentication;

public class CustomJwtValidationMiddleware : IMiddleware
{
	private readonly ILibraryDbContext _libraryDbContext;

	public CustomJwtValidationMiddleware(ILibraryDbContext libraryDbContext)
	{
		_libraryDbContext = libraryDbContext;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (context.User.Identity.IsAuthenticated)
		{
			Guid userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.Actor));

			if (await _libraryDbContext.Users.AnyAsync(user => user.Id == userId) == false)
			{
				throw new SecurityTokenException("Invalid User");
			}
		}

		await next(context);
	}
}
