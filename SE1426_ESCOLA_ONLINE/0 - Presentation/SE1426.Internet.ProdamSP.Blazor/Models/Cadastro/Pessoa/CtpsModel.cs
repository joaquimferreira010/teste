using System;
using System.Collections.Generic;
using System.Text;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class CtpsModel
    {
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string CodigoUfSus { get; set; }
        public DateTime? DataEmissao { get; set; }
    }
}
