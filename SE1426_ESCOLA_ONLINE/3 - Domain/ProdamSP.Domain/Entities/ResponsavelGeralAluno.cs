﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace ProdamSP.Domain.Entities
{
    public partial class ResponsavelGeralAluno
    {
        public int CdResponsavelGeralAluno { get; set; }
        public int CdResponsavelGeral { get; set; }
        public int CdAluno { get; set; }
        public int CdTipoPessoaResponsavel { get; set; }
        public DateTime DtInicioPeriodoResponsavel { get; set; }
        public DateTime? DtFimPeriodoResponsavel { get; set; }
        public string CdOperador { get; set; }
        public DateTime DtAtualizacaoTabela { get; set; }

        public virtual Aluno CdAlunoNavigation { get; set; }
        public virtual ResponsavelGeral CdResponsavelGeralNavigation { get; set; }
        public virtual TipoPessoaResponsavel CdTipoPessoaResponsavelNavigation { get; set; }
    }
}