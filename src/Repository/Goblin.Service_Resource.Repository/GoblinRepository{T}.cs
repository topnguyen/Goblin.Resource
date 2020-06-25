using Elect.DI.Attributes;
using Goblin.Service_Resource.Contract.Repository.Interfaces;
using Goblin.Service_Resource.Contract.Repository.Models;

namespace Goblin.Service_Resource.Repository
{
    [ScopedDependency(ServiceType = typeof(IGoblinRepository<>))]
    public class GoblinRepository<T> : Elect.Data.EF.Services.Repository.BaseEntityRepository<T>, IGoblinRepository<T> where T : GoblinEntity, new()
    {
        public GoblinRepository(Elect.Data.EF.Interfaces.DbContext.IDbContext dbContext) : base(dbContext)
        {
        }
    }
}