using System;
using System.Collections.Generic;
using System.Text;
using ProdamSP.Business.Common;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Interfaces.Repository;
using ProdamSP.Domain.Interfaces.Repository.Common;
using ProdamSP.CrossCutting;
using System.Threading.Tasks;
using System.Linq;
using ProdamSP.Domain.Constants;
using ProdamSP.Domain.Models;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using ProdamSP.CrossCutting.Services;
using Microsoft.Extensions.Configuration;

namespace ProdamSP.Business
{
    public  class CaptchaBusiness : Business<Captcha, int>, ICaptchaBusiness
    {
        private readonly IConfiguration _configuration;
        public CaptchaBusiness(IConfiguration configuration)
            : base(null)
        {
            _configuration = configuration;
        }

        public async Task<CaptchaModel> ObterCaptcha(string tipoSom)
         {

            CaptchaModel objCaptchaModelRetorno = new CaptchaModel();

            WsCaptcha.GeracaoSomSoapClient.EndpointConfiguration endpoint = WsCaptcha.GeracaoSomSoapClient.EndpointConfiguration.GeracaoSomSoap;
           
            WsCaptcha.GeracaoSomSoapClient client = new WsCaptcha.GeracaoSomSoapClient(endpoint);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_configuration.GetSection("Captcha:Url").Value);

            WsCaptcha.GeraComponentesCaptchaArquivoGeralRequest objRequest = new WsCaptcha.GeraComponentesCaptchaArquivoGeralRequest();

            objRequest.pstrCaptchaChars = "0b1c2d34f5g6hj7klmnp8qrstv9xwyz";
            objRequest.pstrCaptchaFont = "Arial";
            objRequest.pstrCaptchaLength = 4;
            objRequest.pstrCaptchaFontWarping = 3;
            objRequest.pstrTipoArquivoSom = tipoSom;

            WsCaptcha.GeraComponentesCaptchaArquivoGeralResponse objRetorno = client.GeraComponentesCaptchaArquivoGeral(objRequest);

            objCaptchaModelRetorno.Imagem64 = Convert.ToBase64String(objRetorno.pImagemValidacao);
            objCaptchaModelRetorno.Imagem = objRetorno.pImagemValidacao;
            objCaptchaModelRetorno.ExtensaoImagem = objRetorno.pstrExtensaoArquivo;
            objCaptchaModelRetorno.SomGerado64 = Convert.ToBase64String(objRetorno.pSomGerado);
            objCaptchaModelRetorno.NomeArquivoSugerido  = objRetorno.pstrNomeArquivoSugerido;
            objCaptchaModelRetorno.RetornoValidacao = objRetorno.pRetornoValidacao;
            objCaptchaModelRetorno.MensagemRetorno = objRetorno.pstrMensagemRetorno;

            return objCaptchaModelRetorno;
            
        }

        public async Task<CaptchaModel> ValidarCaptcha(string strRetornoValidacao, string strValidacao)
        {

            CaptchaModel objCaptchaModelRetorno = new CaptchaModel();

            WsCaptcha.GeracaoSomSoapClient.EndpointConfiguration endpoint = WsCaptcha.GeracaoSomSoapClient.EndpointConfiguration.GeracaoSomSoap;

            WsCaptcha.GeracaoSomSoapClient client = new WsCaptcha.GeracaoSomSoapClient(endpoint);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_configuration.GetSection("Captcha:Url").Value);
           
            WsCaptcha.ValidaTextoCaptchaRequest objRequest = new WsCaptcha.ValidaTextoCaptchaRequest();
            objRequest.pRetornoValidacao = strRetornoValidacao;
            objRequest.pstrTextoDigitado = strValidacao;

            WsCaptcha.ValidaTextoCaptchaResponse objResponse = client.ValidaTextoCaptcha(objRequest);

            objCaptchaModelRetorno.Valido = objResponse.ValidaTextoCaptchaResult;

            return objCaptchaModelRetorno;
        }
    }

}