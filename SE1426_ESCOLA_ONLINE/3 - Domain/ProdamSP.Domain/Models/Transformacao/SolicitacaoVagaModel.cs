using ProdamSP.Domain.Models.Cadastro.Pessoa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.Domain.Models.Cadastro
{
    public class SolicitacaoVagaModel
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string NrCnsResponsavel { get; set; }       //SolicitacaoMatriculaPreNatal.NrCnsResponsavel
        public string NrPreNatal { get; set; }             //SolicitacaoMatriculaPreNatal.NrPreNatal
        public string CdSolicitacaoMatricula { get; set; }  //SolicitacaoMatriculaPreNatal.CdSolicitacaoMatricula
        public DadosMaeModel DadosMae { get; set; }
        public DadosCriancaModel DadosCrianca { get; set; }
        //public int? CodRetorno { get; set; }
        ///public string MsgRetorno { get; set; }
    }
}
