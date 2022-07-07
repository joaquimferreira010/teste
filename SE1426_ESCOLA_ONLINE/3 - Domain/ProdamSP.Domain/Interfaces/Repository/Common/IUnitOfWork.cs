using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Interfaces.Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void Save();

        void Flush();
        //int? ExecuteIntQuery(string query);

        //void ExecuteCommand(string command);

        bool IsTransactionOpened();

        void Save(List<object> entitiesEntries);
    }
}
