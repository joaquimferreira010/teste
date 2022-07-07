using ProdamSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Models
{
    public class EmailModel
    {
        public IList<string> ListaDestinatarios { get; set; }
        public string TextoMensagem { get; set; }
        public string TituloMensagem { get; set; }
        public string SubTituloOuAssuntoMensagem { get; set; }
        public string ConteudoHtmlEmail { get; set; }

        public Dictionary<string, string> AttachmentsCorpoHtml { get; set; }
    }
}
