using System;
using System.Collections.Generic;
using System.Text;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public class DadosCriancaModel
    {
        public string NrCnsCrianca { get; set; }           //NumeroCns
        public string NmCrianca { get; set; }               //Nome
        public string NmMae { get; set; }                   //Nome da mãe
        public string NmPai { get; set; }                   //Nome do pai
        public DateTime? DtNascimentoCrianca { get; set; } //DataNascimento
        public string SexoCrianca { get; set; }             //CodigoSexoSus
        public int? TpRacaCorCrianca { get; set; }             //TpRacaCor
        public string CdPaisOrigemCrianca { get; set; }   //CodigoPaisNascimento
        public decimal? CdCpfCrianca { get; set; }           //NumeroCpf

        public string NrRgCrianca { get; set; }            //IdentidadeModel.Numero
        public string SgUfRgCrianca { get; set; }         //IdentidadeModel.CodigoUfSus

        public string NrDocumentoEstrangeiroCrianca { get; set; }   //IdentidadeEstrangeiroModel.Numero

        //Endereco
        public string DcTpLogradouroCrianca { get; set; }         //Endereco.TipoLogradouro//vem da localidade-oracle
        public string NmLogradouroCrianca { get; set; }            //Endereco.Logradouro//vem da localidade-oracle
        public string CdNrEnderecoCrianca { get; set; }           //Endereco.NumeroResidencia    
        public string DcComplementoEnderecoCrianca { get; set; }  //Endereco.Complemento 
        public string NmBairroCrianca { get; set; }                //Endereco.Bairro//vem da localidade-oracle
        public int? CdCepCrianca { get; set; }                   //Endereco.Cep
        public string NmMunicipioCrianca { get; set; }             //vem da localidade-oracle

        //Contato
        public string CdDddCelularCrianca { get; set; }                //Contato.DddCelular
        public string DcDispositivoCelularCrianca { get; set; }        //Contato.TelefoneCelular
        public string CdDddComercialCrianca { get; set; }              //Contato.DddComercial     
        public string DcDispositivoComercialCrianca { get; set; }      //Contato.TelefoneComercial    
        public string CdDddResidencialCrianca { get; set; }            //Contato.DddResidencial
        public string DcDispositivoResidencialCrianca { get; set; }    //Contato.TelefoneResidencial 
        public string EmailCrianca { get; set; }                         //Contato.Email

        public string FoneticaNmPaiCrianca { get; set; }
        public string FoneticaNmCrianca { get; set; }
        public string DtNascimentoCriancaFormatada
        {
            get
            {
                if (DtNascimentoCrianca != null)
                {
                    return Convert.ToDateTime(DtNascimentoCrianca).ToString("dd/MM/yyyy");
                }
                else
                {
                    return "00/00/0000";
                }

            }
        }

    }
}
