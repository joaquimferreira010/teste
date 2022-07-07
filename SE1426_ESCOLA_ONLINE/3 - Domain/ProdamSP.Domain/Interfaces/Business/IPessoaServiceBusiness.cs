using ProdamSP.Domain.Models.Cadastro.Pessoa;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Interfaces.Business
{
    public interface IPessoaServiceBusiness
    {
        Task<List<PreNatalModel>> PesquisarAsPreNatal(string numeroCns);
        Task<List<PessoaSIGAModel>> PesquisarAsPessoaSIGA(string numeroCns);
    }
}
