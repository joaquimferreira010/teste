using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class PassaporteModel
    {
        public string Numero { get; set; }
        public string CodigoPaisdeEmissao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
    }
}
