using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SE1426.Internet.ProdamSP.Blazor.Models;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Business;
using ProdamSP.Domain;
using SE1426.Internet.ProdamSP.Blazor.Infraestrutura;
using Microsoft.AspNetCore.Http;

namespace SE1426.Internet.ProdamSP.Blazor.Services
{
    public class CaptchaService
    {
       
        private readonly ICaptchaBusiness _captchaBusiness;
        private readonly IConfiguration _configuration;

        public CaptchaService(ICaptchaBusiness captchaBusiness, IConfiguration configuration)
        {
          
            _captchaBusiness = captchaBusiness;
            _configuration = configuration;
        }


        public async Task<CaptchaModel> ObterCaptcha(CaptchaModel objCaptchaModel)

        {
            try
            {

                var objRetorno = await _captchaBusiness.ObterCaptcha(objCaptchaModel.TipoSom);

                objCaptchaModel.SrcImagem = "data:image/png;base64," + objRetorno.Imagem64;
                objCaptchaModel.SrcSom = "data:audio/mp3;base64," + objRetorno.SomGerado64;
                objCaptchaModel.ExtensaoImagem = objRetorno.ExtensaoImagem;
                objCaptchaModel.NomeArquivoSugerido = objRetorno.NomeArquivoSugerido;
                objCaptchaModel.RetornoValidacao = objRetorno.RetornoValidacao;
                objCaptchaModel.MensagemRetorno = objRetorno.MensagemRetorno;


            }
            catch (Exception ex)
            {
                objCaptchaModel = new CaptchaModel { codRetorno = 99, msgRetorno = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };

            }
            return objCaptchaModel;
        }


        public async Task<CaptchaModel> ValidarCaptcha(CaptchaModel objCaptchaModel)

        {
            try
            {
                var objRetorno = await _captchaBusiness.ValidarCaptcha(objCaptchaModel.RetornoValidacao, objCaptchaModel.TextoDigitado);

                objCaptchaModel.Valido = objRetorno.Valido;

            }
            catch (Exception ex)
            {
                objCaptchaModel = new CaptchaModel { codRetorno = 99, msgRetorno = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };

            }
            return objCaptchaModel;
        }


    }
}