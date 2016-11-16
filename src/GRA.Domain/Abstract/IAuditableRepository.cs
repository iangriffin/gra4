using System.Linq;

namespace GRA.Domain.Abstract
{
    public interface IAuditableRepository<DomainEntity>
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
