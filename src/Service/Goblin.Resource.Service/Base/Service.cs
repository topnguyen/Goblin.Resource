using Goblin.Resource.Contract.Repository.Interfaces;

namespace Goblin.Resource.Service.Base
{
    public abstract class Service
    {
        protected readonly IGoblinUnitOfWork GoblinUnitOfWork;

        protected Service(IGoblinUnitOfWork goblinUnitOfWork)
        {
            GoblinUnitOfWork = goblinUnitOfWork;
        }
    }
}