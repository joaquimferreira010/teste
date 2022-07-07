using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SE1426.Internet.ProdamSP.Blazor.Infraestrutura;
using SE1426.Internet.ProdamSP.Blazor.Models;
using ElmahCore.LogCorporativoProdam;
using ElmahCore.Mvc;
using ElmahCore;

namespace SE1426.Internet.ProdamSP.Blazor.Services
{
    public class RestService
    {
        HttpClient _client;
        string _strUri;
        StateContainer _state;
        private readonly IConfiguration _configuration;
        private readonly ServiceErrorLog _logger;
        public DadosPreNatalEOLSIGAModel DadosPreNatalEOLSIGA { get; private set; }

        public List<PreNatalModel> listaPreNatalModel { get; private set; }

        public RestService(IConfiguration Configuration, StateContainer state, IConfiguration configuration, ServiceErrorLog logger)
        {
            _client = new HttpClient();
            _state = state;
            _logger = logger;
            //_client.DefaultRequestHeaders.("Access-Control-Allow-Origin", "*");
            //_client.DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "Content-Type");
            _configuration = configuration;
            _strUri = Configuration.GetSection("EndPoints:EOLAPI").Value;
        }

        public async Task<DadosPreNatalEOLSIGAModel> BuscarSolicitacaoPreNatal(string numeroCns, string numeroSisPreNatal)
        {
            if (!string.IsNullOrEmpty(numeroCns)) { numeroCns = numeroCns.Trim().Replace(" ", ""); }
            if (!string.IsNullOrEmpty(numeroSisPreNatal)) { numeroSisPreNatal = numeroSisPreNatal.Trim(); }
            DadosPreNatalEOLSIGA = new DadosPreNatalEOLSIGAModel();


            try
            {
                //_logger.Log(new Error(new Exception("inicio chamada BuscarSolicitacaoPreNatal " + string.Format(_strUri + "/api/v1/consulta-pre-natal/" + numeroCns + "/" + numeroSisPreNatal, string.Empty))));
                var uri = new Uri(string.Format(_strUri + "/api/v1/consulta-pre-natal/" + numeroCns + "/" + numeroSisPreNatal, string.Empty));
                var request = new HttpRequestMessage(HttpMethod.Get, _strUri + "/api/v1/consulta-pre-natal/" + numeroCns + "/" + numeroSisPreNatal);

                _state.Token = AuthUtil.GetToken(_configuration);

                 request.Headers.Add("Authorization", _state.Token);


                request.Headers.Add("Access-Control-Allow-Origin", "*");
                request.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                HttpResponseMessage response = await _client.SendAsync(request);


                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        //_logger.Log(new Error(new Exception("retorno chamada BuscarSolicitacaoPreNatal " + content)));
                        DadosPreNatalEOLSIGA = JsonConvert.DeserializeObject<DadosPreNatalEOLSIGAModel>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        DadosPreNatalEOLSIGA = new DadosPreNatalEOLSIGAModel { codRetorno = 99, msgRetorno = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        //_logger.Log(new Error(new Exception("retorno chamada BuscarSolicitacaoPreNatal bad request")));
                        var errorContent = await response.Content.ReadAsStringAsync();
                        DadosPreNatalEOLSIGA = JsonConvert.DeserializeObject<DadosPreNatalEOLSIGAModel>(errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        DadosPreNatalEOLSIGA = new DadosPreNatalEOLSIGAModel { codRetorno = 99, msgRetorno = "Acesso Negado. Por favor, reinicie navegação!" };
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                //throw new Exception(ex.Message);
                DadosPreNatalEOLSIGA.codRetorno = 99;
                DadosPreNatalEOLSIGA.msgRetorno = ex.Message;
            }
            return DadosPreNatalEOLSIGA;
        }



        public async Task<List<PreNatalModel>> BuscarSolicitacoesPreNatal(string numeroCns, string numeroSisPreNatal)
        {

            if (!string.IsNullOrEmpty(numeroCns)) { numeroCns = numeroCns.Trim().Replace(" ", ""); }
            if (!string.IsNullOrEmpty(numeroSisPreNatal)) { numeroSisPreNatal = numeroSisPreNatal.Trim(); }
            listaPreNatalModel = new List<PreNatalModel>();
            try
            {
               
                var uri = new Uri(string.Format(_strUri + "/api/v1/cadastro-matricula-pre-natal/pesquisa-pre-natais-eol/" + numeroCns + "/" + numeroSisPreNatal, string.Empty));
                var request = new HttpRequestMessage(HttpMethod.Get, _strUri + "/api/v1/cadastro-matricula-pre-natal/pesquisa-pre-natais-eol/" + numeroCns + "/" + numeroSisPreNatal);

                
                request.Headers.Add("Authorization", _state.Token );
                
                request.Headers.Add("Access-Control-Allow-Origin", "*");
                request.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                HttpResponseMessage response = await _client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        listaPreNatalModel = JsonConvert.DeserializeObject<List<PreNatalModel>>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        listaPreNatalModel.Add( new PreNatalModel { codRetorno = 99, msgRetorno = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" });
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        listaPreNatalModel = JsonConvert.DeserializeObject<List<PreNatalModel>>(errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        listaPreNatalModel.Add(new PreNatalModel { codRetorno = 99, msgRetorno = "Acesso Negado. Por favor, reinicie navegação!" });
                        break;
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                listaPreNatalModel.Add(new PreNatalModel { codRetorno = 99, msgRetorno = ex.Message });
            }
            return listaPreNatalModel;
        }


        public async Task<List<PreNatalModel>> PesquisarPessoaPreNatalSIGA(string numeroCns)
        {
            if (!string.IsNullOrEmpty(numeroCns)) 
                numeroCns = numeroCns.Trim().Replace(" ", "");

            string serviceURI = _strUri + "/api/v1/cadastro-matricula-pre-natal/pesquisa-pessoa-pre-natal/" + numeroCns; 
            
            listaPreNatalModel = new List<PreNatalModel>();
            try
            {
                var uri = new Uri(string.Format(serviceURI, string.Empty));
                var request = new HttpRequestMessage(HttpMethod.Get, serviceURI);

                request.Headers.Add("Authorization", _state.Token);
        

                request.Headers.Add("Access-Control-Allow-Origin", "*");
                request.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                HttpResponseMessage response = await _client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        listaPreNatalModel = JsonConvert.DeserializeObject<List<PreNatalModel>>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        listaPreNatalModel.Add( new PreNatalModel { codRetorno = 99, msgRetorno = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" });
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        listaPreNatalModel = JsonConvert.DeserializeObject<List<PreNatalModel>>(errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        listaPreNatalModel.Add(new PreNatalModel { codRetorno = 99, msgRetorno = "Acesso Negado. Por favor, reinicie navegação!" });
                        break;

                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                listaPreNatalModel.Add(new PreNatalModel { codRetorno = 99, msgRetorno = ex.Message });
            }
            return listaPreNatalModel;
        }

        public async Task<PreNatalRetornoInclusaoModel> IncluirPreNatal(PreNatalModel preNatalModel)
        {
            preNatalModel.Usuario = "webmpa";
            preNatalModel.Senha = "mpa";
            preNatalModel.MatriculaPreNatal.DcOrigemLocalAlteracao = "INTERNET";  

            var preNatalRetornoInclusaoModel = new PreNatalRetornoInclusaoModel();
            try
            {                                                                   
                var uri = new Uri(string.Format(_strUri + "/api/v1/cadastro-matricula-pre-natal/inclusao-pre-natal", string.Empty));
               
                var postBody = JsonConvert.SerializeObject(preNatalModel);

                var Content = new StringContent(postBody, Encoding.UTF8, "application/json");
                Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _state.Token);

                //if (_state.ConexaoValida)
                //{
                //}
                //Content.Headers.Add("Access-Control-Allow-Origin", "*");
                //Content.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                HttpResponseMessage response = await _client.PostAsync(uri, Content);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        preNatalRetornoInclusaoModel = JsonConvert.DeserializeObject<PreNatalRetornoInclusaoModel>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        preNatalRetornoInclusaoModel = new PreNatalRetornoInclusaoModel { Retorno = "99", Mensagem = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        preNatalRetornoInclusaoModel = JsonConvert.DeserializeObject<PreNatalRetornoInclusaoModel> (errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        preNatalRetornoInclusaoModel = new PreNatalRetornoInclusaoModel { Retorno = "99", Mensagem = "Acesso Negado. Por favor, reinicie navegação!" };
                        break;
                        //case System.Net.HttpStatusCode.BadRequest:
                        //    throw new Exception("Erro na inclusão dos dados de Pré-Natal");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                //throw new Exception(ex.Message);
                preNatalRetornoInclusaoModel.Retorno = "99";
                preNatalRetornoInclusaoModel.Mensagem = ex.Message;
            }
            return preNatalRetornoInclusaoModel;
        }


        public async Task<PreNatalRetornoExclusaoModel> ExcluirPreNatal(PreNatalExclusaoModel preNatalExclusaoModel)
        {
            preNatalExclusaoModel.Usuario = "webmpa";
            preNatalExclusaoModel.Senha = "mpa";
            preNatalExclusaoModel.CdMotivoCancelamentoInscricao = null;

            var preNatalRetornoExclusaoModel = new PreNatalRetornoExclusaoModel();
            try
            {
                var uri = new Uri(string.Format(_strUri + "/api/v1/cadastro-matricula-pre-natal/exclusao-pre-natal", string.Empty));
                var postBody = JsonConvert.SerializeObject(preNatalExclusaoModel);

                var Content = new StringContent(postBody, Encoding.UTF8, "application/json");
                Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _state.Token);

                //Content.Headers.Add("Access-Control-Allow-Origin", "*");
                //Content.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                HttpResponseMessage response = await _client.PostAsync(uri, Content);


                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        preNatalRetornoExclusaoModel = JsonConvert.DeserializeObject<PreNatalRetornoExclusaoModel>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        preNatalRetornoExclusaoModel = new PreNatalRetornoExclusaoModel { Retorno = "99", Mensagem = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        preNatalRetornoExclusaoModel = JsonConvert.DeserializeObject<PreNatalRetornoExclusaoModel>(errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        preNatalRetornoExclusaoModel = new PreNatalRetornoExclusaoModel { Retorno = "99", Mensagem = "Acesso Negado. Por favor, reinicie navegação!" };
                        break;

                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                //throw new Exception(ex.Message);
                preNatalRetornoExclusaoModel.Retorno = "99";
                preNatalRetornoExclusaoModel.Mensagem = ex.Message;
            }
            return preNatalRetornoExclusaoModel;
        }



        public async Task<TransformacaoConsultaCriancaRetornoModel> ConsultarDadosCrianca(TransformacaoConsultaCriancaModel transformacaoConsultaCriancaModel)
        {
            var transformacaoRetornoModel = new TransformacaoConsultaCriancaRetornoModel();
            try
            {
                var uri = new Uri(string.Format(_strUri + "/api/v1/transformacao-pre-natal/consultar-dados-crianca", string.Empty));
                var postBody = JsonConvert.SerializeObject(transformacaoConsultaCriancaModel);

                var Content = new StringContent(postBody, Encoding.UTF8, "application/json");
                Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _state.Token);


                HttpResponseMessage response = await _client.PostAsync(uri, Content);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        transformacaoRetornoModel = JsonConvert.DeserializeObject<TransformacaoConsultaCriancaRetornoModel>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        //transformacaoRetornoModel = new TransformacaoModel { Retorno = "99", Mensagem = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        transformacaoRetornoModel = JsonConvert.DeserializeObject<TransformacaoConsultaCriancaRetornoModel>(errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        transformacaoRetornoModel = new TransformacaoConsultaCriancaRetornoModel { CodRetorno = 99,  MsgRetorno = "Acesso Negado. Por favor, reinicie navegação!" };
                        break;

                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                //throw new Exception(ex.Message);
                transformacaoRetornoModel.CodRetorno= 99;
                transformacaoRetornoModel.MsgRetorno = ex.Message;
            }
            return transformacaoRetornoModel;
        }
        public async Task<List<PreNatalTransformacaoModel>> BuscarSolicitacoesPreNatalTransformacao(string numeroCns, string numeroSisPreNatal)
        {

            if (!string.IsNullOrEmpty(numeroCns)) { numeroCns = numeroCns.Trim().Replace(" ", ""); }
            if (!string.IsNullOrEmpty(numeroSisPreNatal)) { numeroSisPreNatal = numeroSisPreNatal.Trim(); }
            var listaPreNatalTransformacaoModel = new List<PreNatalTransformacaoModel>();
            try
            {

                var uri = new Uri(string.Format(_strUri + "/api/v1/transformacao-pre-natal/pesquisa-pre-natais-transformacao-eol/" + numeroCns + "/" + numeroSisPreNatal, string.Empty));
                var request = new HttpRequestMessage(HttpMethod.Get, _strUri + "/api/v1/transformacao-pre-natal/pesquisa-pre-natais-transformacao-eol/" + numeroCns + "/" + numeroSisPreNatal);

                request.Headers.Add("Access-Control-Allow-Origin", "*");
                request.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                request.Headers.Add("Authorization", _state.Token);


                HttpResponseMessage response = await _client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        listaPreNatalTransformacaoModel = JsonConvert.DeserializeObject<List<PreNatalTransformacaoModel>>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        listaPreNatalTransformacaoModel.Add(new PreNatalTransformacaoModel { CodRetorno = 99, MsgRetorno = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" });
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        listaPreNatalTransformacaoModel = JsonConvert.DeserializeObject<List<PreNatalTransformacaoModel>>(errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        listaPreNatalTransformacaoModel.Add(new PreNatalTransformacaoModel { CodRetorno = 99, MsgRetorno = "Acesso Negado. Por favor, reinicie navegação!" });
                        break;
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                listaPreNatalTransformacaoModel.Add(new PreNatalTransformacaoModel { CodRetorno = 99, MsgRetorno = ex.Message });
            }
            return listaPreNatalTransformacaoModel;
        }

        public async Task<SolicitacaoVagaRetornoModel> SolicitarVaga(SolicitacaoVagaModel solicitacaoVagaModel)
        {
            solicitacaoVagaModel.Usuario = "webmpa";
            solicitacaoVagaModel.Senha = "mpa";

            var solicitacaoVagaRetornoModel = new SolicitacaoVagaRetornoModel();
            try
            {
                var uri = new Uri(string.Format(_strUri + "/api/v1/transformacao-pre-natal/solicitar-vaga", string.Empty));

                var postBody = JsonConvert.SerializeObject(solicitacaoVagaModel);

                var Content = new StringContent(postBody, Encoding.UTF8, "application/json");
                Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _state.Token);

                HttpResponseMessage response = await _client.PostAsync(uri, Content);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        solicitacaoVagaRetornoModel = JsonConvert.DeserializeObject<SolicitacaoVagaRetornoModel>(content);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        solicitacaoVagaRetornoModel = new SolicitacaoVagaRetornoModel { Retorno = "99", Mensagem = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        var errorContent = await response.Content.ReadAsStringAsync();
                        solicitacaoVagaRetornoModel = JsonConvert.DeserializeObject<SolicitacaoVagaRetornoModel>(errorContent);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        solicitacaoVagaRetornoModel = new SolicitacaoVagaRetornoModel { Retorno = "99", Mensagem = "Acesso Negado. Por favor, reinicie navegação!" };
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Error(ex));
                solicitacaoVagaRetornoModel.Retorno = "99";
                solicitacaoVagaRetornoModel.Mensagem = ex.Message;
            }
            return solicitacaoVagaRetornoModel;
        }

    }
}

////teste consulta - enviar dados por post
//public async Task<DadosPreNatalEOLSIGAModel> BuscarSolicitacaoPreNatalPost(string numeroCns, string numeroSisPreNatal)

//{
//    if (!string.IsNullOrEmpty(numeroCns)) { numeroCns = numeroCns.Trim().Replace(" ", ""); }
//    if (!string.IsNullOrEmpty(numeroSisPreNatal)) { numeroSisPreNatal = numeroSisPreNatal.Trim(); }
//    DadosPreNatalEOLSIGA = new DadosPreNatalEOLSIGAModel();

//    try
//    {

//        var uri = new Uri(string.Format(_strUri + "/api/v1/consulta-pre-natal-post", string.Empty));
//        var testePost = new TestePost() { numeroCns = numeroCns, numeroSisPreNatal = numeroSisPreNatal };
//        var postBody = JsonConvert.SerializeObject(testePost);

//        var Content = new StringContent(postBody, Encoding.UTF8, "application/json");
//        Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
//        Content.Headers.Add("Access-Control-Allow-Origin", "*");
//        Content.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

//        HttpResponseMessage response = await _client.PostAsync(uri, Content);


//        switch (response.StatusCode)
//        {
//            case System.Net.HttpStatusCode.OK:
//                var content = await response.Content.ReadAsStringAsync();
//                DadosPreNatalEOLSIGA = JsonConvert.DeserializeObject<DadosPreNatalEOLSIGAModel>(content);
//                break;

//            case System.Net.HttpStatusCode.InternalServerError:
//                DadosPreNatalEOLSIGA = new DadosPreNatalEOLSIGAModel { codRetorno = 99, msgRetorno = "Serviços indisponíveis no momento. Por favor, tente mais tarde!" };
//                break;

//            case System.Net.HttpStatusCode.BadRequest:
//                throw new Exception("Erro na consulta dos dados de Pré-Natal");

//        }
//    }
//    catch (Exception ex)
//    {
//        throw new Exception(ex.Message);
//    }
//    return DadosPreNatalEOLSIGA;
//}

//public class TestePost
//{
//    public string numeroCns { get; set; }
//    public string numeroSisPreNatal { get; set; }
//}
