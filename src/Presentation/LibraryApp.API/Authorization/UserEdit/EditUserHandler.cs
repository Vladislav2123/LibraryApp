using LibraryApp.API.Authorization.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryApp.API.Authorization.UserEdit;

public class EditUserHandler : BaseResourceEditHandler<EditUserRequirement, Guid>
{
	protected override async Task<bool> IsResourceAuthor(AuthorizationHandlerContext context, Guid resourceId)
	{
		if (Guid.TryParse(context.User.FindFirstValue(ClaimTypes.Actor), out Guid userId) == false)
			return false;

		return userId == resourceId;
	}
}
