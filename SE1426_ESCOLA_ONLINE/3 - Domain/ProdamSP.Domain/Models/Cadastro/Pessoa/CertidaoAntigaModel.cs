using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
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
