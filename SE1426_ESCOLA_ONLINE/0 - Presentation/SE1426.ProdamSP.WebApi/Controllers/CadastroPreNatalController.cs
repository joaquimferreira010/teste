using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProdamSP.Domain.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using ElmahCore.LogCorporativoProdam;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Models.Cadastro.Pessoa;
using ElmahCore;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using ProdamSP.Domain.Models;
using ProdamSP.Domain.Constants;
using ProdamSP.CAC.Token.Std;

namespace SE1426.ProdamSP.WebApi.Controllers
{
    [ApiController]
    public class CadastroPreNatalController : BaseController
    {
        private readonly ServiceErrorLog _logger;
        private readonly ISolicitacaoMatriculaPreNatalRepository _solicitacaoMatriculaPreNatalRepository;
        private readonly IConfiguration _configuration;

        private readonly ISolicitacaoMatriculaPreNatalBusiness _solicitacaoMatriculaPreNatalBusiness;
        private readonly IPessoaServiceBusiness _pessoaServiceBusiness;
        private readonly ILocalidadeBusiness _localidadeBusiness;


        public CadastroPreNatalController(IConfiguration configuration, ServiceErrorLog logger,
            ISolicitacaoMatriculaPreNatalRepository solicitacaoMatriculaPreNatalRepository,
            ISolicitacaoMatriculaPreNatalBusiness solicitacaoMatriculaPreNatalBusiness,
            IPessoaServiceBusiness pessoaServiceBusiness,
            ILocalidadeBusiness localidadeBusiness
           )
        {
            _logger = logger;
            _solicitacaoMatriculaPreNatalRepository = solicitacaoMatriculaPreNatalRepository;
            _configuration = configuration;

            _solicitacaoMatriculaPreNatalBusiness = solicitacaoMatriculaPreNatalBusiness;
            _pessoaServiceBusiness = pessoaServiceBusiness;
            _localidadeBusiness = localidadeBusiness;
        }

        [HttpGet]
        [Route("api/v{version:apiVersion}/cadastro-matricula-pre-natal/pesquisa-pessoa-siga/{numeroCns}")]
        public async Task<ActionResult<List<PessoaSIGAModel>>> PesquisarAsPessoaSIGA(string numeroCns)
        {
            try
            {
                var listaPessoaSIGAModel = await _pessoaServiceBusiness.PesquisarAsPessoaSIGA(numeroCns);
                return Ok(listaPessoaSIGAModel);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                var listaPessoaSIGAModel = new List<PessoaSIGAModel>();
                PessoaSIGAModel pessoaSIGAModel = new PessoaSIGAModel();
                pessoaSIGAModel.CodRetorno = 99;
                pessoaSIGAModel.MsgRetorno = ex.Message;
                listaPessoaSIGAModel.Add(pessoaSIGAModel);

                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(listaPessoaSIGAModel);
            }
        }

        [HttpGet]
        [Route("api/v{version:apiVersion}/cadastro-matricula-pre-natal/pesquisa-pessoa-pre-natal/{numeroCns}")]
        public async Task<ActionResult<List<PreNatalModel>>> PesquisarPessoaAsPreNatal(string numeroCns)
        {
            try
            {
                var preNatalModel = await _pessoaServiceBusiness.PesquisarAsPreNatal(numeroCns);
                return Ok(preNatalModel);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                var listaPreNatalModel = new List<PreNatalModel>();
                PreNatalModel preNataModel = new PreNatalModel();
                preNataModel.CodRetorno = 99;
                preNataModel.MsgRetorno = ex.Message;
                listaPreNatalModel.Add(preNataModel);

                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(listaPreNatalModel);
            }
        }

        [HttpGet]
        [Route("api/v{version:apiVersion}/cadastro-matricula-pre-natal/pesquisa-pre-natais-eol/{numeroCns}/{numeroSisPreNatal}")]
        public async Task<ActionResult<List<PreNatalModel>>> PesquisarPreNataisEOL(string numeroCns, string numeroSisPreNatal)
        {
            try
            {
                var preNatalModel = await _solicitacaoMatriculaPreNatalBusiness.PesquisarPreNataisEOL(numeroCns,numeroSisPreNatal);
                return Ok(preNatalModel);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                var listaPreNatalModel = new List<PreNatalModel>();
                PreNatalModel preNataModel = new PreNatalModel();
                preNataModel.CodRetorno = 99;
                preNataModel.MsgRetorno = ex.Message;
                listaPreNatalModel.Add(preNataModel);

                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(listaPreNatalModel);
            }
        }


