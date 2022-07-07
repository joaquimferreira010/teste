using System;
using System.Collections.Generic;
using System.Text;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class TituloEleitorModel
    {
        public string Numero { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string Zona { get; set; }
        public string Secao { get; set; }
    }
}
