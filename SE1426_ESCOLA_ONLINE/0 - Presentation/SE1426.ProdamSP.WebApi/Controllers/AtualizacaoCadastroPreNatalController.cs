using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProdamSP.Domain.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using ProdamSP.Domain.Constants;
using ElmahCore.LogCorporativoProdam;
using ElmahCore;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Models;
using ProdamSP.CAC.Token.Std;

namespace SE1426.ProdamSP.WebApi.Controllers
{
    [ApiController]
    
    public class AtualizacaoCadastroPreNatalController : BaseController
    {

        private readonly ServiceErrorLog _logger;
        private readonly ISolicitacaoMatriculaPreNatalRepository _solicitacaoMatriculaPreNatalRepository;
        private readonly IConfiguration _configuration;

        private readonly ISolicitacaoMatriculaPreNatalBusiness _solicitacaoMatriculaPreNatalBusiness;
        private readonly ILocalidadeBusiness _localidadeBusiness;

        public AtualizacaoCadastroPreNatalController(IConfiguration configuration, ServiceErrorLog logger,
            ISolicitacaoMatriculaPreNatalRepository solicitacaoMatriculaPreNatalRepository,
            ISolicitacaoMatriculaPreNatalBusiness solicitacaoMatriculaPreNatalBusiness,
            ILocalidadeBusiness localidadeBusiness)
        {
            _logger = logger;
            _solicitacaoMatriculaPreNatalRepository = solicitacaoMatriculaPreNatalRepository;
            _configuration = configuration;

            _solicitacaoMatriculaPreNatalBusiness = solicitacaoMatriculaPreNatalBusiness;
            _localidadeBusiness = localidadeBusiness;
        }


        [HttpPost, Route("api/v{version:apiVersion}/atualizar-cadastro-pre-natal")]
        public async Task<ActionResult<DadosCadastraisPreNatalAtualizacaoRetornoModel>> AtualizarDadosCadastraisPreNatal(DadosCadastraisPreNatalAtualizacaoModel dadosCadastraisPreNatalAtualizacaoModel)
        {
            try
            {
                //consulta endereço Oracle
                if (dadosCadastraisPreNatalAtualizacaoModel.CdCep != null && dadosCadastraisPreNatalAtualizacaoModel.CdCep != 0)
                {
                    string cep = String.Format("{0:00000000}", dadosCadastraisPreNatalAtualizacaoModel.CdCep);
                    LocalidadeModel enderecoOracle = _localidadeBusiness.ConsultarLocalidadePorCep(cep);

                    if (enderecoOracle != null && !String.IsNullOrEmpty(enderecoOracle.CEP) && !enderecoOracle.CEP.Equals("0"))
                    {
                        dadosCadastraisPreNatalAtualizacaoModel.CdCep = Convert.ToInt32(enderecoOracle.CEP);
                        dadosCadastraisPreNatalAtualizacaoModel.DcTpLogradouro = enderecoOracle.codTipoLogradouroDNE;
                        dadosCadastraisPreNatalAtualizacaoModel.NmLogradouro = enderecoOracle.NmLogradouro;
                        dadosCadastraisPreNatalAtualizacaoModel.NmBairro = enderecoOracle.Bairro;
                        dadosCadastraisPreNatalAtualizacaoModel.NmMunicipio = enderecoOracle.Cidade;
                    }
                    else
                    {
                        DadosCadastraisPreNatalAtualizacaoRetornoModel retorno = new DadosCadastraisPreNatalAtualizacaoRetornoModel();
                        retorno.Retorno = "FALSE";
                        retorno.Mensagem = Mensagens.MS_PUB_CPT_012_DADOS_INCONSISTENTES + Mensagens.MS_PUB_CPT_010_CEP_NAO_ENCONTRADO;
                        return Ok(retorno);
                    }

                    var objDadosCadastraisPreNatalAtualizacaoRetornoModel = _solicitacaoMatriculaPreNatalBusiness.AtualizarDadosCadastraisPreNatal(dadosCadastraisPreNatalAtualizacaoModel);
                    return Ok(objDadosCadastraisPreNatalAtualizacaoRetornoModel.Result);

                }
                else if (dadosCadastraisPreNatalAtualizacaoModel.CdCep == null || dadosCadastraisPreNatalAtualizacaoModel.CdCep == 0)
                {
                    DadosCadastraisPreNatalAtualizacaoRetornoModel retorno = new DadosCadastraisPreNatalAtualizacaoRetornoModel();
                    retorno.Retorno = "FALSE";
                    retorno.Mensagem = "O CEP é obrigatório na atualização cadastral do pré-natal. Por favor, atualize suas informações no SIGA";
                    return Ok(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                DadosCadastraisPreNatalAtualizacaoRetornoModel model = new DadosCadastraisPreNatalAtualizacaoRetornoModel();
                model.Retorno = "99";
                model.Mensagem = ex.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(model);
            }
        }
    }

}



