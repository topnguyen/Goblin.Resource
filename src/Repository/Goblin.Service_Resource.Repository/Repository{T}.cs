using Elect.DI.Attributes;
using Goblin.Service_Resource.Contract.Repository.Interfaces;
using Goblin.Service_Resource.Contract.Repository.Models;

namespace Goblin.Service_Resource.Repository
{
    [ScopedDependency(ServiceType = typeof(IRepository<>))]
    public class Repository<T> : Elect.Data.EF.Services.Repository.BaseEntityRepository<T>, IRepository<T> where T : GoblinEntity, new()
    {
        public Repository(Elect.Data.EF.Interfaces.DbContext.IDbContext dbContext) : base(dbContext)
        {
        }
    }
}