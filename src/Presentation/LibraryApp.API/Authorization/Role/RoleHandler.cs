using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryApp.API.Authorization.Role
{
	public class RoleHandler : AuthorizationHandler<RoleRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
		{
			if (context.User.HasClaim(ClaimTypes.Role, requirement.AllowedRole))
				context.Succeed(requirement);

			return Task.CompletedTask;
		}
	}
}
