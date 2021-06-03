using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DC.Entities.Base
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindAllBy(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T Find(int id);
        void Add(T entity);
        void Add(IQueryable<T> entities);
        void Delete(T entity);
        void Delete(IQueryable<T> entities);
        void Edit(T entity);
        void Edit(IQueryable<T> entities);
        int Save();
        int Count(Expression<Func<T, bool>> predicate);
        int Count();
        void Refresh(T entity);
    }
}
