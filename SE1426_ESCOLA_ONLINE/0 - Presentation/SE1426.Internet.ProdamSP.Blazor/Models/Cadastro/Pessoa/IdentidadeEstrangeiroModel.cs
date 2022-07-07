using System;
using System.Collections.Generic;
using System.Text;

namespace SE1426.Internet.ProdamSP.Blazor.Models
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
