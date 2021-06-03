using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Entities.Base
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class , new()
    {
        //private DbContextTransaction _transaction = null;
        public DataDbContext context { get; set; }

        public GenericRepository()
        {
            context = new DataDbContext();
            //_transaction = context.Database.BeginTransaction();
        }

        public virtual IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public virtual IQueryable<T> FindAllBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

        public virtual T FirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public virtual void Add(IQueryable<T> entities)
        {
            foreach (T entity in entities.ToList())
                context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public virtual void Delete(IQueryable<T> entities)
        {
            foreach (T entity in entities.ToList())
                context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Edit(IQueryable<T> entities)
        {
            foreach (T entity in entities.ToList())
                context.Entry(entity).State = EntityState.Modified;
        }
        public virtual int Save()
        {
            try
            {
                context.SaveChanges();
                //_transaction.Commit();
            }
            catch (Exception ex)
            {
                //_transaction.Rollback();
                throw;
            }
            finally
            {
               //_transaction = context.Database.BeginTransaction();
            }

            return 0;
        }

        public virtual T Find(int id)
        {
            return context.Set<T>().Find(id);
        }

        public void Refresh(T entity)
        {
            context.Entry(entity).Reload();
        }

        public virtual int Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Count(predicate);
        }
        public virtual int Count()
        {
            return context.Set<T>().Count();
        }

        public void Dispose()
        {
            context?.Dispose();
            //_transaction.Dispose();
        }
    }
}
