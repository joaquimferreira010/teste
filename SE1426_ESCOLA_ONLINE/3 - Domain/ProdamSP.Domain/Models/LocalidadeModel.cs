using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models
{
    public class LocalidadeModel
    {
        public string CEP { get; set; }
        public string codTipoLogradouro { get; set; }
        public string codTipoLogradouroDNE { get; set; }
        public string NmLogradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public int? CodRetorno { get; set; }
        public string MsgRetorno { get; set; }

    }
}
