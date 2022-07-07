using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;

namespace ProdamSP.CAC.Token.Std
{
    public class TokenValidationHandler : DelegatingHandler
    {
        
        private readonly ILogger Log = (new LoggerFactory()).CreateLogger<TokenValidationHandler>();

        const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
        private readonly string EnvironmentName = "Producao";

        string ChaveAcesso { get; set; }
        string Audience { get; set; }
        string Issuer { get; set; }

        string Token { get; }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        public static void ObterChavesDeAutenticacao(HttpContext context, out string token, out string chaveDeAcesso, out string audience, out string urlCACLogin)
        {

            HttpRequestMessageFeature hreqmf = new HttpRequestMessageFeature(context);
            HttpRequestMessage request = hreqmf.HttpRequestMessage;


            if (TryRetrieveToken(request, out token))
            {

                var _config = context.RequestServices.GetService<IConfiguration>();
                //var Context = this.

                //token do out ....
                chaveDeAcesso = (_config["AppConfig:CAC_ChaveAcesso"] != null) ? _config["AppConfig:CAC_ChaveAcesso"].ToString() : "";
                    urlCACLogin = (_config["AppConfig:CAC_URl_login"] != null) ? _config["AppConfig:CAC_URl_login"].ToString() : "";
                    audience = "http://" + chaveDeAcesso;

            }
            else
            {
                //token = null; ...
                chaveDeAcesso = null;
                urlCACLogin = null;
                audience = null;

            }

        }





        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

          

            HttpStatusCode statusCode;
            string mensagem = "Ok...";
            string token;
            //checa se um token jwt existe ou não
            if (!TryRetrieveToken(request, out token))
            {
                if (token == null)
                {
                    Log.LogWarning(string.Format("Não há token. Não é possível validar a requisição."));
                }
                else
                {
                    Log.LogWarning(string.Format("Não foi possível checar a validade do token {0}...", token));
                }

                statusCode = HttpStatusCode.Unauthorized;
                //permite requests sem tocken - usando com claimsauthorization attribute
                return base.SendAsync(request, cancellationToken);
            }

            
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var _config = builder.Build();


            try
            { 
                string chaveDeAcesso = (_config["AppConfig:CAC_ChaveAcesso"] != null) ? _config["AppConfig:CAC_ChaveAcesso"].ToString() : "";
                string urlCACLogin = (_config["AppConfig:CAC_URl_login"] != null) ? _config["AppConfig:CAC_URl_login"].ToString() : "";

                
               
           
                var audience = "http://" + chaveDeAcesso;
                Log.LogDebug(string.Format("Audience {0}, Issuer {1}, ChaveAcesso {2}", audience, urlCACLogin, chaveDeAcesso));

                ValidarTokenCriaIdentity(token,
                                         chaveDeAcesso,
                                         audience,
                                         urlCACLogin);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                mensagem = ex.Message + " - Obs: Verifique a URL e o tempo de expiracao do token no cadastro do CAC.";
                System.Diagnostics.Debug.WriteLine(ex.Message);
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { ReasonPhrase = mensagem });
        }


        public bool ValidarTokenCriaIdentity(string token, string ChaveAcesso, string Audience, string Issuer)
        {

            if (string.IsNullOrEmpty(ChaveAcesso))
            {
                new Exception("Não localizada a chave CAC_ChaveAcesso no web.config desta aplicação.");
            }
            if (string.IsNullOrEmpty(Audience))
            {
                new Exception("Não informado o endereço do solicitante.");
            }

            if (string.IsNullOrEmpty(Issuer))
            {
                new Exception("Não localizada a chave CAC_URl_login no .config desta aplicação.");
            }

            //Log.Info(string.Format("Paramâmetros de validação [Token:{0}, ChaveAcesso:{1}, Audience:{2}, Issuer:{3}]", token, ChaveAcesso, Audience, Issuer));

            string segredo = ChaveAcesso != null ? ChaveAcesso + sec.Substring(ChaveAcesso.Length) : sec;
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(segredo));

            SecurityToken securityToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidAudience = Audience,
                ValidIssuer = Issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = this.LifetimeValidator,
                IssuerSigningKey = securityKey,
            };


            try
            {

                //extrai e valida o usuário no jwt
                var identity = handler.ValidateToken(token, validationParameters, out securityToken);
                Thread.CurrentPrincipal = identity;
              /*  if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = identity;

                }*/


            }
            catch (Exception ex)
            {
                Log.LogError("Erro ao validar o token. " + ex.Message);
                return false;

            }


            return true;
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

    }
}
