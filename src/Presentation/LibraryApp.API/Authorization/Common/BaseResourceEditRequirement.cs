using Microsoft.AspNetCore.Authorization;

namespace LibraryApp.API.Authorization.Common
{
	public class BaseResourceEditRequirement : IAuthorizationRequirement
	{
		public bool AllowAdmins { get; }

        public BaseResourceEditRequirement(bool allowAdmins)
        {
            AllowAdmins = allowAdmins;
        }
    }
}
