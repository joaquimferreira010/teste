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
using ProdamSP.Domain.Models.Cadastro;
using ProdamSP.CAC.Token.Std;

namespace SE1426.ProdamSP.WebApi.Controllers
{
    [ApiController]
    public class TransformacaoPreNatalController : BaseController
    {
        private readonly ServiceErrorLog _logger;
        private readonly IConfiguration _configuration;
        private readonly ITransformacaoPreNatalBusiness _transformacaoPreNatalBusiness;
        private readonly ISolicitacaoMatriculaPreNatalRepository _solicitacaoMatriculaPreNatalRepository;
        private readonly ISolicitacaoMatriculaPreNatalBusiness _solicitacaoMatriculaPreNatalBusiness;
        private readonly ILocalidadeBusiness _localidadeBusiness;

        public TransformacaoPreNatalController(IConfiguration configuration, ServiceErrorLog logger,
           ITransformacaoPreNatalBusiness transformacaoPreNatalBusiness,
           ISolicitacaoMatriculaPreNatalRepository solicitacaoMatriculaPreNatalRepository,
           ISolicitacaoMatriculaPreNatalBusiness solicitacaoMatriculaPreNatalBusiness,
           ILocalidadeBusiness localidadeBusiness
          )
        {
            _logger = logger;
            _configuration = configuration;

            _transformacaoPreNatalBusiness = transformacaoPreNatalBusiness;
            _solicitacaoMatriculaPreNatalRepository = solicitacaoMatriculaPreNatalRepository;
            _solicitacaoMatriculaPreNatalBusiness = solicitacaoMatriculaPreNatalBusiness;
            _localidadeBusiness = localidadeBusiness;
        }

        [HttpPost, Route("api/v{version:apiVersion}/transformacao-pre-natal/consultar-dados-crianca")]
        public async Task<ActionResult<TransformacaoConsultaCriancaRetornoModel>> ConsultarDadosCriancaSIGA(TransformacaoConsultaCriancaModel transformacaoConsultaCriancaModel)
        {
            try
            {
                var transformacaoConsultaCriancaRetornoModel = await _transformacaoPreNatalBusiness.ConsultarDadosCriancaSIGA(transformacaoConsultaCriancaModel);
                return Ok(transformacaoConsultaCriancaRetornoModel);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                var transformacaoConsultaCriancaRetornoModel = new TransformacaoConsultaCriancaRetornoModel();
                transformacaoConsultaCriancaRetornoModel.CodRetorno = 99;
                transformacaoConsultaCriancaRetornoModel.MsgRetorno = ex.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(transformacaoConsultaCriancaRetornoModel);
            }
        }

        [HttpGet]
        [Route("api/v{version:apiVersion}/transformacao-pre-natal/pesquisa-pre-natais-transformacao-eol/{numeroCns}/{numeroSisPreNatal}")]
        public async Task<ActionResult<List<PreNatalModel>>> PesquisarPreNataisTransformacaoEOL(string numeroCns, string numeroSisPreNatal)
        {
            try
            {
                var preNatalTransformacaoModel = await _solicitacaoMatriculaPreNatalBusiness.PesquisarPreNataisTransformacaoEOL(numeroCns, numeroSisPreNatal);
                return Ok(preNatalTransformacaoModel);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                var listaPreNatalTransformacaoModel = new List<PreNatalTransformacaoModel>();
                PreNatalTransformacaoModel preNatalTransformacaoModel = new PreNatalTransformacaoModel();
                preNatalTransformacaoModel.CodRetorno = 99;
                preNatalTransformacaoModel.MsgRetorno = ex.Message;
                listaPreNatalTransformacaoModel.Add(preNatalTransformacaoModel);

                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(listaPreNatalTransformacaoModel);
            }
        }


