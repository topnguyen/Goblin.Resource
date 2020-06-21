using Elect.Core.ObjUtils;
using Elect.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Goblin.Service_Resource.Contract.Repository.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Elect.DI.Attributes;
using Goblin.Core.DateTimeUtils;
using Goblin.Service_Resource.Contract.Repository.Models;

namespace Goblin.Service_Resource.Repository
{
    [ScopedDependency(ServiceType = typeof(IUnitOfWork))]
    public class UnitOfWork : Elect.Data.EF.Services.UnitOfWork.BaseEntityUnitOfWork, IUnitOfWork
    {
        protected readonly IServiceProvider ServiceProvider;

        protected ConcurrentDictionary<Type, object> Repositories = new ConcurrentDictionary<Type, object>();

        public UnitOfWork(Elect.Data.EF.Interfaces.DbContext.IDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            ServiceProvider = serviceProvider;
        }

        public IRepository<T> GetRepository<T>() where T : GoblinEntity, new()
        {
            if (!Repositories.TryGetValue(typeof(IRepository<T>), out var repository))
            {
                Repositories[typeof(IRepository<T>)] = repository = ServiceProvider.GetRequiredService<IRepository<T>>();
            }

            return repository as IRepository<T>;
        }

        protected override void StandardizeEntities()
        {
            var listState = new List<EntityState>
            {
                EntityState.Added,
                EntityState.Modified,
                EntityState.Deleted
            };

            var listEntry = DbContext.ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && listState.Contains(x.State))
                .Select(x => x).ToList();

            var dateTimeNow = GoblinDateTimeHelper.SystemTimeNow;

            foreach (var entry in listEntry)
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        baseEntity.DeletedTime = null;

                        baseEntity.LastUpdatedTime = baseEntity.CreatedTime = dateTimeNow;
                    }
                    else
                    {
                        if (baseEntity.DeletedTime != null)
                        {
                            baseEntity.DeletedTime =
                                ObjHelper.ReplaceNullOrDefault(baseEntity.DeletedTime, dateTimeNow);
                        }
                        else
                        {
                            baseEntity.LastUpdatedTime = dateTimeNow;
                        }
                    }
                }

                if (!(entry.Entity is GoblinEntity entity))
                {
                    continue;
                }

                var loggedInUserId = 0; // TODO

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = entity.LastUpdatedBy = entity.CreatedBy ?? loggedInUserId;
                }
                else
                {
                    if (entity.DeletedTime != null)
                    {
                        entity.DeletedBy ??= loggedInUserId;
                    }
                    else
                    {
                        entity.LastUpdatedBy ??= loggedInUserId;
                    }
                }
            }
        }
    }
}