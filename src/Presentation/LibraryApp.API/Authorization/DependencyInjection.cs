using LibraryApp.API.Authorization.ReviewEdit;
using LibraryApp.API.Authorization.Role;
using LibraryApp.API.Authorization.UserEdit;
using LibraryApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace LibraryApp.API.Authorization;

public static class DependencyInjection
{
	public static IServiceCollection AddMyAuthorization(this IServiceCollection services)
	{
		// Adding Policies Handlers
		services.AddScoped<IAuthorizationHandler, RoleHandler>();
		services.AddScoped<IAuthorizationHandler, EditReviewHandler>();
		services.AddScoped<IAuthorizationHandler, EditUserHandler>();

		// Adding Authorization
		services.AddAuthorization(options =>
		{
			options.AddPolicy(Policies.AdminOnlyPolicyName, policy =>
				policy.Requirements.Add(new RoleRequirement(UserRole.Admin.ToString())));

			options.AddPolicy(Policies.ReviewUpdatePolicyName, policy =>
				policy.Requirements.Add(new EditReviewRequirement(false)));

			options.AddPolicy(Policies.ReviewDeletePolicyName, policy =>
				policy.Requirements.Add(new EditReviewRequirement(true)));

			options.AddPolicy(Policies.UserUpdatePolicyName, policy =>
				policy.Requirements.Add(new EditUserRequirement(false)));

			options.AddPolicy(Policies.UserDeletePolicyName, policy =>
				policy.Requirements.Add(new EditUserRequirement(true)));
		});

		return services;
	}
}
