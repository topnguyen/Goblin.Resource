using Goblin.Service_Resource.Contract.Repository.Interfaces;

namespace Goblin.Service_Resource.Service.Base
{
    public abstract class Service
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}