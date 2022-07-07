using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class IdentidadeModel
    {
        public string Numero { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string CodigoUfSus { get; set; }
        public string CodigoOrgaoEmissorSus { get; set; }
    }
}
