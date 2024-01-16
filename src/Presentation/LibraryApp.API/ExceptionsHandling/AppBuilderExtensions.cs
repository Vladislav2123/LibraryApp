namespace LibraryApp.API.ExceptionsHandling;

public static class AppBuilderExtensions
{
	/// <summary>
	/// Adding global exceptions handling middleware.
	/// </summary>
	public static IApplicationBuilder UseGlobalExceptionsHandling(this IApplicationBuilder app)
	{
		return app.UseMiddleware<GlobalExceptionsHandlingMiddleware>();
	}
}
