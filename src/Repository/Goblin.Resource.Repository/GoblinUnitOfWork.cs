using Elect.Core.ObjUtils;
using Elect.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Goblin.Resource.Contract.Repository.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Elect.DI.Attributes;
using Goblin.Core.DateTimeUtils;
using Goblin.Resource.Contract.Repository.Models;
using Goblin.Resource.Core;

namespace Goblin.Resource.Repository
{
    [ScopedDependency(ServiceType = typeof(IGoblinUnitOfWork))]
    public class GoblinUnitOfWork : Elect.Data.EF.Services.UnitOfWork.BaseEntityUnitOfWork, IGoblinUnitOfWork
    {
        protected readonly IServiceProvider ServiceProvider;

        protected ConcurrentDictionary<Type, object> Repositories = new ConcurrentDictionary<Type, object>();

        public GoblinUnitOfWork(Elect.Data.EF.Interfaces.DbContext.IDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            ServiceProvider = serviceProvider;
        }

        public IGoblinRepository<T> GetRepository<T>() where T : GoblinEntity, new()
        {
            if (!Repositories.TryGetValue(typeof(IGoblinRepository<T>), out var repository))
            {
                Repositories[typeof(IGoblinRepository<T>)] = repository = ServiceProvider.GetRequiredService<IGoblinRepository<T>>();
            }

            return repository as IGoblinRepository<T>;
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

                var loggedInUserId = LoggedInUser.Current.Id;

                if (entry.State == EntityState.Added)
                {
                    if (entity.CreatedBy == null || entity.CreatedBy == default(long))
                    {
                        entity.CreatedBy = entity.LastUpdatedBy = loggedInUserId;
                    }
                }
                else
                {
                    if (entity.DeletedTime != null)
                    {
                        if (entity.DeletedBy == null || entity.DeletedBy == default(long))
                        {
                            entity.DeletedBy = loggedInUserId;
                        }
                    }
                    else
                    {
                        if (entity.LastUpdatedBy == null || entity.LastUpdatedBy == default(long))
                        {
                            entity.LastUpdatedBy = loggedInUserId;
                        }
                    }
                }
            }
        }
    }
}