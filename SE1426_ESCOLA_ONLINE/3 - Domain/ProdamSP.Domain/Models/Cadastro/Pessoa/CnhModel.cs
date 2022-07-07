using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class CnhModel
    {
        public String Numero { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime? DataEmissao { get; set; }
    }
}
