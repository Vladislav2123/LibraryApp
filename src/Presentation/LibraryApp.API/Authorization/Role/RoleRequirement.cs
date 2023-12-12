using Microsoft.AspNetCore.Authorization;

namespace LibraryApp.API.Authorization.Role
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string AllowedRole { get; }

        public RoleRequirement(string allowedRole)
        {
            AllowedRole = allowedRole;
        }
    }
}
