using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class IdentidadeEstrangeiroModel
    {
        public string Numero { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataValidade { get; set; }
        public string CodigoOrgaoEmissorSus { get; set; }
        public string CodigoNaturalidade { get; set; }
    }
}
