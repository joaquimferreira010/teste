using ProdamSP.Domain.Models.Cadastro.Pessoa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class TransformacaoConsultaCriancaModel
    {
        public string NrCnsResponsavel { get; set; }
        public string NrPreNatal { get; set; }
        public string CdSolicitacaoMatricula { get; set; }
        public string NrCnsCrianca { get; set; }          
    }
}
