using Elect.DI.Attributes;
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
            bool isCanAccess = true;

            if (!string.IsNullOrWhiteSpace(SystemSetting.Current.Authorization))
            {
                context.HttpContext.Request.Headers.TryGetValue(nameof(SystemSetting.Current.Authorization), out var secretKeyInHeader);

                string httpRequestSecretKey = secretKeyInHeader;

                if (string.IsNullOrWhiteSpace(httpRequestSecretKey))
                {
                    httpRequestSecretKey = context.HttpContext.Request.Cookies[nameof(SystemSetting.Current.Authorization)];
                }
                
                if (string.IsNullOrWhiteSpace(secretKeyInHeader))
                {
                    httpRequestSecretKey = context.HttpContext.Request.Query[nameof(SystemSetting.Current.Authorization)];
                }

                isCanAccess = httpRequestSecretKey?.Trim().ToLowerInvariant() == SystemSetting.Current.Authorization?.Trim().ToLowerInvariant();
            }

            if (!isCanAccess)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
}