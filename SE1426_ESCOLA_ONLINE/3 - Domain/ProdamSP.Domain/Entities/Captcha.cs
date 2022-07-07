using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Entities
{
    public partial class Captcha
    {
        public byte[] BytesImagem { get; set; }

        public string ExtensaoImagem { get; set; }

        public byte[] BytesSomGerado { get; set; }

        public string RetornoValidacao { get; set; }

        public string NomeArquivoSugerido { get; set; }

        public string MensagemRetorno { get; set; }

    }
}
