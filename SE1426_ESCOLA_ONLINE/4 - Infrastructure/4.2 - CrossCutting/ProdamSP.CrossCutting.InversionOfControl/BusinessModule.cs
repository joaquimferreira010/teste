  using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

using ProdamSP.Business;
using ProdamSP.Domain.Interfaces.Business;


namespace ProdamSP.CrossCutting.InversionOfControl
{
  
    public static class BusinessCollectionExtensions
    {
        public static IServiceCollection AddBusinessLibrary(this IServiceCollection services)
        {
            services.AddTransient<ISolicitacaoMatriculaPreNatalBusiness, SolicitacaoMatriculaPreNatalBusiness>();
            services.AddTransient<IPessoaServiceBusiness, PessoaServiceBusiness>();
            services.AddTransient<IFoneticaBusiness, FoneticaBusiness>();
            services.AddTransient<ILocalidadeBusiness, LocalidadeBusiness>();
            services.AddTransient<ICaptchaBusiness, CaptchaBusiness>();
            services.AddTransient<ITransformacaoPreNatalBusiness, TransformacaoPreNatalBusiness>();
            return services;
        }
    }

}