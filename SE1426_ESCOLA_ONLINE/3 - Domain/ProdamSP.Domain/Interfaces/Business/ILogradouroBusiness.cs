using ProdamSP.Domain.Models;
using ProdamSP.Domain.Models.Cadastro.Pessoa;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Interfaces.Business
{
    public interface ILogradouroBusiness
    {
        LocalidadeModel ConsultarLogradouroPorCep(string CEP);
    }
}
