using Goblin.Service_Resource.Contract.Repository.Models;

namespace Goblin.Service_Resource.Contract.Repository.Interfaces
{
    public interface IRepository<T> : Elect.Data.EF.Interfaces.Repository.IRepository<T> where T : GoblinEntity, new()
    {
    }
}