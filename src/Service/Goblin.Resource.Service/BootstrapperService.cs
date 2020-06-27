using Elect.DI.Attributes;
using Goblin.Resource.Contract.Repository.Interfaces;
using Goblin.Resource.Contract.Service;
using System.Threading;
using System.Threading.Tasks;

namespace Goblin.Resource.Service
{
    [ScopedDependency(ServiceType = typeof(IBootstrapperService))]
    public class BootstrapperService : Base.Service, IBootstrapperService
    {
        public BootstrapperService(IGoblinUnitOfWork goblinUnitOfWork) : base(goblinUnitOfWork)
        {
        }

        public Task InitialAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task RebuildAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}