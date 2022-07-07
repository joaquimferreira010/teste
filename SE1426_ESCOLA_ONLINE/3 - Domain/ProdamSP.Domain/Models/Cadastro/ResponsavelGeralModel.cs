using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.Domain.Models.Cadastro
{
    public class ResponsavelGeralModel
    {
        public int CdResponsavelGeral { get; set; }
        public int CdTipoPessoaResponsavel { get; set; }
        public decimal CdCpfResponsavel { get; set; }
        public string InCpfResponsavelConferido { get; set; }
        public string NmResponsavel { get; set; }
        public long CdEndereco { get; set; }
        public string NrRgResponsavel { get; set; }
        public string CdDigitoRgResponsavel { get; set; }
        public string SgUfRgResponsavel { get; set; }
        public string NmMaeResponsavel { get; set; }
        public DateTime? DtNascimentoMaeResponsavel { get; set; }
        public int CdTipoDocumentoEstrangeiro { get; set; }
        public string NrDocumentoEstrangeiro { get; set; }
        public DateTime DtInicio { get; set; }
        public DateTime DtFim { get; set; }
        public string CdSituacaoDocumentoResponsavel{ get; set; }
        public long CiEndereco { get; set; }
        public int TpLogradouro { get; set; }
        public string DcTpLogradouro { get; set; }
        public string NmLogradouro { get; set; }
        public string CdNrEndereco { get; set; }
        public string DcComplementoEndereco { get; set; }
        public string NmBairro { get; set; }
        public int? CdCep { get; set; }
        public string TpLocalizacaoEndereco { get; set; }
        public int? CdMunicipio { get; set; }
        public string NmMunicipio { get; set; }
        public int CdDispositivoComunicacaoResponsavel { get; set; }
        public string CdDddCelularResponsavel { get; set; }
        public string NrCelularResponsavel { get; set; }
        public int? CdTipoTurnoCelular { get; set; }
        public string CdDddTelefoneFixoResponsavel { get; set; }
        public string NrTelefoneFixoResponsavel { get; set; }
        public int? CdTipoTurnoFixo { get; set; }
        public string CdDddTelefoneComercialResponsavel { get; set; }
        public string NrTelefoneComercialResponsavel { get; set; }
        public string NrRamalTelefoneComercialResponsavel { get; set; }
        public int? CdTipoTurnoComercial { get; set; }
        public string InAutorizaEnvioSms { get; set; }
        public string NmEmailResponsavel { get; set; }
        public string TpRacaCor { get; set; }
        public string CdPaisOrigemMae { get; set; }
    }
}
