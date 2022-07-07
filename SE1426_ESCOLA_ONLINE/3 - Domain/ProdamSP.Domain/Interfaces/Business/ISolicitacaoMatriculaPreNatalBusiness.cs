using System;
using System.Collections.Generic;
using System.Text;

using ProdamSP.Domain.Interfaces.Business.Common;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Models;
using System.Threading.Tasks;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using ProdamSP.Domain.Models.Cadastro;

namespace ProdamSP.Domain.Interfaces.Business
{
    public interface ISolicitacaoMatriculaPreNatalBusiness : IBusiness<SolicitacaoMatriculaPreNatal, int>
    {
        Task<SolicitacaoMatriculaPreNatalModelRetorno> PesquisarPreNatalEOL(string numeroCns, string numeroSISPRENATAL);

        Task<DadosPreNatalEolSigaModelRetorno> PesquisarPreNatalEOLSIGA(string numeroCns, string numeroSisPreNatal);

        Task<List<PreNatalModel>> PesquisarPreNataisEOL(string numeroCns, string numeroSisPreNatal,  int codSolicMatricula = 0);

        Task<PreNatalRetornoInclusaoModel> InserirMaePaulistanaPreNatal(PreNatalModel preNatalModel);

        Task<PreNatalRetornoExclusaoModel> ExcluirMaePaulistanaPreNatal(PreNatalExclusaoModel preNatalExclusaoModel);

        Task<StatusPreNatalRetornoModel> AtualizarStatusPreNatal(StatusPreNatalModel statusPreNatalModel);
        Task<DadosCadastraisPreNatalAtualizacaoRetornoModel> AtualizarDadosCadastraisPreNatal(DadosCadastraisPreNatalAtualizacaoModel dadosCadastraisPreNatalAtualizacaoModel);

        Task<List<PreNatalTransformacaoModel>> PesquisarPreNataisTransformacaoEOL(string numeroCns, string numeroSisPreNatal);
    }
}
