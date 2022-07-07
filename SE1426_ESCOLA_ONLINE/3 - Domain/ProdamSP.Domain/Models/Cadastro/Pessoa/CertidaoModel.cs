using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class CertidaoModel
    {
        public CertidaoAntigaModel CertidaoAntiga { get; set; }
        public CertidaoNovaModel CertidaoNova { get; set; }
    }
}
