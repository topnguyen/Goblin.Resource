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
        private readonly IBootstrapper _bootstrapper;

        public BootstrapperService(IGoblinUnitOfWork goblinUnitOfWork, IBootstrapper bootstrapper) : base(goblinUnitOfWork)
        {
            _bootstrapper = bootstrapper;
        }

        public async Task InitialAsync(CancellationToken cancellationToken = default)
        {
            await _bootstrapper.InitialAsync(cancellationToken).ConfigureAwait(true);
        }

        public Task RebuildAsync(CancellationToken cancellationToken = default)
        {
            _bootstrapper.RebuildAsync(cancellationToken).Wait(cancellationToken);

            return Task.CompletedTask;
        }
    }
}