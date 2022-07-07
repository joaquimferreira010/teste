using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

using ProdamSP.Application;
using ProdamSP.Application.Interfaces;


namespace ProdamSP.CrossCutting.InversionOfControl
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationLibrary(this IServiceCollection services)
        {
            services.AddTransient<ISolicitacaoMatriculaPreNatalService, SolicitacaoMatriculaPreNatalService>();
            return services;
        }
    }
}