        [HttpPost, Route("api/v{version:apiVersion}/transformacao-pre-natal/solicitar-vaga")]
        public async Task<ActionResult<SolicitacaoVagaRetornoModel>> SolicitarVaga(SolicitacaoVagaModel solicitacaoVagaModel)
        {
            try
            {
                //consulta endereço Oracle - cep da mae
                if (solicitacaoVagaModel != null && solicitacaoVagaModel.DadosMae.CdCep != null && solicitacaoVagaModel.DadosMae.CdCep != 0)
                {
                    string cep = String.Format("{0:00000000}", solicitacaoVagaModel.DadosMae.CdCep);
                    LocalidadeModel enderecoOracle = _localidadeBusiness.ConsultarLocalidadePorCep(cep);

                    if (enderecoOracle != null && !String.IsNullOrEmpty(enderecoOracle.CEP) && !enderecoOracle.CEP.Equals("0"))
                    {
                        solicitacaoVagaModel.DadosMae.CdCep = Convert.ToInt32(enderecoOracle.CEP);
                        solicitacaoVagaModel.DadosMae.DcTpLogradouro = enderecoOracle.codTipoLogradouroDNE;
                        solicitacaoVagaModel.DadosMae.NmLogradouro = enderecoOracle.NmLogradouro;
                        solicitacaoVagaModel.DadosMae.NmBairro = enderecoOracle.Bairro;
                        solicitacaoVagaModel.DadosMae.NmMunicipio = enderecoOracle.Cidade;
                    }
                    else
                    {
                        SolicitacaoVagaRetornoModel retorno = new SolicitacaoVagaRetornoModel();
                        retorno.Retorno = "FALSE";
                        retorno.Mensagem = Mensagens.MS_PUB_CPT_012_DADOS_INCONSISTENTES + Mensagens.MS_PUB_CPT_010_CEP_NAO_ENCONTRADO;
                        return Ok(retorno);
                    }
                }
                else if (solicitacaoVagaModel.DadosMae.CdCep == null || solicitacaoVagaModel.DadosMae.CdCep == 0)
                {
                    SolicitacaoVagaRetornoModel retorno = new SolicitacaoVagaRetornoModel();
                    retorno.Retorno = "FALSE";
                    retorno.Mensagem = "O CEP é obrigatório para a solicitação da vaga. Por favor, atualize suas informações no SIGA";
                    return Ok(retorno);
                }


                //consulta endereço Oracle - cep da crianca
                if (solicitacaoVagaModel != null && solicitacaoVagaModel.DadosCrianca.CdCepCrianca != null && solicitacaoVagaModel.DadosCrianca.CdCepCrianca != 0)
                {
                    string cep = String.Format("{0:00000000}", solicitacaoVagaModel.DadosCrianca.CdCepCrianca);
                    LocalidadeModel enderecoOracle = _localidadeBusiness.ConsultarLocalidadePorCep(cep);

                    if (enderecoOracle != null && !String.IsNullOrEmpty(enderecoOracle.CEP) && !enderecoOracle.CEP.Equals("0"))
                    {
                        solicitacaoVagaModel.DadosCrianca.CdCepCrianca = Convert.ToInt32(enderecoOracle.CEP); ;
                        solicitacaoVagaModel.DadosCrianca.DcTpLogradouroCrianca = enderecoOracle.codTipoLogradouroDNE;
                        solicitacaoVagaModel.DadosCrianca.NmLogradouroCrianca = enderecoOracle.NmLogradouro;
                        solicitacaoVagaModel.DadosCrianca.NmBairroCrianca = enderecoOracle.Bairro;
                        solicitacaoVagaModel.DadosCrianca.NmMunicipioCrianca = enderecoOracle.Cidade;
                    }
                    else
                    {
                        SolicitacaoVagaRetornoModel retorno = new SolicitacaoVagaRetornoModel();
                        retorno.Retorno = "FALSE";
                        retorno.Mensagem = Mensagens.MS_PUB_CPT_012_DADOS_INCONSISTENTES + Mensagens.MS_PUB_CPT_010_CEP_NAO_ENCONTRADO;
                        return Ok(retorno);
                    }
                }
                else if (solicitacaoVagaModel.DadosCrianca.CdCepCrianca == null || solicitacaoVagaModel.DadosCrianca.CdCepCrianca == 0)
                {
                    SolicitacaoVagaRetornoModel retorno = new SolicitacaoVagaRetornoModel();
                    retorno.Retorno = "FALSE";
                    retorno.Mensagem = "O CEP é obrigatório para a solicitação da vaga. Por favor, atualize suas informações no SIGA";
                    return Ok(retorno);
                }

                var solicitacaoVagaRetornoModel = await _transformacaoPreNatalBusiness.SolicitarVaga(solicitacaoVagaModel);
                return Ok(solicitacaoVagaRetornoModel);
                
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                SolicitacaoVagaRetornoModel retorno = new SolicitacaoVagaRetornoModel();
                retorno.Retorno = "FALSE";
                retorno.Mensagem = (ex.InnerException==null?ex.Message: ex.InnerException.Message);
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(retorno);
            }

        }
    }
}
