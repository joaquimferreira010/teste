using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Constants
{
    public static class Constantes_Example
    {
        public static readonly string CLAIMS_CPF_CNPJ = "http://prodamsp/security/messagetype/identity/claims/codigopessoa";
        public static readonly string CLAIMS_TIPO_PESSOA = "http://prodamsp/security/messagetype/identity/claims/tipopessoa";
        public static readonly string CLAIMS_NOME_PESSOA = "http://prodamsp/security/messagetype/identity/claims/nomepessoa";

        public const string MAXIMO_DE_REGISTROS_POR_PAGINA = "MAXIMO_DE_REGISTROS_POR_PAGINA";
        public const string PASSWORD_HASH = "P@ss0dCAC";
        public const string AMBIENTE_EXECUCAO = "AmbienteExecucao";
        public const string SALT_KEY = "S@ltkeyCACWeb";
        public const string VI_KEY = "@2c4&6G8i0kCACWeb";

        public struct TIPO_GERADOR_OPERACAO
        {
            public const string EMPREITEIRA = "EMPR";
            public const string REPARTICAO_PUBLICA = "RP";
            public const string SUBPREFEITURA = "AD";
            public const string SECRETARIA = "SEC";

            public const string SUBPREFEITURA_V2 = "AR"; // Verificar com analista
        }

        public struct TIPO_VEICULO
        {
            public const string EMPREITEIRA = "VE";
            public const string CARRETA = "CAR";
        }

        public struct TIPO_DOCUMENTO_LIMPURB
        {
            public const string CONTRATO = "CONTUC";
        }

        public struct TIPO_MAO_OBRA
        {
            public const string DIRETA = "D";
            public const string INDIRETA = "I";
        }

        public struct DATA_BASE_REAJUSTE
        {
            public const string CONTRATO = "C";
            public const string PROPOSTA = "P";
        }

        public struct INDICADOR_CONTRATO_SAUDE
        {
            public const string NAO = "N";
            public const string SIM = "S";

        }

        public struct TIPO_RESIDUO
        {
            public const string ESPECIAL = "RE";
           
        }

        public struct TIPO_APLICACAO
        {
            public const int SISRH = 3;
        }
    }

    public static class DropDown
    {
        public const string OPCAO_SELECIONE = "Selecione";
        public const string VALOR_PADRAO_SELECIONE = "";

        public const string OPCAO_TODAS = "Todas";
        public const int VALOR_PADRAO_TODAS = -1;

        public const string OPCAO_TODOS = "Todos";
        public const string VALOR_PADRAO_TODOS = "";

        public const string OPCAO_OUTRO = "Outro";
        public const int VALOR_PADRAO_OUTRO = -1;

        public const string OPCAO_ATIVO = "Ativo";
        public const int VALOR_PADRAO_ATIVO = 1;

        public const string OPCAO_INATIVO = "Inativo";
        public const int VALOR_PADRAO_INATIVO = 0;

        public const string OPCAO_SIM = "Sim";
        public const int VALOR_PADRAO_SIM = 1;

        public const string OPCAO_NAO = "Não";
        public const int VALOR_PADRAO_NAO = 0;

        public const string OPCAO_RAIZ = "-- (raiz) --";
        public const int VALOR_PADRAO_RAIZ = 999;

        public const string OPCAO_CANCELADO = "Cancelado";
        public const int VALOR_PADRAO_CANCELADO = 2;

        public const string OPCAO_UNIDADE = "-- nenhuma --";
        public const string VALOR_PADRAO_NENHUMA = "";
    }

}
