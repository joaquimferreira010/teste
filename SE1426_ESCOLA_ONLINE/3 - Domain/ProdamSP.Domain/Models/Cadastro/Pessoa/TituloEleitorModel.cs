using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class TituloEleitorModel
    {
        public string Numero { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string Zona { get; set; }
        public string Secao { get; set; }
    }
}
