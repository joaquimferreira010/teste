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
    public interface ISolicitacaoMatriculaPreNatalRepository : IRepository<SolicitacaoMatriculaPreNatal, int>
    {
        Task<SolicitacaoMatriculaPreNatal> ConsultarSolicitacao(string numeroCns, string numeroSisPreNatal);
        Task<List<SolicitacaoMatriculaPreNatal>> ConsultarSolicitacoes(string numeroCns, string numeroSisPreNatal, int codSolicMatricula = 0);       
        Task<PreNatalRetornoInclusaoModel> InserirMaePaulistanaPreNatal(PreNatalModel preNatalModel);
        Task<PreNatalRetornoExclusaoModel> ExcluirMaePaulistanaPreNatal(PreNatalExclusaoModel preNatalExclusaoModel);
        Task<StatusPreNatalRetornoModel> AtualizarStatusMaePaulistanaPreNatal(StatusPreNatalModel statusPreNatalModel);
        Task<DadosCadastraisPreNatalAtualizacaoRetornoModel> AtualizarDadosCadastraisPreNatal(DadosCadastraisPreNatalAtualizacaoModel dadosCadastraisPreNatalAtualizacaoModel);
        Task<SolicitacaoVagaRetornoModel> SolicitarVaga(SolicitacaoVagaModel solicitacaoVagaRetornoModel);
        Task<string> ConsultarParametroGeral(string nome_parametro);
    }
}
