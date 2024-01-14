using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LibraryApp.Domain.Entities;

namespace LibraryApp.API.Authorization.Common;

public abstract class BaseResourceEditHandler<TRequirement, Guid> 
	: AuthorizationHandler<TRequirement, Guid>
	where TRequirement : BaseResourceEditRequirement
{
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		TRequirement requirement,
		Guid resourceId)
	{
		if (requirement.AllowAdmins &&
			context.User.HasClaim(ClaimTypes.Role, UserRole.Admin.ToString()))
		{
			context.Succeed(requirement);
			return;
		}

		if (await IsResourceAuthor(context, resourceId))
			context.Succeed(requirement);
	}

	protected abstract Task<bool> IsResourceAuthor(
		AuthorizationHandlerContext context, Guid resourceId);
}
