using System;
using System.Collections.Generic;
using System.Text;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class DadosMaeModel
    {

        //Dados da Mãe  
        public string NmMaeCrianca { get; set; }                 //ResponsavelGeralModel.NmResponsavel
        public DateTime? DtNascimentoMaeCrianca { get; set; }    //ResponsavelGeralModel.DtNascimentoMaeResponsavel

        public string CdPaisOrigemMae { get; set; }        //ResponsavelGeralModel.CdPaisOrigemMae
        public decimal? CdCpfResponsavel { get; set; }      //ResponsavelGeralModel.CdCpfResponsavel
        public string NrRgResponsavel { get; set; }        //ResponsavelGeralModel.NrRgResponsavel //ver se vai precisar do CdDigitoRgResponsavel
        public string SgUfRgResponsavel { get; set; }      //ResponsavelGeralModel.SgUfRgResponsavel
        public string NrDocumentoEstrangeiro { get; set; } //ResponsavelGeralModel.SgUfRgResponsavel
        
        //dados do endereco
        public string DcTpLogradouro { get; set; }         //ResponsavelGeralModel.DcTpLogradouro
        public string NmLogradouro { get; set; }           //ResponsavelGeralModel.NmLogradouro
        public string CdNrEndereco { get; set; }           //ResponsavelGeralModel.CdNrEndereco
        public string DcComplementoEndereco { get; set; }  //ResponsavelGeralModel.DcComplementoEndereco
        public string NmBairro { get; set; }               //ResponsavelGeralModel.NmBairro
        public int? CdCep { get; set; }                    //ResponsavelGeralModel.CdCep
        public string NmMunicipio { get; set; }            //ResponsavelGeralModel.NmMunicipio

        //dados de contato
        public string CdDddCelularMae { get; set; }                      //ResponsavelGeralModel.CdDddCelularResponsavel
        public string DcDispositivoCelularMae { get; set; }           //ResponsavelGeralModel.NrCelularResponsavel

        public string CdDddComercialMae { get; set; }                 //ResponsavelGeralModel.CdDddTelefoneComercialResponsavel 
        public string DcDispositivoComercialMae { get; set; }         //ResponsavelGeralModel.NrTelefoneComercialResponsavel 

        public string CdDddResidencialMae { get; set; }               //ResponsavelGeralModel.CdDddTelefoneFixoResponsavel
        public string DcDispositivoResidencialMae { get; set; }       //ResponsavelGeralModel.NrTelefoneFixoResponsavel   

        public string EmailResponsavel { get; set; }                    //ResponsavelGeralModel.NmEmailResponsavel
        
        //fonetica
        public string FoneticaNmMaeCrianca { get; set; }
    }
}
