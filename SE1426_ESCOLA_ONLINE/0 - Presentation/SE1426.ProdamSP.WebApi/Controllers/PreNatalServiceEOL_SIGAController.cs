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
    //[ApiVersion("1.0")]
    public class PreNatalServiceEOL_SIGAController : BaseController
    {

        private readonly ServiceErrorLog _logger;
        private readonly ISolicitacaoMatriculaPreNatalRepository _solicitacaoMatriculaPreNatalRepository;
        private readonly IConfiguration _configuration;

        private readonly ISolicitacaoMatriculaPreNatalBusiness _solicitacaoMatriculaPreNatalBusiness;


        public PreNatalServiceEOL_SIGAController(IConfiguration configuration, ServiceErrorLog logger,
            ISolicitacaoMatriculaPreNatalRepository solicitacaoMatriculaPreNatalRepository,
            ISolicitacaoMatriculaPreNatalBusiness solicitacaoMatriculaPreNatalBusiness)
        {
            _logger = logger;
            _solicitacaoMatriculaPreNatalRepository = solicitacaoMatriculaPreNatalRepository;
            _configuration = configuration;

            _solicitacaoMatriculaPreNatalBusiness = solicitacaoMatriculaPreNatalBusiness;
        }



        [HttpGet, Route("api/v{version:apiVersion}/solicitacaoMatriculaPreNatalTeste/eol/{numeroCns}/{numeroSisPreNatal}")]
        public async Task<ActionResult<SolicitacaoMatriculaPreNatalModelRetorno>> SolicitacaoMatriculaPreNatalTeste(string codigo)
        {
            try
            {

                var objRetorno = new SolicitacaoMatriculaPreNatalModelRetorno();

                objRetorno.cdOperador = "12121";
                return Ok(objRetorno);

            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                SolicitacaoMatriculaPreNatalModelRetorno model = new SolicitacaoMatriculaPreNatalModelRetorno();
                model.codRetorno = 99;
                model.msgRetorno = ex.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(model);
            }
        }


        //[HttpGet, Route("api/v{version:apiVersion}/solicitacaoMatriculaPreNatal/{numeroCns}/{numeroSisPreNatal}")]
        [HttpGet, Route("api/v{version:apiVersion}/solicitacaoMatriculaPreNatal/pesquisarPreNatalEOLSIGA/{numeroCns}/{numeroSisPreNatal}")]
        public async Task<ActionResult<DadosPreNatalEolSigaModelRetorno>> PesquisarPreNatalEOLSIGA(string numeroCns, string numeroSisPreNatal)
        {
            try
            {
                var objDadosPreNatalEolSigaModelRetorno = _solicitacaoMatriculaPreNatalBusiness.PesquisarPreNatalEOLSIGA(numeroCns, numeroSisPreNatal);
                return Ok(objDadosPreNatalEolSigaModelRetorno.Result);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                DadosPreNatalEolSigaModelRetorno model = new DadosPreNatalEolSigaModelRetorno();
                model.codRetorno = 99;
                model.msgRetorno = ex.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(model);
            }
        }

    }
}





