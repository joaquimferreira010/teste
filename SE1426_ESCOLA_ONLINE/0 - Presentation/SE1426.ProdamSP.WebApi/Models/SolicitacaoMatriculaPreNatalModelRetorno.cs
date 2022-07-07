using System;
using System.Collections.Generic;

namespace SE1426.ProdamSP.WebApi.Models
{
    public partial class SolicitacaoMatriculaPreNatalModelRetorno
    {

        public int cdSolicitacaoMatriculaPreNatal { get; set; }
        public string nrCnsResponsavel { get; set; }
        public string nrPreNatal { get; set; }
        public DateTime? dtInicioPreNatal { get; set; }
        public DateTime dtInscricaoEol { get; set; }
        public DateTime? dtExclusaoInscricaoEol { get; set; }
        public int cdOrigemLocalAlteracao { get; set; }
        public DateTime dtNascimentoPrevista { get; set; }
        public DateTime dtIntencaoMatricula { get; set; }
        public DateTime? dtUltimaConsultaGestante { get; set; }
        public DateTime? dtCancelamentoInscricaoEol { get; set; }
        public int? cdMotivoCancelamentoInscricao { get; set; }
        public DateTime? dtTransformacaoCandidato { get; set; }
        public int cdSolicitacaoMatricula { get; set; }
        public DateTime? dtSolicitacaoMatricula { get; set; }
        public DateTime? dtEncaminhamentoMatricula { get; set; }
        public string nrCnsCrianca { get; set; }
        public DateTime dtAtualizacaoTabela { get; set; }
        public string cdOperador { get; set; }

        public int? codRetorno { get; set; }
        public string msgRetorno { get; set; }
        public int? codEOLPreNatal { get; set; }

    }
}


