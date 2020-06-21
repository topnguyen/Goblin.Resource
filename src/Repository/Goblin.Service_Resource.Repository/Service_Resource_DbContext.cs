using Elect.Core.ConfigUtils;
using Elect.Data.EF.Utils.ModelBuilderUtils;
using Elect.DI.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Elect.Core.EnvUtils;

namespace Goblin.Service_Resource.Repository
{
    [ScopedDependency(ServiceType = typeof(Elect.Data.EF.Interfaces.DbContext.IDbContext))]
    public sealed partial class Service_Resource_DbContext : Elect.Data.EF.Services.DbContext.DbContext
    {
        public readonly int CommandTimeoutInSecond = 20 * 60;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

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
                
                sqlServerOptionsAction.MigrationsAssembly(typeof(Service_Resource_DbContext).GetTypeInfo().Assembly.GetName().Name);

                sqlServerOptionsAction.MigrationsHistoryTable("Migration");
            });
            
            optionsBuilder.EnableSensitiveDataLogging(EnvHelper.IsDevelopment());

            // Force all query is No Tracking to boost-up performance
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // [Important] Keep Under Base For Override And Make End Result

            // Scan and apply Config/Mapping for Tables/Entities (from folder "Maps")
            builder.AddConfigFromAssembly<Service_Resource_DbContext>(typeof(Service_Resource_DbContext).GetTypeInfo().Assembly);

            // Set Delete Behavior as Restrict in Relationship
            builder.DisableCascadingDelete();

            // Convention for Table name
            builder.RemovePluralizingTableNameConvention();

            builder.ReplaceTableNameConvention("Entity", string.Empty);
        }
    }
}