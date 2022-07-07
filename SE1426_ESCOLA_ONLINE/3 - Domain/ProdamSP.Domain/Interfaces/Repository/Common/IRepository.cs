using ProdamSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProdamSP.Domain.Interfaces.Repository.Common
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity Get(TKey id);
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        long Count(Expression<Func<TEntity, bool>> predicate);
        void SaveAll();
        void Dispose();
        void AddAll(IEnumerable<TEntity> obj);
        void DeleteAll(IEnumerable<TEntity> obj);
        int ObterMaxNumeroSequencia(Expression<Func<TEntity, int?>> expression);
    }
}