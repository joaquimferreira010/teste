using ElmahCore.LogCorporativoProdam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using ElmahCore;

namespace ProdamSP.CAC.Token.Std
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            string token;
            string ChaveAcesso;
            string Audience;
            string Issuer;

           

            TokenValidationHandler.ObterChavesDeAutenticacao(context.HttpContext, out token, out ChaveAcesso, out Audience, out Issuer);

            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            //{

            //ServiceErrorLog logger = context.HttpContext.RequestServices.GetService<ServiceErrorLog>();

           
            if (token != null)
                {
                    TokenValidationHandler _service = new TokenValidationHandler();

                    if (!_service.ValidarTokenCriaIdentity(token, ChaveAcesso, Audience, Issuer))
                    {
                    //logger.Log(new Error(new Exception("TokenAuthorizeAttribute - !_service.ValidarTokenCriaIdentity " + ChaveAcesso)));
                    //logger.Log(new Error(new Exception("TokenAuthorizeAttribute - !_service.ValidarTokenCriaIdentity " + token)));
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
                else
                {
                //logger.Log(new Error(new Exception("TokenAuthorizeAttribute - OnAuthorization token nulo")));
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            //}

        }
    }
}
