﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace ProdamSP.Domain.Entities
{
    public partial class OrigemLocalAlteracao
    {
        public OrigemLocalAlteracao()
        {
            SolicitacaoMatriculaPreNatal = new HashSet<SolicitacaoMatriculaPreNatal>();
        }

        public int CdOrigemLocalAlteracao { get; set; }
        public string DcOrigemLocalAlteracao { get; set; }
        public string CdOperador { get; set; }
        public DateTime DtAtualizacaoOperador { get; set; }

        public virtual ICollection<SolicitacaoMatriculaPreNatal> SolicitacaoMatriculaPreNatal { get; set; }
    }
}