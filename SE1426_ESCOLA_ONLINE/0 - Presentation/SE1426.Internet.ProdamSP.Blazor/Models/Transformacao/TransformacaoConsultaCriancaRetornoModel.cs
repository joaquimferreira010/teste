using ProdamSP.Domain.Models.Cadastro.Pessoa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class TransformacaoConsultaCriancaRetornoModel
    {
        //Dados da Mae
        //public string NmResponsavel { get; set; }
        //public string DcTpLogradouroMae { get; set; }         
        //public string NmLogradouroMae { get; set; }           
        //public string CdNrEnderecoMae { get; set; }           
        //public string DcComplementoEnderecoMae { get; set; }  
        //public string NmBairroMae { get; set; }               
        //public string CdCepMae { get; set; }                    
        //public string NmMunicipioMae { get; set; }            
        //public DateTime? DtInscricaoEol { get; set; }
        public DadosMaeModel DadosMae { get; set; }
        public DadosCriancaModel DadosCrianca { get; set; }

        public bool InNomeMaeDiferente { get; set; }
        public bool InEnderecoDiferente { get; set; }
        public bool InNascimentoAnteriorInscricao { get; set; }


        public int? CodRetorno { get; set; }
        public string MsgRetorno { get; set; }
    }
}
