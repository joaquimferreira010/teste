using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.Domain.Models.Cadastro
{
    public class PreNatalExclusaoModel
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int QtdeCriancaGestacao { get; set; }
        public string NmMaeCrianca { get; set; }
        public DateTime DtNascimentoPrevista { get; set; }
        public string CdMotivoCancelamentoInscricao { get; set; }
        public string NrCnsResponsavel { get; set; }
        public string NrPreNatal { get; set; }
        public string Base { get; set; }

    }
}
