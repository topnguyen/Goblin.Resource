using System.Reflection;
using Elect.Core.ConfigUtils;
using Elect.Core.EnvUtils;
using Elect.Data.EF.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goblin.Service_Resource.Repository
{
    public static class IServiceCollectionExtensions
    {
        private const int CommandTimeoutInSecond = 20 * 60;

        public static IServiceCollection AddGoblinDbContext(this IServiceCollection services)
        {
            services.AddDbContextPool<IDbContext, GoblinDbContext>(optionsBuilder =>
            {
                var config =
                    new ConfigurationBuilder()
                        .AddJsonFile(Elect.Core.Constants.ConfigurationFileName.ConnectionConfig, false, true)
                        .Build();

                var connectionString =
                    config.GetValueByEnv<string>(Elect.Core.Constants.ConfigurationSectionName.ConnectionStrings);

                optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction =>
                {
                    // Command timeout in seconds
                    sqlServerOptionsAction.CommandTimeout(CommandTimeoutInSecond);

                    sqlServerOptionsAction.MigrationsAssembly(typeof(GoblinDbContext).GetTypeInfo().Assembly
                        .GetName().Name);

                    sqlServerOptionsAction.MigrationsHistoryTable("Migration");
                });

                optionsBuilder.EnableSensitiveDataLogging(EnvHelper.IsDevelopment());

                // Force all query is No Tracking to boost-up performance
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, 200);

            return services;
        }
    }
}