        [HttpPost, Route("api/v{version:apiVersion}/cadastro-matricula-pre-natal/inclusao-pre-natal")]
        public async Task<ActionResult<PreNatalRetornoInclusaoModel>> IncluirPreNatal(PreNatalModel preNatalModel)
        {
            try
            {
                //consulta endereço Oracle
                if (preNatalModel.Responsavel != null && preNatalModel.Responsavel.CdCep != null && preNatalModel.Responsavel.CdCep != 0)
                {
                    string cep = String.Format("{0:00000000}", preNatalModel.Responsavel.CdCep);
                    LocalidadeModel enderecoOracle = _localidadeBusiness.ConsultarLocalidadePorCep(cep);

                    if (enderecoOracle != null && !String.IsNullOrEmpty(enderecoOracle.CEP) && !enderecoOracle.CEP.Equals("0"))
                    {
                        preNatalModel.Responsavel.CdCep = Convert.ToInt32(enderecoOracle.CEP);
                        preNatalModel.Responsavel.DcTpLogradouro = enderecoOracle.codTipoLogradouroDNE;
                        preNatalModel.Responsavel.NmLogradouro = enderecoOracle.NmLogradouro;
                        preNatalModel.Responsavel.NmBairro = enderecoOracle.Bairro;
                        preNatalModel.Responsavel.NmMunicipio = enderecoOracle.Cidade;  
                    } else
                    {
                        PreNatalRetornoInclusaoModel retorno = new PreNatalRetornoInclusaoModel();
                        retorno.Retorno = "FALSE";
                        retorno.Mensagem = Mensagens.MS_PUB_CPT_012_DADOS_INCONSISTENTES + Mensagens.MS_PUB_CPT_010_CEP_NAO_ENCONTRADO;                        
                        return Ok(retorno);
                    }  

                    var objPreNatalRetornoInclusaoModel = _solicitacaoMatriculaPreNatalBusiness.InserirMaePaulistanaPreNatal(preNatalModel);
                    return Ok(objPreNatalRetornoInclusaoModel.Result);

                } else if (preNatalModel.Responsavel.CdCep == null || preNatalModel.Responsavel.CdCep == 0)
                {
                    PreNatalRetornoInclusaoModel retorno = new PreNatalRetornoInclusaoModel();
                    retorno.Retorno = "FALSE";
                    //retorno.Mensagem = Mensagens.MS_PUB_CPT_012_DADOS_INCONSISTENTES + Mensagens.MS_PUB_CPT_010_CEP_NAO_ENCONTRADO;
                    retorno.Mensagem = "O CEP é obrigatório para o registro de nascimento da criança. Por favor, atualize suas informações no SIGA";
                    return Ok(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                PreNatalRetornoInclusaoModel model = new PreNatalRetornoInclusaoModel();
                model.Retorno = "99";
                model.Mensagem = ex.InnerException.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(model);
            }
        }


        [HttpPost, Route("api/v{version:apiVersion}/cadastro-matricula-pre-natal/exclusao-pre-natal")]
        public async Task<ActionResult<PreNatalRetornoExclusaoModel>> ExcluirPreNatal(PreNatalExclusaoModel preNatalExclusaoModel)
        {
            try
            {
                var objPreNatalRetornoExclusaoModel = _solicitacaoMatriculaPreNatalBusiness.ExcluirMaePaulistanaPreNatal(preNatalExclusaoModel);
                return Ok(objPreNatalRetornoExclusaoModel.Result);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                PreNatalRetornoExclusaoModel model = new PreNatalRetornoExclusaoModel();
                model.Retorno = "99";
                model.Mensagem = ex.InnerException.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(model);
            }
        }

    }
}
