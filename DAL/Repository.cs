using System.Collections.Generic;
using System.Linq;
using DAL.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity: class
    {
        protected DbContext DbContext;

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TEntity GetSingle(long id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
            DbContext.SaveChangesAsync();
        }

        public void AddMany(List<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
            DbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            DbContext.SaveChangesAsync();
        }
        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            DbContext.SaveChangesAsync();
        }
        
    }
}