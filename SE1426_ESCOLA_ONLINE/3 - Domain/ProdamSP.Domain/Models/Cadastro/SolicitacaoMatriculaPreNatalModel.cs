using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.Domain.Models.Cadastro
{
    public class SolicitacaoMatriculaPreNatalModel
    {
        public int CdSolicitacaoMatriculaPreNatal { get; set; }
        public string NrCnsResponsavel { get; set; }
        public string NrPreNatal { get; set; }
        public DateTime? DtInicioPreNatal { get; set; }
        public DateTime? DtInscricaoEol { get; set; }
        public DateTime? DtExclusaoInscricaoEol { get; set; }
        public int? CdOrigemLocalAlteracao { get; set; }
        public string DcOrigemLocalAlteracao { get; set; }
        public DateTime DtNascimentoPrevista { get; set; }
        public DateTime DtIntencaoMatricula { get; set; }
        public DateTime? DtUltimaConsultaGestante { get; set; }
        public DateTime? DtCancelamentoInscricaoEol { get; set; }
        public int? CdMotivoCancelamentoInscricao { get; set; }
        public DateTime? DtTansformacaoCandidato { get; set; }
        public int CdSolicitacaoMatricula { get; set; }
        public DateTime? DtSolicitacaoMatricula { get; set; }
        public DateTime? DtEncaminhamentoMatricula { get; set; }
        public string NrCnsCrianca { get; set; }
        public DateTime DtAtualizacaoTabela { get; set; }
        public string CdOperador { get; set; }
        public DateTime DtLimiteInformarNascimento { get; set; }
    }
}
