using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class AlunoModel
    {
        public int CdAluno { get; set; }
        public string NmAluno { get; set; }
        public string NmAlunoFonetico { get; set; }
        public DateTime DtNascimentoAluno { get; set; }
        public string CdSexoAluno { get; set; }
        public string CdSexoMae { get; set; }
        public string NmMaeAluno { get; set; }
        public string NmMaeAlunoFonetico { get; set; }
        public int? TpRacaCor { get; set; }
        public int? CdOrgaoEmissor { get; set; }
        public int? CdPaisMec { get; set; }
        public string CdPaisOrigemMae { get; set; }
        public string SgUfRgAluno { get; set; }
        public long? CdCpfAluno { get; set; }
        public string NrRgAluno { get; set; }
        public string CdDigitoRgAluno { get; set; }
        public DateTime? DtEmissaoRg { get; set; }
        public string CdNacionalidadeMae { get; set; }
        public string CdDddResidencialMae { get; set; }
        public string DcDispositivoResidencialMae { get; set; }
        public string CdDddTrabalhoMae { get; set; }
        public string DcDispositivoTrabalhoMae { get; set; }
        public string CdDddCelularMae { get; set; }
        public string DcDispositivoCelularMae { get; set; }
        public string CdDddRecado { get; set; }
        public string DcDispositivoRecado { get; set; }
        public string NmContatoRecado { get; set; }
        public string DcDispositivoEmailAluno { get; set; }
        public int TpLogradouro { get; set; }
        public string SgTituloLogradouro { get; set; }
        public string NmLogradouro { get; set; }
        public string CdNrEndereco { get; set; }
        public string DcComplementoEndereco { get; set; }
        public string NmBairro { get; set; }
        public int CdCep { get; set; }
        public string TpLocalizacaoEndereco { get; set; }
        public int CdMunicipio { get; set; }
        public string TxReferenciaEndereco { get; set; }
        public string NmResponsavel { get; set; }
        public string NrRgResponsavel { get; set; }
        public string CdDigitoRgResponsavel { get; set; }
        public string SgUfRgResponsavel { get; set; }
        public decimal CdCpfResponsavel { get; set; }
        public string NmMaeResponsavel { get; set; }
        public string NrDocumentoEstrangeiro { get; set; }
        public int TpLogradouroResponsavel { get; set; }
        public string NmLogradouroResponsavel { get; set; }
        public string CdNrEnderecoResponsavel { get; set; }
        public string DcComplementoResponsavel { get; set; }
        public string NmBairroResponsavel { get; set; }
        public int CdMunicipioEnderecoResponsavel { get; set; }
        public int CdCepEnderecoResponsavel { get; set; }
        public string CdDddCelularResponsavel { get; set; }
        public string NrCelularResponsavel { get; set; }
        public string CdDddTelefoneFixoResponsavel { get; set; }
        public string NrTelefoneFixoResponsavel { get; set; }
        public string CdDddTelefoneComercialResponsavel { get; set; }
        public string NrTelefoneComercialResponsavel { get; set; }
        public string EmailResponsavel { get; set; }


        public string DtNascimentoAlunoFormatada
        {
            get
            {
                if (DtNascimentoAluno != null)
                {
                    return Convert.ToDateTime(DtNascimentoAluno).ToString("dd/MM/yyyy");
                }
                else
                {
                    return "00/00/0000";
                }

            }
        }

    }
}
