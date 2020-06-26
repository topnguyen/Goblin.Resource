using System;
using Goblin.Core.Constants;
using Goblin.Core.Web.Utils;
using Goblin.Resource.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Goblin.Resource.Filters
{
    public static class GoblinApiAuthHelper
    {
        public static bool IsAuthenticate(AuthorizationFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(SystemSetting.Current.AuthorizationKey))
            {
                return true;
            }

            var authenticationKey = context.HttpContext.GetKey<string>(GoblinHeaderKeys.Authorization);

            var isAuthenticate = string.Equals(authenticationKey?.Trim(),
                SystemSetting.Current.AuthorizationKey?.Trim(), StringComparison.InvariantCultureIgnoreCase);

            return isAuthenticate;
        }

       
    }
}