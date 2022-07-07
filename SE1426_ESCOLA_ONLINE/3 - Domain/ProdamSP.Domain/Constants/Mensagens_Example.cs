using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Constants
{
    public static class Mensagens_Example
    {
        public static readonly string MS_PERMISSAO_NEGADA = "Permissão Negada.";

        public static readonly string MS_CAD_005_CADASTRO_UNIDADE_JA_EXISTE = "Já existe um cadastro com o mesmo nome e CNES.";
        
        public static readonly string MS_CAD_003_CADASTRO_ATUALIZADO = "Cadastro atualizado com sucesso.";

        public static readonly string MS_CAD_004_CANCELAR_EDICAO_CADASTRO = "Deseja realmente cancelar a edição do cadastro da unidade?";

        public static readonly string MS_CAD_006_CAMPOS_OBRIGATORIO = "É obrigatório o preenchimento dos campos em destaque.";

        public static readonly string MS_CAD_007_CADASTRO_SUCESSO = "Cadastro realizado com sucesso.";

        public static readonly string MS_CAD_008_CADASTRO_JA_EXISTE = "Já existe um cadastro com o mesmo Nome.";

        public static readonly string MS_CAD_009_CADASTRO_CMES_JA_EXISTE = "Já existe um cadastro com o mesmo CMES.";        
        public static readonly string MS_CAD_010_CONFIRMA_EXCLUSAO_HIERARQUIA = "Deseja realmente excluir esta hierarquia?";
        public static readonly string MS_CAD_011_EXCLUSAO_NAO_PERMITIDA = "Não é possível excluir este registro, pois ele está sendo usado em outros componentes.";
        public static readonly string MS_CAD_012_REGISTRO_EXCLUIDO = "Registro excluído com sucesso.";
        public static readonly string MS_CAD_013_AVISO_SAIR_CADASTRO = "Deseja realmente sair do cadastro?";
        public static readonly string MS_PROF_005_REGISTRO_VINCULO_IGUAIS = "Já existe um cadastro do órgão com mesmo Registro e Vínculo para o profissional.";
        public static readonly string MS_PROF_011_DTADMISSAO_ANTERIOR_DATAS_UNIDADES = "Data de admissão inválida, deverá ser anterior às unidades e à data de desligamento se existir.";
        public static readonly string MS_PROF_014_DTFIM_MENOR_DTINICIO = "Data fim menor que a data início.";
        public static readonly string MS_PROF_018_NAO_ALTERAR_ORGAO = "Não será possível alterar o órgão, pois existem cargos x especialidades de unidades que estão atrelados ao órgão.";
        public static readonly string MS_PROF_020_DATA_TERMINO_UNIDADE_DEVE_SER_POSTERIOR_DATA_INICIO = "Data de término da unidade inválida, deverá ser posterior à data de início da unidade, e menor ou igual à data de desligamento se existir.";
        public static readonly string MS_PROF_021_JORNADA_ORGAO_MENOR_QUE_UNIDADES = "Jornada de contratação inválida, está menor que a somatória das jornadas das unidades.";
        public static readonly string MS_PROF_022_JORNADA_UNIDADES_MAIOR_QUE_ORGAO = "Jornada da unidade inválida, a somatória das jornadas das unidades ultrapassa a jornada de contratação do órgão.";
        public static readonly string MS_PROF_023_QUANTIDADE_DIGITOS_INCOMPATIVEL = "Quantidade de dígitos informados incombatível com a máscara cadastrada: XXXX";
        public static readonly string MS_PROF_026_HORARIO_SOBREPOSTO = "Horário inválido, pois existem horários sobrepostos ou mais de um horário para o mesmo dia da semana";
        public static readonly string MS_PROF_031_FALTA_ORGAO_PROFISSIONAL = "Falta informar órgão para o profissional.";
        public static readonly string MS_PROF_032_FALTA_UNIDADE_ORGAO = "Falta informar unidade para o órgão ";
        public static readonly string MS_PROF_033_ORGAO_NAO_PERMITE_MAIS_DE_UMA_UNIDADE_ATIVA = "O órgão só permite uma unidade ativa e existe mais de uma ativa: ";        
        public static readonly string MS_PROF_042_JA_ADIMITIDO_NO_ORGAO = "O profissional já está admitido, ou data de admissão está inválida, para o mesmo órgão.";
        public static readonly string MS_PROF_043_JA_INICIOU_UNIDADE_NO_ORGAO = "O profissional já iniciou, ou data de início está inválida, para a mesma unidade dentro do mesmo órgão.";
        public static readonly string MS_PROF_ORGAO_SITUACAO_ATIVO_SEM_UNIDADES = "O órgão está com situação de ativo, porém não existem unidades ativas para o órgão.";

        public static readonly string MS_CAD_001_SIGLA_JA_EXISTE = "Já existe um cadastro com a mesma Sigla.";
    }
}
