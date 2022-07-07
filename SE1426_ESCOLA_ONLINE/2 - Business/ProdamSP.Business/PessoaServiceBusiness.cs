using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using System.Xml;
using ProdamSP.Domain.Constants;
using ProdamSP.Domain.Enums;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Models.Cadastro.Pessoa;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using Microsoft.Extensions.Configuration;

namespace ProdamSP.Business
{

    public class AddPessoaServiceSoapHeaderClientMessageInspector : IClientMessageInspector
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
         
            while (xrdr.Read())
            {
                if (xrdr.IsStartElement("numeroCns"))
                    numCNS = xrdr.ReadInnerXml();                
            }

            var xmlPayload = ChangeMessage(numCNS);
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
        private string ChangeMessage(string numCNS)
        {

            var document = new XmlDocument();
            var root = document.CreateElement("s", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            root.SetAttribute("xmlns:s", "http://www.w3.org/2003/05/soap-envelope");
            root.SetAttribute("xmlns:ser", "http://service.smssp.fundacao.br");
            root.SetAttribute("xmlns:ns2", "http://auth.smssp.atech.br");

            document.AppendChild(root);


            var sHeader = document.CreateElement("s", "Header", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(sHeader);

            var body = document.CreateElement("s", "Body", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(body);



            var replacedString = GetPayloadString(document);
            var removedString = replacedString.Replace(@"<s:Header />", "<s:Header><ns2:login>SME-SP</ns2:login><ns2:password>SME-SP!@#$</ns2:password><ns2:sistema>EOL-MPA</ns2:sistema></s:Header>");
            var removedString1 = removedString.Replace(@"<s:Body />", "<s:Body><ser:pesquisar><ser:numeroCns>" + numCNS + "</ser:numeroCns></ser:pesquisar></s:Body>");

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
    public class AddPessoaServiceSoapHeaderEndpointBehavior : IEndpointBehavior
    {
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new AddPessoaServiceSoapHeaderClientMessageInspector());
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

    public class PessoaServiceBusiness : IPessoaServiceBusiness
    {
        private readonly IConfiguration _configuration;

        public PessoaServiceBusiness(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<PreNatalModel>> PesquisarAsPreNatal(string numeroCns)
        {
            List<PessoaService.Pessoa> pessoaList = null;
            List<PreNatalModel> preNatalList = null;

            try
            {
                PessoaService.PessoaServicePortTypeClient.EndpointConfiguration endpoint = PessoaService.PessoaServicePortTypeClient.EndpointConfiguration.PessoaServiceHttpSoap12Endpoint;

                PessoaService.PessoaServicePortTypeClient client = new PessoaService.PessoaServicePortTypeClient(endpoint);
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_configuration.GetSection("PessoaServiceUrl").Value);

                client.Endpoint.EndpointBehaviors.Add(new AddPessoaServiceSoapHeaderEndpointBehavior());

                PessoaService.pesquisarRequest pesquisarRequest = new PessoaService.pesquisarRequest();
                pesquisarRequest.numeroCns = numeroCns;

                PessoaService.pesquisarResponse response = new PessoaService.pesquisarResponse();

                using (OperationContextScope ocs = new OperationContextScope(client.InnerChannel))
                {
                    response = client.pesquisar(pesquisarRequest);

                    if (response.@return != null)
                    {
                        pessoaList = new List<PessoaService.Pessoa>();
                        pessoaList.Add(response.@return);

                        preNatalList = pessoaList.ConvertAll(new Converter<PessoaService.Pessoa, PreNatalModel>(PessoaServiceToPreNatal));

                        if (preNatalList.Count > 0)
                            return preNatalList;
                        else
                            return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public async Task<List<PessoaSIGAModel>> PesquisarAsPessoaSIGA(string numeroCns)
        {
            List<PessoaService.Pessoa> pessoaList = null;
            List<PessoaSIGAModel> pessoaSIGAList = null;

            try
            {
                PessoaService.PessoaServicePortTypeClient.EndpointConfiguration endpoint = PessoaService.PessoaServicePortTypeClient.EndpointConfiguration.PessoaServiceHttpSoap12Endpoint;

                PessoaService.PessoaServicePortTypeClient client = new PessoaService.PessoaServicePortTypeClient(endpoint);
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_configuration.GetSection("PessoaServiceUrl").Value);

                client.Endpoint.EndpointBehaviors.Add(new AddPessoaServiceSoapHeaderEndpointBehavior());                 
                
                PessoaService.pesquisarRequest pesquisarRequest = new PessoaService.pesquisarRequest();
                pesquisarRequest.numeroCns = numeroCns;               

                PessoaService.pesquisarResponse response = new PessoaService.pesquisarResponse();
               
                using (OperationContextScope ocs = new OperationContextScope(client.InnerChannel))
                {                       
                    response = client.pesquisar(pesquisarRequest);

                    if (response.@return != null) {
                        pessoaList = new List<PessoaService.Pessoa>();
                        pessoaList.Add(response.@return);
                                
                        pessoaSIGAList = pessoaList.ConvertAll(new Converter<PessoaService.Pessoa, PessoaSIGAModel>(PessoaServiceToPessoaSIGA));

                        if (pessoaSIGAList.Count > 0)
                            return pessoaSIGAList;
                        else
                            return null;
                    }  
                } 
            }
            catch(Exception e)
            {
                throw e;
            }

            return null;
        }

        private PessoaSIGAModel PessoaServiceToPessoaSIGA(PessoaService.Pessoa pessoa)
        {
            PessoaSIGAModel pessoaSIGA = new PessoaSIGAModel();

            pessoaSIGA.CertidaoAdministrativaIndio = preencherCertidaoModel(pessoaSIGA.CertidaoAdministrativaIndio, pessoa.certidaoAdministrativaIndio);
            pessoaSIGA.CertidaoCasamento = preencherCertidaoModel(pessoaSIGA.CertidaoCasamento, pessoa.certidaoCasamento);
            pessoaSIGA.CertidaoDivorcio = preencherCertidaoModel(pessoaSIGA.CertidaoDivorcio, pessoa.certidaoDivorcio);
            pessoaSIGA.CertidaoNascimento = preencherCertidaoModel(pessoaSIGA.CertidaoNascimento, pessoa.certidaoNascimento);

            if( pessoa.cnh != null)
            {
                pessoaSIGA.Cnh = new CnhModel();
                pessoaSIGA.Cnh.DataEmissao = pessoa.cnh.dataEmissao;
                pessoaSIGA.Cnh.DataValidade = pessoa.cnh.dataValidade;
            }

            pessoaSIGA.CodigoCnesEstabelecimentoCadastro = pessoa.codigoCnesEstabelecimentoCadastro;
            pessoaSIGA.CodigoCnesEstabelecimentoVinculo = pessoa.codigoCnesEstabelecimentoVinculo;
            pessoaSIGA.CodigoConvenioSus = pessoa.codigoConvenioSus;
            pessoaSIGA.CodigoEscolaridadeSus = pessoa.codigoEscolaridadeSus;
            pessoaSIGA.CodigoEtniaSus = pessoa.codigoEtniaSus;
            pessoaSIGA.CodigoMunicipioNascimentoSus = pessoa.codigoMunicipioNascimentoSus;
            pessoaSIGA.CodigoOcupacaoSus = pessoa.codigoOcupacaoSus;
            pessoaSIGA.CodigoPaisNascimento = pessoa.codigoPaisNascimento;
            pessoaSIGA.CodigoRacaSus = pessoa.codigoRacaSus;
            pessoaSIGA.CodigoSexoSus = pessoa.codigoSexoSus;
            pessoaSIGA.CodigoSituacaoFamiliarSus = pessoa.codigoSituacaoFamiliarSus;
            pessoaSIGA.CodigoTipoNacionalidade = pessoa.codigoTipoNacionalidade;

            if(pessoa.contato != null)
            {
                pessoaSIGA.Contato = new ContatoModel();
                pessoaSIGA.Contato.DddCelular = pessoa.contato.dddCelular;
                pessoaSIGA.Contato.DddComercial = pessoa.contato.dddComercial;
                pessoaSIGA.Contato.DddContato = pessoa.contato.dddContato;
                pessoaSIGA.Contato.DddResidencial = pessoa.contato.dddResidencial;
                pessoaSIGA.Contato.Email = pessoa.contato.email;
                pessoaSIGA.Contato.NomeContato = pessoa.contato.nomeContato;
                pessoaSIGA.Contato.SemCelular = pessoa.contato.semCelular;
                pessoaSIGA.Contato.TelefoneCelular = pessoa.contato.telefoneCelular;
                pessoaSIGA.Contato.TelefoneComercial = pessoa.contato.telefoneComercial;
                pessoaSIGA.Contato.TelefoneContato = pessoa.contato.telefoneContato;
                pessoaSIGA.Contato.TelefoneResidencial = pessoa.contato.telefoneResidencial;
            }

            if(pessoa.cpf != null)
                pessoaSIGA.NumeroCpf = pessoa.cpf.numero;

            if(pessoa.ctps != null)
            {
                pessoaSIGA.Ctps = new CtpsModel();
                pessoaSIGA.Ctps.CodigoUfSus = pessoa.ctps.codigoUfSus;
                pessoaSIGA.Ctps.DataEmissao = pessoa.ctps.dataEmissao;
                pessoaSIGA.Ctps.Numero = pessoa.ctps.numero;
                pessoaSIGA.Ctps.Serie = pessoa.ctps.serie;
            }

            pessoaSIGA.DataEntradaPais = pessoa.dataEntradaPais;
            pessoaSIGA.DataNascimento = pessoa.dataNascimento;
            pessoaSIGA.DataNaturalizacao = pessoa.dataNaturalizacao;
            pessoaSIGA.Dnv = pessoa.dnv;

            if(pessoa.endereco != null)
            {
                pessoaSIGA.Endereco = new EnderecoModel();
                pessoaSIGA.Endereco.Bairro = pessoa.endereco.bairro;
                pessoaSIGA.Endereco.Cep = pessoa.endereco.cep;
                pessoaSIGA.Endereco.CodigoDistritoAdministrativoSUS = pessoa.endereco.codigoDistritoAdministrativoSUS;
                pessoaSIGA.Endereco.CodigoMunicipioResidenciaSUS = pessoa.endereco.codigoMunicipioResidenciaSUS;
                pessoaSIGA.Endereco.CodigoOrigemEndereco = pessoa.endereco.codigoOrigemEndereco;
                pessoaSIGA.Endereco.Complemento = pessoa.endereco.complemento;
                pessoaSIGA.Endereco.Logradouro = pessoa.endereco.logradouro;
                pessoaSIGA.Endereco.NumeroResidencia = pessoa.endereco.numeroResidencia;
                pessoaSIGA.Endereco.Referencia = pessoa.endereco.referencia;
                pessoaSIGA.Endereco.SemNumero = pessoa.endereco.semNumero;
                pessoaSIGA.Endereco.TipoLogradouro = pessoa.endereco.tipoLogradouro;
            }

            pessoaSIGA.frequentaEscola = pessoa.frequentaEscola;

            if(pessoa.identidade != null)
            {
                pessoaSIGA.Identidade = new IdentidadeModel();
                pessoaSIGA.Identidade.CodigoOrgaoEmissorSus = pessoa.identidade.codigoOrgaoEmissorSus;
                pessoaSIGA.Identidade.CodigoUfSus = pessoa.identidade.codigoUfSus;
                pessoaSIGA.Identidade.DataEmissao = pessoa.identidade.dataEmissao;
                pessoaSIGA.Identidade.Numero = pessoa.identidade.numero;
            }

            if(pessoa.identidadeEstrangeiro != null)
            {
                pessoaSIGA.IdentidadeEstrangeiro = new IdentidadeEstrangeiroModel();
                pessoaSIGA.IdentidadeEstrangeiro.CodigoNaturalidade = pessoa.identidadeEstrangeiro.codigoNaturalidade;
                pessoaSIGA.IdentidadeEstrangeiro.CodigoOrgaoEmissorSus = pessoa.identidadeEstrangeiro.codigoOrgaoEmissorSus;
                pessoaSIGA.IdentidadeEstrangeiro.DataEntrada = pessoa.identidadeEstrangeiro.dataEntrada;
                pessoaSIGA.IdentidadeEstrangeiro.DataValidade = pessoa.identidadeEstrangeiro.dataValidade;
                pessoaSIGA.IdentidadeEstrangeiro.Numero = pessoa.identidadeEstrangeiro.numero;
            }

            pessoaSIGA.MaeDesconhecida = pessoa.maeDesconhecida;
            pessoaSIGA.Nome = pessoa.nome;
            pessoaSIGA.NomeMae = pessoa.nomeMae;
            pessoaSIGA.NomePai = pessoa.nomePai;
            pessoaSIGA.NomeSocial = pessoa.nomeSocial;
            pessoaSIGA.NumeroCns = pessoa.numeroCns;
            pessoaSIGA.NumeroCnsUsuario = pessoa.numeroCnsUsuario;
            pessoaSIGA.Observacao = pessoa.observacao;
            pessoaSIGA.PaiDesconhecido = pessoa.paiDesconhecido;

            if(pessoa.passaporte != null)
            {
                pessoaSIGA.Passaporte = new PassaporteModel();
                pessoaSIGA.Passaporte.CodigoPaisdeEmissao = pessoa.passaporte.codigoPaisdeEmissao;
                pessoaSIGA.Passaporte.DataEmissao = pessoa.passaporte.dataEmissao;
                pessoaSIGA.Passaporte.DataValidade = pessoa.passaporte.dataValidade;
                pessoaSIGA.Passaporte.Numero = pessoa.passaporte.numero;
            }

            if(pessoa.pisPasep != null)            
                pessoaSIGA.NumeroPisPasep = pessoa.pisPasep.numero;

            pessoaSIGA.PortariaNaturalizacao = pessoa.portariaNaturalizacao;
            pessoaSIGA.PossuiConvenio = pessoa.possuiConvenio;
            pessoaSIGA.protocoloCadastro = pessoa.protocoloCadastro;

            if(pessoa.tituloEleitor != null)
            {
                pessoaSIGA.TituloEleitor = new TituloEleitorModel();
                pessoaSIGA.TituloEleitor.DataEmissao = pessoa.tituloEleitor.dataEmissao;
                pessoaSIGA.TituloEleitor.Numero = pessoa.tituloEleitor.numero;
                pessoaSIGA.TituloEleitor.Secao = pessoa.tituloEleitor.secao;
                pessoaSIGA.TituloEleitor.Zona = pessoa.tituloEleitor.zona;
            }

            return pessoaSIGA;
        }

        private PreNatalModel PessoaServiceToPreNatal(PessoaService.Pessoa pessoa)
        {
            PreNatalModel preNatal = new PreNatalModel();

            SolicitacaoMatriculaPreNatalModel matriculaPreNatal = new SolicitacaoMatriculaPreNatalModel();
            matriculaPreNatal.NrCnsResponsavel = pessoa.numeroCns;
            preNatal.MatriculaPreNatal = matriculaPreNatal;

            ResponsavelGeralModel responsavel = new ResponsavelGeralModel();
            if (pessoa.cpf != null)
                responsavel.CdCpfResponsavel = Convert.ToDecimal(pessoa.cpf.numero); 

            responsavel.NmResponsavel = pessoa.nome;

            if (pessoa.identidade != null)
            {
                responsavel.NrRgResponsavel = pessoa.identidade.numero;

                if (!String.IsNullOrEmpty(pessoa.identidade.codigoUfSus))
                responsavel.SgUfRgResponsavel = Enum.GetName(typeof(SiglaUFEnum), Convert.ToInt32(pessoa.identidade.codigoUfSus));    
            }

            responsavel.NmMaeResponsavel = pessoa.nomeMae;  
            responsavel.DtNascimentoMaeResponsavel = pessoa.dataNascimento;
            responsavel.TpRacaCor = pessoa.codigoRacaSus;

            if (pessoa.identidadeEstrangeiro != null)
                responsavel.NrDocumentoEstrangeiro = pessoa.identidadeEstrangeiro.numero;

            if(pessoa.endereco != null)
            {
                //responsavel.TpLogradouro = pessoa.endereco.tipoLogradouro; //tipoLogradouro no SIGA é string, ex.: AV
                responsavel.DcTpLogradouro = pessoa.endereco.tipoLogradouro;
                responsavel.NmLogradouro = pessoa.endereco.logradouro;                
                responsavel.CdNrEndereco = pessoa.endereco.numeroResidencia; 
                responsavel.DcComplementoEndereco = pessoa.endereco.complemento;
                responsavel.NmBairro = pessoa.endereco.bairro;

                try
                {
                    responsavel.CdCep = Convert.ToInt32(pessoa.endereco.cep);
                } catch(FormatException e)
                {
                    responsavel.CdCep = 0;
                }
            }

            if (pessoa.contato != null) {
                responsavel.CdDddCelularResponsavel = pessoa.contato.dddCelular;
                responsavel.NrCelularResponsavel = pessoa.contato.telefoneCelular;
                responsavel.CdDddTelefoneFixoResponsavel = pessoa.contato.dddResidencial;
                responsavel.NrTelefoneFixoResponsavel = pessoa.contato.telefoneResidencial;
                responsavel.CdDddTelefoneComercialResponsavel = pessoa.contato.dddComercial;
                responsavel.NrTelefoneComercialResponsavel = pessoa.contato.telefoneComercial;
                responsavel.NmEmailResponsavel = pessoa.contato.email;
            }

            preNatal.Responsavel = responsavel;

            AlunoModel aluno = new AlunoModel();            
            aluno.CdSexoMae = pessoa.codigoSexoSus;
            aluno.NmMaeAluno = pessoa.nome;
            aluno.CdPaisOrigemMae = pessoa.codigoPaisNascimento;
            //aluno.CdNacionalidadeMae = pessoa.codigoPaisNascimento;
            aluno.TpRacaCor = Convert.ToInt32(pessoa.codigoRacaSus);

            preNatal.Aluno = aluno;

            return preNatal;
        }

        private CertidaoModel preencherCertidaoModel(CertidaoModel model, PessoaService.Certidao certidao)
        {
            if (certidao != null)
            {
                model = new CertidaoModel();
                if (certidao.certidaoAntiga != null)
                {
                    model.CertidaoAntiga = new CertidaoAntigaModel();
                    model.CertidaoAntiga.DataEmissao = certidao.certidaoAntiga.dataEmissao;
                    model.CertidaoAntiga.NomeCartorio = certidao.certidaoAntiga.nomeCartorio;
                    model.CertidaoAntiga.NumeroFolha = certidao.certidaoAntiga.numeroFolha;
                    model.CertidaoAntiga.NumeroLivro = certidao.certidaoAntiga.numeroLivro;
                    model.CertidaoAntiga.NumeroTermo = certidao.certidaoAntiga.numeroTermo;
                }
                if (certidao.certidaoNova != null)
                {
                    model.CertidaoNova = new CertidaoNovaModel();
                    model.CertidaoNova.DataEmissao = certidao.certidaoNova.dataEmissao;
                    model.CertidaoNova.Matricula = certidao.certidaoNova.matricula;
                }

            }
            return model;
        }
    }
}
