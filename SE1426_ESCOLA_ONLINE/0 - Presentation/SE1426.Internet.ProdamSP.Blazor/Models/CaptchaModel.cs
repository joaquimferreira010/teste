using System;
using System.Collections.Generic;

namespace SE1426.Internet.ProdamSP.Blazor.Models

{
    public class CaptchaModel
    {

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

        public string SrcImagem { get; set; }
        public string SrcSom { get; set; }

        public string Token { get; set; }
    }
}


