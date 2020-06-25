using Goblin.Service_Resource.Contract.Repository.Interfaces;

namespace Goblin.Service_Resource.Service.Base
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