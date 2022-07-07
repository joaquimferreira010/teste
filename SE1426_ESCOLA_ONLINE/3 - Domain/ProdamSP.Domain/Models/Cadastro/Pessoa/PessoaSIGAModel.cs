using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class PessoaSIGAModel
    {
        public CertidaoModel CertidaoAdministrativaIndio { get; set; }
        public CertidaoModel CertidaoCasamento { get; set; }
        public CertidaoModel CertidaoDivorcio { get; set; }
        public CertidaoModel CertidaoNascimento { get; set; }       
        public CnhModel Cnh { get; set; }
        public string CodigoCnesEstabelecimentoCadastro { get; set; }
        public string CodigoCnesEstabelecimentoVinculo { get; set; }
        public string CodigoConvenioSus { get; set; }
        public string CodigoEscolaridadeSus { get; set; }
        public string CodigoEtniaSus { get; set; }
        public string CodigoMunicipioNascimentoSus { get; set; }
        public string CodigoOcupacaoSus { get; set; }
        public string CodigoPaisNascimento { get; set; }
        public string CodigoRacaSus { get; set; }
        public string CodigoSexoSus { get; set; }
        public string CodigoSituacaoFamiliarSus { get; set; }
        public string CodigoTipoNacionalidade { get; set; }
        public ContatoModel Contato { get; set; }        
        public string NumeroCpf { get; set; }
        public CtpsModel Ctps { get; set; }
        public DateTime? DataEntradaPais { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime? DataNaturalizacao { get; set; }
        public string Dnv { get; set; }
        public  EnderecoModel Endereco { get; set; }
        public string frequentaEscola { get; set; }
        public IdentidadeModel Identidade { get; set; }
        public string MaeDesconhecida { get; set; }
        public string Nome { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public string NomeSocial { get; set; }
        public string NumeroCns { get; set; }
        public string NumeroCnsUsuario { get; set; }
        public string Observacao { get; set; }
        public string PaiDesconhecido{ get; set; }
        public PassaporteModel Passaporte { get; set; }
        public string NumeroPisPasep { get; set; }
        public string PortariaNaturalizacao { get; set; }
        public string PossuiConvenio { get; set; }
        public string protocoloCadastro { get; set; }
        public IdentidadeEstrangeiroModel IdentidadeEstrangeiro { get; set; }
        public TituloEleitorModel TituloEleitor { get; set; }
        public int? CodRetorno { get; set; }
        public string MsgRetorno { get; set; }
    }
}
