using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.Domain.Models.Cadastro.Pessoa
{
    public class ContatoModel
    {
		public string DddResidencial { get; set; }
		public string TelefoneResidencial { get; set; }
		public string DddCelular { get; set; }
		public string TelefoneCelular { get; set; }
		public string SemCelular { get; set; }
		public string DddComercial { get; set; }
		public string TelefoneComercial { get; set; }
		public string DddContato { get; set; }
		public string TelefoneContato { get; set; }
		public string Email { get; set; }
		public string NomeContato { get; set; }
	}
}
