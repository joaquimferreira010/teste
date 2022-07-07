using System;
using System.Collections.Generic;
using System.Text;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class PassaporteModel
    {
        public string Numero { get; set; }
        public string CodigoPaisdeEmissao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
    }
}
