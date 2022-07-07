using System;
using System.Collections.Generic;
using System.Text;
using ProdamSP.Business.Common;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Interfaces.Repository;
using ProdamSP.Domain.Interfaces.Repository.Common;
using ProdamSP.CrossCutting;
using System.Threading.Tasks;
using System.Linq;
using ProdamSP.Domain.Constants;
using ProdamSP.Domain.Models;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using ProdamSP.CrossCutting.Services;
using Microsoft.Extensions.Configuration;

namespace ProdamSP.Business
{
    public class LocalidadeBusiness : Business<SolicitacaoMatriculaPreNatal, int>, ILocalidadeBusiness
    {
        private IUnitOfWork _uow;
                private readonly IConfiguration _configuration;
        public LocalidadeBusiness(IConfiguration configuration,
            IUnitOfWork uow)
            : base(null)
        {
            _configuration = configuration;
            _uow = uow;
        }

        public LocalidadeModel ConsultarLocalidadePorCep(string CEP)
        {

            var localidadeService = new LocalidadeService(_configuration["SOALocalidade:Url"],
                                                          _configuration["SOALocalidade:txSenhaSOA"],
                                                          _configuration["SOALocalidade:txSenhaCAC"],
                                                          _configuration["SOALocalidade:nomUsuarioSistema"]);

            var localidadeModel = localidadeService.BuscarEnderecoPorCEP(CEP);

            return localidadeModel;
            
        }
    }

}