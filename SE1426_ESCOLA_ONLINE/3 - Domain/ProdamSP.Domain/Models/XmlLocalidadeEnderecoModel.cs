namespace ProdamSP.Domain.Models
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2003/05/soap-envelope", IsNullable = false)]
    public partial class Envelope
    {

        private object headerField;

        private EnvelopeBody bodyField;

        /// <remarks/>
        public object Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public EnvelopeBody Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public partial class EnvelopeBody
    {

        private listarEnderecosPorCEPCompletav10RetornoMensagem listarEnderecosPorCEPCompletav10RetornoMensagemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("listarEnderecosPorCEPCompleta.v1.0.Retorno.Mensagem", Namespace = "http://prodam.sp.gov.br/prodam/wsdl/localidade.v2")]
        public listarEnderecosPorCEPCompletav10RetornoMensagem listarEnderecosPorCEPCompletav10RetornoMensagem
        {
            get
            {
                return this.listarEnderecosPorCEPCompletav10RetornoMensagemField;
            }
            set
            {
                this.listarEnderecosPorCEPCompletav10RetornoMensagemField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://prodam.sp.gov.br/prodam/wsdl/localidade.v2")]
    [System.Xml.Serialization.XmlRootAttribute("listarEnderecosPorCEPCompleta.v1.0.Retorno.Mensagem", Namespace = "http://prodam.sp.gov.br/prodam/wsdl/localidade.v2", IsNullable = false)]
    public partial class listarEnderecosPorCEPCompletav10RetornoMensagem
    {

        private uint codCEPField;

        private string codTipoLogradorouroField;

        private string codTipoLogradorouroDNEField;

        private string nomLogradouroProdamField;

        private string nomLogradouroDNEField;

        private string nomLogradouroField;

        private string nomBairroField;

        private string nomeCidadeField;

        private string sglUFField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public uint codCEP
        {
            get
            {
                return this.codCEPField;
            }
            set
            {
                this.codCEPField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string codTipoLogradorouro
        {
            get
            {
                return this.codTipoLogradorouroField;
            }
            set
            {
                this.codTipoLogradorouroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string codTipoLogradorouroDNE
        {
            get
            {
                return this.codTipoLogradorouroDNEField;
            }
            set
            {
                this.codTipoLogradorouroDNEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string nomLogradouroProdam
        {
            get
            {
                return this.nomLogradouroProdamField;
            }
            set
            {
                this.nomLogradouroProdamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string nomLogradouroDNE
        {
            get
            {
                return this.nomLogradouroDNEField;
            }
            set
            {
                this.nomLogradouroDNEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string nomLogradouro
        {
            get
            {
                return this.nomLogradouroField;
            }
            set
            {
                this.nomLogradouroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string nomBairro
        {
            get
            {
                return this.nomBairroField;
            }
            set
            {
                this.nomBairroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string nomeCidade
        {
            get
            {
                return this.nomeCidadeField;
            }
            set
            {
                this.nomeCidadeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string sglUF
        {
            get
            {
                return this.sglUFField;
            }
            set
            {
                this.sglUFField = value;
            }
        }
    }
}
