using GRA.Data.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using AutoMapper;

namespace GRA.Data.Abstract
{
    abstract class BaseRepository<T> : Domain.Abstract.IRepository<T> where T : BaseDbEntity
    {
        private readonly Context context;
        private readonly ILogger<BaseRepository<T>> logger;
        private readonly AutoMapper.IMapper mapper;

        private DbSet<T> dbSet;

        internal BaseRepository(Context context,
            ILogger<BaseRepository<T>> logger,
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

        protected DbSet<T> DbSet {
            get {
                return dbSet ?? (dbSet = context.Set<T>());
            }
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public IQueryable<T> PageAll(int skip, int take)
        {
            return DbSet.Skip(skip).Take(take);
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            EntityEntry<T> dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State != (EntityState)EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }
        public virtual void Update(T entity)
        {
            EntityEntry<T> dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State != (EntityState)EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            EntityEntry<T> dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State != (EntityState)EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public void Remove(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;

            Remove(entity);
        }
    }
}
