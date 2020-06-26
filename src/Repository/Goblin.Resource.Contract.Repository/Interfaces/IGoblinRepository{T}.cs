using Goblin.Resource.Contract.Repository.Models;

namespace Goblin.Resource.Contract.Repository.Interfaces
{
    public interface IGoblinRepository<T> : Elect.Data.EF.Interfaces.Repository.IBaseEntityRepository<T> where T : GoblinEntity, new()
    {
    }
}