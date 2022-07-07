using System;
using System.Collections.Generic;
using System.Text;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class CertidaoAntigaModel
    {
        public DateTime? DataEmissao { get; set; }
        public String NomeCartorio { get; set; }
        public String NumeroFolha { get; set; }
        public String NumeroLivro { get; set; }
        public String NumeroTermo { get; set; }
    }

}
