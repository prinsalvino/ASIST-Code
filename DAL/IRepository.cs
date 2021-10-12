using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ASIST.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetSingle(long id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void AddMany(List<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}