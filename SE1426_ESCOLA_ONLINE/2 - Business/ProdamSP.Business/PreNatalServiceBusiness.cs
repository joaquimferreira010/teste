using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ProdamSP.CrossCutting;
using ProdamSP.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace ProdamSP.Business
{

    public class AddPreNatalServiceSoapHeaderClientMessageInspector : IClientMessageInspector
    {
        public string LastRequestXml { get; private set; }
        public string LastResponseXml { get; private set; }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            LastRequestXml = request.ToString();

            //Fazer a cópia do pacote SOAP para visualização.
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            Message msgCopy = buffer.CreateMessage();

            request = buffer.CreateMessage();

            //Obter o conteúdo do SOAP XML.
            string strMessage = buffer.CreateMessage().ToString();

            //Obter o corpo do conteúdo do SOAP XML.
            System.Xml.XmlDictionaryReader xrdr = msgCopy.GetReaderAtBodyContents();
            string numCNS = null;
            string numSisPreNatal = null;

            while(xrdr.Read())
            {
                if(xrdr.IsStartElement("numCNS"))
                    numCNS = xrdr.ReadInnerXml();
                if (xrdr.IsStartElement("numSisPreNatal"))
                    numSisPreNatal = xrdr.ReadInnerXml();
            } 

            var xmlPayload = ChangeMessage(numCNS, numSisPreNatal);
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(xmlPayload);
            writer.Flush();
            ms.Position = 0;

            var reader = XmlReader.Create(ms);
            request = Message.CreateMessage(reader, int.MaxValue, request.Version);
            var payload = request.ToString();

            return request;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms);
            reply.WriteMessage(writer);
            writer.Flush();
            ms.Position = 0;
            doc.Load(ms);
            ms.SetLength(0);
            writer = XmlWriter.Create(ms);
            doc.WriteTo(writer);
            writer.Flush();
            ms.Position = 0;
            XmlReader reader = XmlReader.Create(ms);
            reply = Message.CreateMessage(reader, int.MaxValue, reply.Version);
        }
                
        /// Manipulate the SOAP message
        private string ChangeMessage(string numCNS, string numSisPreNatal)
        {

            var document = new XmlDocument();
            var root = document.CreateElement("s", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            root.SetAttribute("xmlns:s", "http://www.w3.org/2003/05/soap-envelope");
            root.SetAttribute("xmlns:ser", "http://service.prodam.sp.gov.br");
            root.SetAttribute("xmlns:ns2", "http://auth.smssp.atech.br");

            document.AppendChild(root);

            var sHeader = document.CreateElement("s", "Header", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(sHeader);

            var body = document.CreateElement("s", "Body", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(body);

            var replacedString = GetPayloadString(document);
            var removedString = replacedString.Replace(@"<s:Header />", "<s:Header><ns2:login>SME-SP</ns2:login><ns2:password>SME-SP!@#$</ns2:password><ns2:sistema>EOL-MPA</ns2:sistema></s:Header>");
            var removedString1 = removedString.Replace(@"<s:Body />", "<s:Body><ser:pesquisar><ser:numCNS>" + numCNS + "</ser:numCNS><ser:numSisPreNatal>" + numSisPreNatal + "</ser:numSisPreNatal></ser:pesquisar></s:Body>");

            return removedString1;
        }

        /// Helper method to get the XMLDocument format right
        private string GetPayloadString(XmlDocument document)
        {
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
            {
                document.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }

    // Endpoint behavior used to add the User-Agent HTTP Header to WCF calls for Server
    public class AddPreNatalServiceSoapHeaderEndpointBehavior : IEndpointBehavior
    {
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new AddPreNatalServiceSoapHeaderClientMessageInspector());
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class PreNatalServiceBusiness
    {
        private readonly IConfiguration _configuration;

        public PreNatalServiceBusiness(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------
        //Consulta no SIGA 
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<DadosPreNatalSIGAModelRetorno> PesquisarPreNatal(string numeroCns, string numeroSisPreNatal)
        {
            try
            {
                if (!string.IsNullOrEmpty(numeroSisPreNatal))
                {
                    //-->não considerar zeros à esquerda
                    Int64 numeroSisPreNatalInt = 0;
                    numeroSisPreNatalInt = Convert.ToInt64(numeroSisPreNatal);
                    numeroSisPreNatal = numeroSisPreNatalInt.ToString();
                    //-->
                }

                DadosPreNatalSIGAModelRetorno objDadosPreNatalSIGAModelRetorno = null;

                PreNatalService.PreNatalServicePortTypeClient.EndpointConfiguration endpoint = PreNatalService.PreNatalServicePortTypeClient.EndpointConfiguration.PreNatalServiceHttpSoap12Endpoint;

                PreNatalService.PreNatalServicePortTypeClient client = new PreNatalService.PreNatalServicePortTypeClient(endpoint);
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_configuration.GetSection("PreNatalServiceUrl").Value);

                client.Endpoint.EndpointBehaviors.Add(new AddPreNatalServiceSoapHeaderEndpointBehavior());

                PreNatalService.pesquisarRequest pesquisarRequest = new PreNatalService.pesquisarRequest();
                pesquisarRequest.numCNS = numeroCns;
                pesquisarRequest.numSisPreNatal = numeroSisPreNatal;

                PreNatalService.pesquisarResponse response = new PreNatalService.pesquisarResponse();

                using (OperationContextScope ocs = new OperationContextScope(client.InnerChannel))
                {
                    response = client.pesquisar(pesquisarRequest);

                    PreNatalService.DadosPreNatal dados = response.@return;

                    if (dados != null)
                        return convertDadosPreNatalToModelRetorno(dados, objDadosPreNatalSIGAModelRetorno);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DadosPreNatalSIGAModelRetorno convertDadosPreNatalToModelRetorno(PreNatalService.DadosPreNatal dados, DadosPreNatalSIGAModelRetorno model)
        {
            model = new DadosPreNatalSIGAModelRetorno();

            model.codRetorno = dados.codRetorno.ToInt();
            model.msgRetorno = dados.msgRetorno;
            model.dataPrevisaoParto = dados.dataPrevisaoParto;
            model.dataCadastroPreNatal = dados.dataCadastroPreNatal;
            model.dataUltimaMenstruacao = dados.dataUltimaMenstruacao;
            model.dataUltimaConsulta = dados.dataUltimaConsulta;
            model.dataParto = dados.dataPrevisaoParto;
            model.dataInterrupcao = dados.dataInterrupcao;

            return model;
        }
               
    }
}
