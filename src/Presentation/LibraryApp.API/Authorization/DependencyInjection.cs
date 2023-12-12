using LibraryApp.API.Authorization.ReviewEdit;
using LibraryApp.API.Authorization.Role;
using LibraryApp.API.Authorization.UserEdit;
using Microsoft.AspNetCore.Authorization;

namespace LibraryApp.API.Authorization
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPoliciesHandlers(this IServiceCollection services)
		{
			services.AddScoped<IAuthorizationHandler, RoleHandler>();
			services.AddScoped<IAuthorizationHandler, EditReviewHandler>();
			services.AddScoped<IAuthorizationHandler, EditUserHandler>();

			return services;
		}
	}
}
