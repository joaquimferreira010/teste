using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Extensions.Logging;

namespace ProdamSP.CAC.Token.Std
{



    public class CacJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
      

        public CacJwtMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<CacJwtMiddleware>();            
            
 
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Obtendo o token de : " + context.Request.Path);

          

          

            await _next.Invoke(context);

            _logger.LogInformation("Fim da chamada da API.");
        }
    }
}
