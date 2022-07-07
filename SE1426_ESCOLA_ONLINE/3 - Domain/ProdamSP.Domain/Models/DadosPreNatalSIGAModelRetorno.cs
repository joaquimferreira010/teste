using System;
using System.Collections.Generic;

namespace ProdamSP.Domain.Models
{
    public partial class DadosPreNatalSIGAModelRetorno
    {
        public int? codRetorno { get; set; }
        public string msgRetorno { get; set; }
        
        public DateTime? dataCadastroPreNatal { get; set; }
        public DateTime? dataUltimaMenstruacao { get; set; }
        public DateTime? dataPrevisaoParto { get; set; }
        public DateTime? dataUltimaConsulta { get; set; }
        public DateTime? dataParto { get; set; }
        public DateTime? dataInterrupcao { get; set; }

    }
}


