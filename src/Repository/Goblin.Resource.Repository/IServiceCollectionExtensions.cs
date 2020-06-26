using System.Reflection;
using Elect.Core.ConfigUtils;
using Elect.Core.EnvUtils;
using Elect.Data.EF.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goblin.Resource.Repository
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddGoblinDbContext(this IServiceCollection services)
        {
            var configBuilder =
                new ConfigurationBuilder()
                    .AddJsonFile("connectionconfig.json", false, false);

            var config = configBuilder.Build();

            var connectionString = config.GetValueByEnv<string>("ConnectionString");
            
            var commandTimeoutInSecond = config.GetValueByEnv<int>("CommandTimeoutInSecond");

            var dbContextPoolSize = config.GetValueByEnv<int>("DbContextPoolSize");

            services.AddDbContextPool<IDbContext, GoblinDbContext>(optionsBuilder =>
            {
                optionsBuilder
                    .UseSqlServer(connectionString, sqlServerOptionsAction =>
                    {
                        sqlServerOptionsAction
                            .CommandTimeout(commandTimeoutInSecond)
                            .MigrationsAssembly(typeof(GoblinDbContext).GetTypeInfo().Assembly.GetName().Name)
                            .MigrationsHistoryTable("Migration");
                    })
                    .EnableSensitiveDataLogging(EnvHelper.IsDevelopment())
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, dbContextPoolSize);

            return services;
        }
    }
}