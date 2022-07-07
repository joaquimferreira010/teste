using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectsComparer;

namespace ProdamSP.Domain.Models
{
    public class DiferencaModel
    {
        #region Propriedades
        public string Propriedade { get; set; }
        public string ValorAntes { get; set; }
        public string ValorDepois { get; set; }

        #endregion

        #region Construtores
        public DiferencaModel() { }

        #endregion
    }
}
