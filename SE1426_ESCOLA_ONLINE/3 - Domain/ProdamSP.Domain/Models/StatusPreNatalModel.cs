using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models
{
    public class StatusPreNatalModel
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string NumeroCns { get; set; }
        public string NumeroSISPRENATAL { get; set; }
        public DateTime? DataPrevisaoParto { get; set; }
        public DateTime? DataUltimaConsulta { get; set; }
        public DateTime? DataInterrupcao { get; set; }
        public string CodigoMotivoInterrupcao { get; set; }
    }
    public class StatusPreNatalRetornoModel
    {
        public string Retorno { get; set; }
        public string Mensagem { get; set; }
    }
}
