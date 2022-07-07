using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

using ProdamSP.Data;
using ProdamSP.Data.RepositoryImpl;
using ProdamSP.Domain.Interfaces.Repository;
using ProdamSP.Domain.Interfaces.Repository.Common;

namespace ProdamSP.CrossCutting.InversionOfControl
{

    public static class RepositoryCollectionExtensions
    {
        public static IServiceCollection AddRepositoryLibrary(this IServiceCollection services)
        {
            services.AddTransient<ISolicitacaoMatriculaPreNatalRepository, SolicitacaoMatriculaPreNatalRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }

}