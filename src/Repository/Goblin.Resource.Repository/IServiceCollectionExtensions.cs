using Goblin.Notification.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Goblin.Resource.Repository
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddGoblinDbContext(this IServiceCollection services)
        {
            GoblinDbContextFactoryHelper.Build(services, null);
            
            return services;
        }
    }
}