using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class PreNatalModel
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Base { get; set; }
        public int QtdeCriancaGestacao { get; set; }

        public AlunoModel Aluno { get; set; }
        public ResponsavelGeralModel Responsavel { get; set; }
        public SolicitacaoMatriculaModel Matricula { get; set; }
        public SolicitacaoMatriculaPreNatalModel MatriculaPreNatal { get; set; }

        public int? codRetorno { get; set; }
        public string msgRetorno { get; set; }
    }
}
