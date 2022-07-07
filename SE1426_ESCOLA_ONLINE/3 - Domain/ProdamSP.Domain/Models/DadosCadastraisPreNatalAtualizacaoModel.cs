using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models
{
    public class DadosCadastraisPreNatalAtualizacaoModel
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string NrCnsResponsavel { get; set; }
        public string NmMaeCrianca { get; set; }
        public DateTime? DtNascimentoMaeCrianca { get; set; }
        public string CdPaisOrigemMae { get; set; }
        public decimal? CdCpfResponsavel { get; set; }
        public string NrRgResponsavel { get; set; }
        public string SgUfRgResponsavel { get; set; }
        public string DcTpLogradouro { get; set; }
        public string NmLogradouro { get; set; }
        public string CdNrEndereco { get; set; }
        public string DcComplementoEndereco { get; set; }
        public string NmBairro { get; set; }
        public int? CdCep { get; set; }
        public string NmMunicipio { get; set; }
        public string CdDddCelularMae { get; set; }
        public string DcDispositivoCelularMae { get; set; }
        public string CdDddComercialMae { get; set; }
        public string DcDispositivoComercialMae { get; set; }
        public string CdDddResidencialMae { get; set; }
        public string DcDispositivoResidencialMae { get; set; }
        public string EmailResponsavel { get; set; }
        public string NrDocumentoEstrangeiro { get; set; }
        public string FoneticaNmMaeCrianca { get; set; }
    }
    public class DadosCadastraisPreNatalAtualizacaoRetornoModel
    {
        public string Retorno { get; set; }
        public string Mensagem { get; set; }
    }
}
