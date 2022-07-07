using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
	public class EnderecoModel
	{
		public string Bairro { get; set; }
		public string Cep { get; set; }
		public string CodigoDistritoAdministrativoSUS { get; set; }
		public string CodigoMunicipioResidenciaSUS { get; set; }
		public string CodigoOrigemEndereco { get; set; }
		public string Complemento { get; set; }
		public string Logradouro { get; set; }
		public string NumeroResidencia { get; set; }
		public string Referencia { get; set; }
		public string SemNumero { get; set; }
		public string TipoLogradouro { get; set; }
	}
}
