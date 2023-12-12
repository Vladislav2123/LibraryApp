namespace LibraryApp.API.ExceptionsHandling
{
	public static class AppBuilderExtensions
	{
		public static IApplicationBuilder UseGlobalExceptionsHandling(this IApplicationBuilder app)
		{
			return app.UseMiddleware<GlobalExceptionsHandlingMiddleware>();
		}
	}
}
