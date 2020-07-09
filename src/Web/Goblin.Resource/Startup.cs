using Elect.Core.ConfigUtils;
using Goblin.Core.Web.Setup;
using Goblin.Resource.Core;
using Goblin.Resource.Repository;
using Goblin.Resource.Share.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Goblin.Resource
{
    public class Startup : BaseApiStartup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
            RegisterValidators.Add(typeof(IValidator));

            BeforeConfigureServices = services =>
            {
                // Setting

                SystemSetting.Current = Configuration.GetSection<SystemSetting>("Setting");
                
                // Database

                services.AddGoblinDbContext();
            };
        }
    }
}