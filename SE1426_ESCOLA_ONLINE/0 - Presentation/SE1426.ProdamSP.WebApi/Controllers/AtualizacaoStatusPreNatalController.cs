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
    
    public class AtualizacaoStatusPreNatalController : BaseController
    {

        private readonly ServiceErrorLog _logger;
        private readonly ISolicitacaoMatriculaPreNatalRepository _solicitacaoMatriculaPreNatalRepository;
        private readonly IConfiguration _configuration;

        private readonly ISolicitacaoMatriculaPreNatalBusiness _solicitacaoMatriculaPreNatalBusiness;


        public AtualizacaoStatusPreNatalController(IConfiguration configuration, ServiceErrorLog logger,
            ISolicitacaoMatriculaPreNatalRepository solicitacaoMatriculaPreNatalRepository,
            ISolicitacaoMatriculaPreNatalBusiness solicitacaoMatriculaPreNatalBusiness)
        {
            _logger = logger;
            _solicitacaoMatriculaPreNatalRepository = solicitacaoMatriculaPreNatalRepository;
            _configuration = configuration;

            _solicitacaoMatriculaPreNatalBusiness = solicitacaoMatriculaPreNatalBusiness;
        }


        [HttpPost, Route("api/v{version:apiVersion}/atualizar-status-pre-natal")]
        public async Task<ActionResult<StatusPreNatalRetornoModel>> AtualizarStatusPreNatal(StatusPreNatalModel statusPreNatalModel)
        {
            try
            {
                var objStatusPreNatalRetornoModel = _solicitacaoMatriculaPreNatalBusiness.AtualizarStatusPreNatal(statusPreNatalModel);
                return Ok(objStatusPreNatalRetornoModel.Result);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                StatusPreNatalRetornoModel model = new StatusPreNatalRetornoModel();
                model.Retorno = "99";
                model.Mensagem = ex.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(model);
            }
        }
    }

}



