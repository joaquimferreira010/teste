﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace ProdamSP.Domain.Entities
{
    public partial class ResponsavelCns
    {
        public ResponsavelCns()
        {
            SolicitacaoMatriculaPreNatal = new HashSet<SolicitacaoMatriculaPreNatal>();
        }

        public string NrCnsResponsavel { get; set; }
        public int CdResponsavelGeral { get; set; }
        public string CdOperador { get; set; }
        public DateTime DtAtualizacaoTabela { get; set; }

        public virtual ResponsavelGeral CdResponsavelGeralNavigation { get; set; }
        public virtual ICollection<SolicitacaoMatriculaPreNatal> SolicitacaoMatriculaPreNatal { get; set; }
    }
}