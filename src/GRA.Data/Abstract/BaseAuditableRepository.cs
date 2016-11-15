using AutoMapper.QueryableExtensions;
using GRA.Data.Extension;
using GRA.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GRA.Data.Abstract
{
    public abstract class BaseAuditableRepository<DbEntity, DomainEntity> 
        : Domain.Abstract.IAuditableRepository<DbEntity, DomainEntity> 
        where DbEntity : BaseDbEntity 
        where DomainEntity : Domain.Abstract.IDomainEntity
    {
        private readonly Context context;
        private readonly ILogger<BaseAuditableRepository<DbEntity, DomainEntity>> logger;
        private readonly AutoMapper.IMapper mapper;

        private DbSet<DbEntity> dbSet;
        private DbSet<AuditLog> auditSet;

        internal BaseAuditableRepository(Context context,
            ILogger<BaseAuditableRepository<DbEntity, DomainEntity>> logger,
           AutoMapper.IMapper mapper)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.logger = logger;
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            this.mapper = mapper;
        }

        private void AuditLog(int userId,
            BaseDbEntity newObject,
            BaseDbEntity priorObject = null)
        {
            var audit = new Data.Model.AuditLog
            {
                EntityType = newObject.GetType().ToString(),
                EntityId = newObject.Id,
                UpdatedBy = userId,
                UpdatedAt = DateTime.Now,
                CurrentValue = JsonConvert.SerializeObject(newObject)
            };
            if (priorObject != null)
            {
                audit.PreviousValue = JsonConvert.SerializeObject(priorObject);
            }
            AuditSet.Add(audit);
            try
            {
                if (context.SaveChanges() != 1)
                {
                    logger.LogError($"Error writing audit log for {newObject.GetType()} id {newObject.Id}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(null, ex, $"Error writing audit log for {newObject.GetType()} id {newObject.Id}");
            }
        }

        protected DbSet<AuditLog> AuditSet {
            get {
                return auditSet ?? (auditSet = context.Set<AuditLog>());
            }
        }
        protected DbSet<DbEntity> DbSet {
            get {
                return dbSet ?? (dbSet = context.Set<DbEntity>());
            }
        }

        public IQueryable<DomainEntity> GetAll()
        {
            return DbSet.ProjectTo<DomainEntity>();
        }

        public IQueryable<DomainEntity> PageAll(int skip, int take)
        {
            return DbSet.Skip(skip).Take(take).ProjectTo<DomainEntity>();
        }

        public virtual DomainEntity GetById(int id)
        {
            return mapper.Map<DbEntity, DomainEntity>(DbSet.Find(id));
        }

        public virtual void Add(int userId, DomainEntity domainEntity)
        {
            DbEntity entity = mapper.Map<DomainEntity, DbEntity>(domainEntity);
            EntityEntry<DbEntity> dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State != (EntityState)EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
            AuditLog(userId, entity);
        }
        public virtual void Update(int userId, DomainEntity domainEntity)
        {
            DbEntity entity = mapper.Map<DomainEntity, DbEntity>(domainEntity);
            var original = DbSet.Find(entity.Id);
            EntityEntry<DbEntity> dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State != (EntityState)EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            AuditLog(userId, entity, original);
        }

        public void Remove(int userId, DomainEntity domainEntity)
        {
            DbEntity entity = mapper.Map<DomainEntity, DbEntity>(domainEntity);
            EntityEntry<DbEntity> dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State != (EntityState)EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
            AuditLog(userId, null, entity);
        }

        public void Remove(int userId, int id)
        {
            var entity = GetById(id);
            if (entity == null) return;

            Remove(userId, entity);
        }
    }
}
