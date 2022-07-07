using ElmahCore;
using ElmahCore.LogCorporativoProdam;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProdamSP.CAC.Token.Std;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.WebApi.Controllers
{
    [ApiController]
    public class LocalidadeController : BaseController
    {
        private readonly ServiceErrorLog _logger;

        private readonly IConfiguration _configuration;

        private readonly ILocalidadeBusiness _localidadeBusiness;

        public LocalidadeController(IConfiguration configuration, ServiceErrorLog logger,
           ILocalidadeBusiness localidadeBusiness
          )
        {
            _logger = logger;
            _configuration = configuration;

            _localidadeBusiness = localidadeBusiness;
        }

        [HttpGet]
        [Route("api/v{version:apiVersion}/localidade/consultar/{CEP}")]
        public async Task<ActionResult<LocalidadeModel>> Consultar(string CEP)
        {
            try
            {
                LocalidadeModel localidade = _localidadeBusiness.ConsultarLocalidadePorCep(CEP);
                return Ok(localidade);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                LocalidadeModel model = new LocalidadeModel();
                model.CodRetorno = 99;
                model.MsgRetorno = ex.Message;
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(model);
            }
        }
    }
}
