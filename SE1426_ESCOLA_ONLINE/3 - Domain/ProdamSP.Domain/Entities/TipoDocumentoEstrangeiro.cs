﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace ProdamSP.Domain.Entities
{
    public partial class TipoDocumentoEstrangeiro
    {
        public TipoDocumentoEstrangeiro()
        {
            ResponsavelGeral = new HashSet<ResponsavelGeral>();
        }

        public short CdTipoDocumentoEstrangeiro { get; set; }
        public string DcTipoDocumentoEstrangeiro { get; set; }
        public string CdOperador { get; set; }
        public DateTime DtAtualizacaoTabela { get; set; }

        public virtual ICollection<ResponsavelGeral> ResponsavelGeral { get; set; }
    }
}