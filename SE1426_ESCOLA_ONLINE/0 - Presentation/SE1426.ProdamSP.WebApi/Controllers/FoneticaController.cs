using ElmahCore;
using ElmahCore.LogCorporativoProdam;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProdamSP.CAC.Token.Std;
using ProdamSP.Domain.Interfaces.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.WebApi.Controllers
{
    [ApiController]
    public class FoneticaController :BaseController
    {
        private readonly ServiceErrorLog _logger;
      
        private readonly IConfiguration _configuration;
        
        private readonly IFoneticaBusiness _foneticaBusiness;

        public FoneticaController(IConfiguration configuration, ServiceErrorLog logger,           
           IFoneticaBusiness foneticaBusiness
          )
        {
            _logger = logger;            
            _configuration = configuration;
            
            _foneticaBusiness = foneticaBusiness;
        }


        [HttpGet]
        [Route("api/v{version:apiVersion}/fonetica/geracao-codigo-fonetico/{textoFonetizacao}")]
        public async Task<ActionResult<string>> Fonetizar(string textoFonetizacao)
        {
            try
            {
                string codigoFonetico = _foneticaBusiness.Fonetizar(textoFonetizacao);
                return Ok(codigoFonetico);
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex)); 
                //return new StatusCodeResult(500);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
