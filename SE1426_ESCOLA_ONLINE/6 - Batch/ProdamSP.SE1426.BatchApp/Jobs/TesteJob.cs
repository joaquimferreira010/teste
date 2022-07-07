using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ProdamSP.SE1426.BatchApp.Jobs
{
    public class TesteJob : IJob
    {
     
        private readonly Guid _guid;

        public TesteJob()
        {

            _guid = Guid.NewGuid();
        }

        public Task Execute(IJobExecutionContext context)
        {
            Log.Information("The job GUID is: " + _guid);
            return Task.CompletedTask;
        }
    }
}
