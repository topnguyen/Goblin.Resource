using Elect.DI.Attributes;
using Goblin.Core.Constants;
using Goblin.Core.Web.Utils;
using Goblin.Resource.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Goblin.Resource.Filters
{
    [ScopedDependency]
    public class GoblinApiAuthFilter: IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthenticate = GoblinApiAuthHelper.IsAuthenticate(context);

            if (!isAuthenticate)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            
            var userId = context.HttpContext.GetKey<long>(GoblinHeaderKeys.UserId);

            LoggedInUser.Current = new LoggedInUser(userId);
        }
    }
}