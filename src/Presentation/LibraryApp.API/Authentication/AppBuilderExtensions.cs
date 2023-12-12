namespace LibraryApp.API.Authentication
{
	public static class AppBuilderExtensions
	{
		public static IApplicationBuilder UseCustomJwtValidation(this IApplicationBuilder app)
		{
			return app.UseMiddleware<CustomJwtValidationMiddleware>();
		}
	}
}
