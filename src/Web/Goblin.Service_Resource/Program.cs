using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Goblin.Service_Resource
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Goblin.Core.Web.Setup.ProgramHelper.Main(args, webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                }, scope =>
                {
                    // Initial Database
                    //
                    // var infrastructureBootstrapper = scope.ServiceProvider.GetService<IBootstrapper>();
                    //
                    // infrastructureBootstrapper.InitialAsync().Wait();
                }
            );
        }
    }
}