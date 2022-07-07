using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ProdamSP.Domain.Validation;

namespace ProdamSP.Domain.Interfaces.Business.Common
{
    public interface IBusiness<TEntity, TKey>
    {
        TEntity Get(TKey id);
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        ValidationResult Add(TEntity entity);
        ValidationResult Update(TEntity entity);
        ValidationResult Delete(TEntity entity);
        void DeleteAll(IEnumerable<TEntity> obj);
        void AddAll(IEnumerable<TEntity> obj);
        void SaveAll();
    }
}