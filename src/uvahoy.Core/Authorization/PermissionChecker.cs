using Abp.Authorization;
using uvahoy.Authorization.Roles;
using uvahoy.Authorization.Users;

namespace uvahoy.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
