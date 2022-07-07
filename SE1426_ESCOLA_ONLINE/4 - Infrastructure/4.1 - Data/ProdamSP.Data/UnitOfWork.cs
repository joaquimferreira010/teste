using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Interfaces.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using ProdamSP.Data;
using ProdamSP.Data.Context;

namespace ProdamSP.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected SE1426Context _context;
        private bool disposed;
        private Dictionary<string, object> repositories;
        private IDbContextTransaction _transaction;

        public UnitOfWork(SE1426Context context)
        {
            this._context = context;
        }

        public UnitOfWork()
        {
            _context = new SE1426Context();
        }

        /// <summary>
        /// Realiza persistência das alterações do contexto somente para os registros identificados.
        /// Garante que contexto não irá persistir informações ainda não consolidadas (sem integridade referencial).
        /// Criada para atender rotina de atualização de dados que pode ser executada tanto isoladamente, como via chama de outras rotinas de atualização de dados.
        /// </summary>
        /// <param name="entityEntries">Registros de quaisquer tabelas a serem considerados na persistência do contexto</param>
        public virtual void Save(List<object> entitiesEntries)
        {
          //  _context.Sa(entitiesEntries);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Flush()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToArray())
            {
                if (entry.Entity != null)
                    entry.State = EntityState.Detached;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
            }
            disposed = true;
        }

        public void BeginTransaction()
        {
            _transaction = this._context.Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            this._transaction.Rollback();
        }

        public void CommitTransaction()
        {
            this._transaction.Commit();
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return this._transaction;
        }

        public bool IsTransactionOpened()
        {
            return this._transaction != null;
        }

        public SE1426Context GetContext()
        {
            return this._context;
        }

        /// <summary>
        /// Irá executar a query e retornar um inteiro. Serve para selecionar apenas números como: Ids, NrSequencia etc.
        /// </summary>        
        /// <returns></returns>
        //public int? ExecuteIntQuery(string query)
        //{
        //    return this._context.Database.ExecuteSqlCommand(query);
        //}

        /// <summary>
        /// Irá executar a query e retornar um valor do tipo informado. 
        /// </summary>        
        /// <returns></returns>
        //public void ExecuteCommand(string command)
        //{
        //    this._context.Database.ExecuteSqlCommand(command);
        //}
    }
}
