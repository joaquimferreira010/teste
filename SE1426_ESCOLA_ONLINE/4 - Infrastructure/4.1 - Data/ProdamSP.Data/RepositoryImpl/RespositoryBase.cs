using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using ProdamSP.Data;
using ProdamSP.Data.Context;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Interfaces.Repository.Common;


namespace ProdamSP.Data.RepositoryImpl
{
    public class RepositoryBase<TEntity, TKey> : IDisposable, IRepository<TEntity, TKey> where TEntity : class
    {
        internal readonly SE1426Context _context = null;
        private readonly IUnitOfWork _uow;
        private DbSet<TEntity> _entities;
        string errorMessage = string.Empty;
        private bool disposed;

        //public RepositoryBase(UL0108Entities context)
        //{
        //    this._context = context;
        //    _entities = _context.Set<TEntity>();
        //}

        public RepositoryBase(IUnitOfWork uow)
        {
            this._context = ((UnitOfWork)uow).GetContext();
            _entities = _context.Set<TEntity>();
            this._uow = uow;
        }

        public void Add(TEntity obj)
        {
            //estado anterior
            var x = _context.Entry(obj).State;

            _entities.Add(obj);

            //estado posterior
            var b = _context.Entry(obj).State;
           _context.SaveChanges();
        }


        public void Update(TEntity obj)
        {
            var x = _context.Entry(obj).State;
            
            _entities.Update(obj);

            var b = _context.Entry(obj).State;

            _context.SaveChanges() ;
        }

        public void SaveAll()
        {
            /*
             * Obtem as mudanças das entradas
            var changes = from e in _context.ChangeTracker.Entries()
                          select e;
            */
            _uow.Save();
            //try
            //{
            //    bool isDetached = _context.Entry(obj).State == EntityState.Detached;
            //    if (isDetached)
            //        this.Add(obj);
            //    else
            //        this.Update(obj);                
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public void Delete(TEntity obj)
        {
            var x = _context.Entry(obj).State;

            _entities.Remove(obj);

            var b = _context.Entry(obj).State;

            _context.SaveChanges();
            //((IObjectContextAdapter)_context).ObjectContext.SaveChanges();
        }

        public virtual TEntity Get(TKey id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void DeleteAll(IEnumerable<TEntity> obj)
        {
            _entities.RemoveRange(obj);            
        }
        public void AddAll(IEnumerable<TEntity> obj)
        {
            _entities.AddRange(obj);            
        }

        public IEnumerable<TEntity> All()
        {
            return _entities.ToList();

        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return _entities.Where(predicate);
        }


        public long Count(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return _entities.Where(predicate).Count();
        }
        public int ObterMaxNumeroSequencia(Expression<Func<TEntity, int?>> expression)
        {
            int? consultaMax = _context.Set<TEntity>().Max(expression) ?? 0;

            return consultaMax.Value;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this._uow.Dispose();
                }
            }
            disposed = true;
        }
    }
}
