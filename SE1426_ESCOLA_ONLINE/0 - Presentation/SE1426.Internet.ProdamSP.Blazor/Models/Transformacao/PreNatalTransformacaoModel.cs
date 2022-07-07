using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class PreNatalTransformacaoModel
    {
        public AlunoModel Aluno { get; set; }
        public ResponsavelGeralModel Responsavel { get; set; }
        public SolicitacaoMatriculaModel Matricula { get; set; }
        public SolicitacaoMatriculaPreNatalModel MatriculaPreNatal { get; set; }

        public TransformacaoModel Transformacao { get; set; }
        public int? CodRetorno { get; set; }
        public string MsgRetorno { get; set; }

    }
}
