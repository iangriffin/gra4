using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRA.Domain.Abstract
{
    public interface IAuditableRepository<DbEntity, DomainEntity>
    {
        IQueryable<DomainEntity> GetAll();
        IQueryable<DomainEntity> PageAll(int skip, int take);
        DomainEntity GetById(int id);
        void Add(int userId, DomainEntity entity);
        void Update(int userId, DomainEntity entity);
        void Remove(int userId, DomainEntity entity);
        void Remove(int userId, int id);
    }
}
