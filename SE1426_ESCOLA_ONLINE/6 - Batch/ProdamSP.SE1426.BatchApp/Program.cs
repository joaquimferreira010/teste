using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;

using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;


using ProdamSP.CrossCutting.InversionOfControl;
using ProdamSP.Data;
using ProdamSP.SE1426.BatchApp.Jobs;
using ProdamSP.SE1426.BatchApp.Extensions;
using ProdamSP.Domain.Entities.Options;
using ProdamSP.Data.Context;

namespace ProdamSP.SE1426.BatchApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var isNotDebugging = !(Debugger.IsAttached || args.Contains("--console"));

          
            var hostBuilder = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, args[0]); 
            });

            if (isNotDebugging)
            {
                await hostBuilder.RunTheServiceAsync();
            }
            else
            {
                await hostBuilder.RunConsoleAsync();
            }

         
        }

        private static void ConfigureServices(IServiceCollection services, string strAmbiente)
        {
           
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{strAmbiente}.json", true, true)
                .Build();

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

           
            services.AddDbContext<SE1426Context>(options =>
            {
                options.UseSqlServer(configuration.GetSection("ConnectionStrings:EOLConnection").Value);
                options.EnableSensitiveDataLogging(true);
            });


            services.Configure<ConfiguracoesSE1426.BatchAccessOptions>(configuration.GetSection("ConfiguracoesSE1426"));

            services.AddOptions();
       

            services.AddApplicationLibrary();
            services.AddBusinessLibrary();
            services.AddRepositoryLibrary();

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzJobRunner>();
            services.AddHostedService<QuartzHostedService>();

             services.AddScoped<TesteJob>();            


            services.AddSingleton(new JobSchedule(
                jobType: typeof(TesteJob),
                cronExpression: configuration.GetSection("Quartz:TesteJob").Value)); 
        }


    }
}
