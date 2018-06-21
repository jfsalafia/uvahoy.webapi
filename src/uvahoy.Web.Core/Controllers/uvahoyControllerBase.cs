using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace uvahoy.Controllers
{
    public abstract class uvahoyControllerBase: AbpController
    {
        protected uvahoyControllerBase()
        {
            LocalizationSourceName = uvahoyConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
