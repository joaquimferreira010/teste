﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace ProdamSP.Domain.Entities
{
    public partial class SolicitacaoMatriculaPreNatal
    {
        public int CdSolicitacaoMatriculaPreNatal { get; set; }
        public string NrCnsResponsavel { get; set; }
        public string NrPreNatal { get; set; }
        public DateTime? DtInicioPreNatal { get; set; }
        public DateTime DtInscricaoEol { get; set; }
        public DateTime? DtExclusaoInscricaoEol { get; set; }
        public int CdOrigemLocalAlteracao { get; set; }
        public DateTime DtNascimentoPrevista { get; set; }
        public DateTime DtIntencaoMatricula { get; set; }
        public DateTime? DtUltimaConsultaGestante { get; set; }
        public DateTime? DtCancelamentoInscricaoEol { get; set; }
        public int? CdMotivoCancelamentoInscricao { get; set; }
        public DateTime? DtTransformacaoCandidato { get; set; }
        public int CdSolicitacaoMatricula { get; set; }
        public DateTime? DtSolicitacaoMatricula { get; set; }
        public DateTime? DtEncaminhamentoMatricula { get; set; }
        public string NrCnsCrianca { get; set; }
        public DateTime DtAtualizacaoTabela { get; set; }
        public string CdOperador { get; set; }

        public virtual OrigemLocalAlteracao CdOrigemLocalAlteracaoNavigation { get; set; }
        public virtual SolicitacaoMatricula CdSolicitacaoMatriculaNavigation { get; set; }
        public virtual ResponsavelCns NrCnsResponsavelNavigation { get; set; }
    }
}