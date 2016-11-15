using System.Collections.Generic;
using System.Linq;

namespace GRA.Domain.Abstract
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> PageAll(int skip, int take);
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void Remove(int id);
    }
}
