using System.Reflection;
using Elect.Core.ConfigUtils;
using Elect.Core.EnvUtils;
using Elect.Data.EF.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goblin.Resource.Repository
{
    public static class GoblinDbContextSetup
    {
        /// <summary>
        ///     Setup Goblin DB Context based on have <param name="services"></param> data or not <br />
        ///     If have data for <param name="services"></param> will use AddDbContextPool instead of AddDbContext
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbContextOptionsBuilder"></param>
        /// <returns></returns>
        public static void Build(IServiceCollection services, DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("connectionconfig.json", false, false);

            var config = configBuilder.Build();

            var connectionString = config.GetValueByEnv<string>("ConnectionString");
            
            var commandTimeoutInSecond = config.GetValueByEnv<int>("CommandTimeoutInSecond");

            var dbContextPoolSize = config.GetValueByEnv<int>("DbContextPoolSize");

            if (services != null)
            {
                services.AddDbContextPool<IDbContext, GoblinDbContext>(optionsBuilder =>
                {
                    Build(optionsBuilder, connectionString, commandTimeoutInSecond);
                }, dbContextPoolSize);
            }
            else
            {
                if (dbContextOptionsBuilder.IsConfigured)
                {
                    return;
                }
                
                Build(dbContextOptionsBuilder, connectionString, commandTimeoutInSecond);
            }
        }

        public static void Build(DbContextOptionsBuilder optionsBuilder, string connectionString, int commandTimeoutInSecond)
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
        }
    }
}