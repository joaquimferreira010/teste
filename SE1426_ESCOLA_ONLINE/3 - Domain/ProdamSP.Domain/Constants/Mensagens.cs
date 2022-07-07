using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Constants
{
    public static class Mensagens
    {
        public static readonly string MS_PUB_CPT_005_FILTRO_NAO_INFORMADO = "Campo(s) obrigatório(s) não informado(s).";
        public static readonly string MS_PUB_CPT_005_NUMERO_CNS_OBRIGATORIO = "Número do CNS é obrigatório.";
        public static readonly string MS_PUB_CPT_005_NUMERO_PRE_NATAL_OBRIGATORIO = "Número do cartão Pré-natal é obrigatório.";
        public static readonly string MS_PUB_CPT_005_SOLICITACAO_MATRICULA_PRE_NATAL_NAO_ENCONTRADO = "Cadastro não encontrado para o número CNS e número cartão Pré-natal digitados.";
        public static readonly string MS_PUB_CPT_005_SOLICITACAO_MATRICULA_PRE_NATAL_DESATIVADO_SIGA = "Cadastro com registro de interrupção de acompanhamento. Por favor, procure a unidade de saúde onde realizou o pré-natal.";
        public static readonly string MS_PUB_CPT_005_SOLICITACAO_MATRICULA_PRE_NATAL_DESATIVADO_EOL = "Cadastro desativado, pois não foi informado o nascimento de todas as crianças nos últimos 60 dias. Para reativá-lo, por favor, procure a Unidade de Educação (Escola) mais próxima.";

        public static readonly string MS_PUB_CPT_005_SIGA_PRE_NATAL_SEM_DATA_CADASTRO = "Código de pré-natal encontrado e vinculado corretamente a este CNS, porém com data de cadastro de Pré-natal não informado";
        public static readonly string MS_PUB_CPT_005_SIGA_PRE_NATAL_SEM_DATA_PREVISAO_PARTO = "Código de pré-natal encontrado e vinculado corretamente a este CNS, porém com data de previsão de Parto não informado";
        public static readonly string MS_PUB_CPT_005_SIGA_PRE_NATAL_SEM_DATA_CADASTRO_E_PREVISAO_PARTO = "Código de pré-natal encontrado e vinculado corretamente a este CNS, porém com data de previsão de Parto e data de cadastro de Pré-natal não informados";

        public static readonly string MS_PUB_CPT_012_DADOS_INCONSISTENTES = "Operação não realizada. Erro nas informações fornecidas: ";
        public static readonly string MS_PUB_CPT_010_CEP_NAO_ENCONTRADO = "CEP não localizado";


    }
}
