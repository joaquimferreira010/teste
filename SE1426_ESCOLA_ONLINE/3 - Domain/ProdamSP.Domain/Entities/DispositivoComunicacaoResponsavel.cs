// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace ProdamSP.Domain.Entities
{
    public partial class DispositivoComunicacaoResponsavel
    {
        public int CdDispositivoComunicacaoResponsavel { get; set; }
        public int CdResponsavelGeral { get; set; }
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
        public string CdOperador { get; set; }
        public DateTime DtAtualizacaoTabela { get; set; }

        public virtual ResponsavelGeral CdResponsavelGeralNavigation { get; set; }
    }
}