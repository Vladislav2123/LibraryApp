namespace LibraryApp.API.Authentication;

public static class AppBuilderExtensions
{
	/// <summary>
	/// Adding JWT validation layer.
	/// </summary>
	public static IApplicationBuilder UseCustomJwtValidation(this IApplicationBuilder app)
	{
		return app.UseMiddleware<CustomJwtValidationMiddleware>();
	}
}
