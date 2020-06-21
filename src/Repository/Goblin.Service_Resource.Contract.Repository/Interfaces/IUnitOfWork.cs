using Goblin.Service_Resource.Contract.Repository.Models;

namespace Goblin.Service_Resource.Contract.Repository.Interfaces
{
    public interface IUnitOfWork : Elect.Data.EF.Interfaces.UnitOfWork.IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : GoblinEntity, new();
    }
}