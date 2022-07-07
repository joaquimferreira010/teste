using System;
using System.Collections.Generic;

namespace ProdamSP.Domain.Models

{
    public partial class CaptchaModel
    {
        public byte[] Imagem { get; set; }
        public string Imagem64 { get; set; }

        public string ExtensaoImagem { get; set; }

        public string SomGerado64 { get; set; }

        public string RetornoValidacao { get; set; }

        public string NomeArquivoSugerido { get; set; }

        public string MensagemRetorno { get; set; }

        public int? codRetorno { get; set; }
        public string msgRetorno { get; set; }

        public bool Valido { get; set; }

        public string TipoSom { get; set; }
        public string TextoDigitado { get; set; }
    }
}


