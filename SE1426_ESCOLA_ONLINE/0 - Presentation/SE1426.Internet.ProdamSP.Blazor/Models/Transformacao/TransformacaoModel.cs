using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class TransformacaoModel
    {
        //dados do pre-natal
        public int CdSolicitacaoMatriculaPreNatal { get; set; }
        public int CdSolicitacaoMatricula { get; set; }
        public string NrCnsResponsavel { get; set; }
        public string NrPreNatal { get; set; }
        public DateTime? DtInscricaoEol { get; set; }
        public DateTime? DtTransformacaoCandidato { get; set; }


        //dados da mae
        public DadosMaeModel DadosMae { get; set; }

        //numero sequencial da crianca
        public int NrCrianca { get; set; }
        //numero do cns da crianca
        public string NrCnsCrianca { get; set; }
        //campo para pesquisa de cns
        public string NrCnsCriancaFormatado { get; set; }

        //dados da criança pesquisada
        public DadosCriancaModel DadosCrianca { get; set; }

        //campos dos checkbox - atualizacao de nome de mae / endereco
        public bool InAtualizarNomeMae { get; set; }
        public bool InAtualizarEndereco { get; set; }

        //campos para controle de tela - mostrar ou nao a div correspondente
        public bool InCnsCriancaPreenchida { get; set; }
        public bool InNomeMaeDiferente { get; set; }
        public bool InEnderecoDiferente { get; set; }

        //campos de formatacao
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
        public string DtTransformacaoCandidatoFormatada
        {
            get
            {
                if (DtTransformacaoCandidato != null)
                {
                    return Convert.ToDateTime(DtTransformacaoCandidato).ToString("dd/MM/yyyy");
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

    }
}
