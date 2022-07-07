using Necnat.Shared.Utils;
using ProdamSP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.CrossCutting.Services
{
    public class LocalidadeService
    {
        private string TxSenhaCAC;
        private string TxSenhaSOA;
        private string NomUsuarioSistema;
        private string URL;
        public LocalidadeService(string url, string senhaSOA, string senhaCAC, string nmUsuarioSistema)
        {
            TxSenhaCAC = senhaCAC;
            TxSenhaSOA = senhaSOA;
            URL = url;
            NomUsuarioSistema = nmUsuarioSistema;
        }

        public LocalidadeModel BuscarEnderecoPorCEP(string cep)
        {
            try
            {
                cep = Util.GetOnlyNumbers(cep);
                var localidadeRetorno = new LocalidadeModel();
                var sb = new StringBuilder();
                sb.AppendLine("<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' xmlns:loc='http://prodam.sp.gov.br/prodam/wsdl/localidade.v2'>");
                sb.AppendLine("   <soap:Header/>");
                sb.AppendLine("   <soap:Body>");
                sb.AppendLine("      <loc:listarEnderecosPorCEPCompleta.v1.0.Entrada.Mensagem>");
                sb.AppendLine("         <codCEP>" + cep + "</codCEP>");
                sb.AppendLine("      </loc:listarEnderecosPorCEPCompleta.v1.0.Entrada.Mensagem>");
                sb.AppendLine("   </soap:Body>");
                sb.AppendLine("</soap:Envelope>");

                var content = new StringContent(sb.ToString());
                var contentType = new MediaTypeHeaderValue("application/soap+xml");
                contentType.CharSet = "UTF-8";
                contentType.Parameters.Add(new NameValueHeaderValue("action", "\"http://prodam.sp.gov.br/prodam/wsdl/localidade.v2/listarEnderecosPorCEPCompleta.v1.0\""));
                content.Headers.ContentType = contentType;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("txtSenhaCAC", TxSenhaCAC);
                client.DefaultRequestHeaders.Add("txtSenhaSOA", TxSenhaSOA);
                client.DefaultRequestHeaders.Add("nomUsuarioSistema", NomUsuarioSistema);

                var httpResponseMessage = client.PostAsync(URL, content);
                var response = httpResponseMessage.Result.Content.ReadAsStringAsync();
                var localidade = XmlParseUtil.ParseXML<Envelope>(response.Result).Body?.listarEnderecosPorCEPCompletav10RetornoMensagem;
                if (localidade != null)
                {
                    localidadeRetorno.CEP = localidade.codCEP.ToString();
                    localidadeRetorno.Bairro = localidade.nomBairro;
                    localidadeRetorno.Cidade = localidade.nomeCidade;
                    localidadeRetorno.NmLogradouro = localidade.nomLogradouro;
                    localidadeRetorno.UF = localidade.sglUF;
                    localidadeRetorno.codTipoLogradouro = localidade.codTipoLogradorouro;
                    localidadeRetorno.codTipoLogradouroDNE = localidade.codTipoLogradorouroDNE;
                }
                else
                {
                    throw new Exception("Problemas com servico Localidade com Status Code " + httpResponseMessage.Result.StatusCode);

                }
                return localidadeRetorno;
            }            
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
