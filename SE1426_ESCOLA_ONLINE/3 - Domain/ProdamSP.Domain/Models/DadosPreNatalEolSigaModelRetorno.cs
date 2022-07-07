using System;
using System.Collections.Generic;

namespace ProdamSP.Domain.Models
{
    public partial class DadosPreNatalEolSigaModelRetorno
    {
        public int? codEOLPreNatal { get; set; }
        public DateTime? dataPrevisaoParto { get; set; }
        public DateTime? dataCadastroPreNatal { get; set; }
        public int? codRetorno { get; set; }
        public string msgRetorno { get; set; }
    }
}


