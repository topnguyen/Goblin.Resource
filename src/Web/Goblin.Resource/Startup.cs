using Elect.Core.ConfigUtils;
using Elect.Data.IO;
using Elect.Data.IO.DirectoryUtils;
using Goblin.Core.Web.Setup;
using Goblin.Resource.Core;
using Goblin.Resource.Repository;
using Goblin.Resource.Share.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

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

                // Directory Browser

                if (SystemSetting.Current.IsEnableDictionaryBrowser)
                {
                    services.AddDirectoryBrowser();
                }
            };

            BeforeConfigureApp = (app, environment, lifetime) =>
            {
                var resourceFolderPathAbsolute = PathHelper.GetFullPath(SystemSetting.Current.ResourceFolderPath);

                DirectoryHelper.CreateIfNotExist(resourceFolderPathAbsolute);

                // Static Files

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(resourceFolderPathAbsolute),

                    RequestPath = new PathString(SystemSetting.Current.ResourceFolderEndpoint)
                });

                // Directory Browser

                if (SystemSetting.Current.IsEnableDictionaryBrowser)
                {
                    app.UseDirectoryBrowser(new DirectoryBrowserOptions
                    {
                        FileProvider = new PhysicalFileProvider(resourceFolderPathAbsolute),
                        RequestPath = new PathString(SystemSetting.Current.ResourceFolderEndpoint)
                    });
                }
            };
        }
    }
}