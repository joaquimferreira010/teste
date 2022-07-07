using ElmahCore;
using ElmahCore.LogCorporativoProdam;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.WebApi.Controllers
{
    [ApiController]
    public class LogradouroController : BaseController
    {
        private readonly ServiceErrorLog _logger;

        private readonly IConfiguration _configuration;

        private readonly ILogradouroBusiness _logradouroBusiness;

        public LogradouroController(IConfiguration configuration, ServiceErrorLog logger,
           ILogradouroBusiness logradouroBusiness
          )
        {
            _logger = logger;
            _configuration = configuration;

            _logradouroBusiness = logradouroBusiness;
        }

        [HttpGet]
        [Route("api/v1/localidade/consultar/{CEP}")]
        public async Task<ActionResult<LocalidadeModel>> Consultar(string CEP)
        {
            try
            {
                LocalidadeModel localidade = _logradouroBusiness.ConsultarLogradouroPorCep(CEP);
                return Ok(localidade);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));

                return new StatusCodeResult(500);
            }
        }
    }
}
