using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
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


        public string NrCnsResponsavelFormatado
        {
            get
            {
                if (NrCnsResponsavel != null)
                {
                    return Convert.ToUInt64(NrCnsResponsavel).ToString(@"000 0000 0000 0000");
                }
                else
                {
                    return "000 0000 0000 0000";
                }
            }
        }

        public string NrPreNatalFormatado
        {
            get
            {
                if (NrPreNatal != null)
                {
                    return Convert.ToUInt64(NrPreNatal).ToString(@"000000000000000");
                }
                else
                {
                    return "000000000000000";
                }
            }
        }

        public string DtNascimentoPrevistaFormatada
        {
            get 
            {
                if (DtNascimentoPrevista != null)
                {
                    return Convert.ToDateTime(DtNascimentoPrevista).ToString("dd/MM/yyyy");
                }
                else 
                {
                    return "00/00/0000";
                }
                
            }
        }
        public string DtIntencaoMatriculaFormatada
        {
            get
            {
                if (DtIntencaoMatricula != null)
                {
                    return Convert.ToDateTime(DtIntencaoMatricula).ToString("dd/MM/yyyy");
                }
                else
                {
                    return "00/00/0000";
                }

            }
        }
        
        public string CdSolicitacaoMatriculaFormatado
        {
            get
            {
                if (CdSolicitacaoMatricula != null)
                {
                    return Convert.ToUInt64(CdSolicitacaoMatricula).ToString(@"00000000000");
                }
                else
                {
                    return "00000000000";
                }
            }
        }

        public string DtLimiteInformarNascimentoFormatada
        {
            get
            {
                if (DtLimiteInformarNascimento != null)
                {
                    return Convert.ToDateTime(DtLimiteInformarNascimento).ToString("dd/MM/yyyy");
                }
                else
                {
                    return "00/00/0000";
                }
            }
        }

    }
}
