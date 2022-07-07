using ProdamSP.Domain.Interfaces.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;

using ProdamSP.Domain.Entities;
using System.Threading.Tasks;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using ProdamSP.Domain.Models;

namespace ProdamSP.Domain.Interfaces.Repository
{
    public interface ILogradouroRepository : IRepository<SolicitacaoMatriculaPreNatal, int>
    {
        LogradouroModel ConsultarLogradouroPorCEP(string CEP);
    }
}
