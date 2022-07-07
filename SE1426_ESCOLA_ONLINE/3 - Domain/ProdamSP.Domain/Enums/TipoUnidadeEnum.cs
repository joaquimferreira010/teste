using System.ComponentModel;

namespace ProdamSP.Domain.Enums
{
    public enum TipoUnidadeEnum
    {
        [Description("Coordenadoria")]
        COORDENADORIA = 1,

        [Description("Supervisão")]
        SUPERVISAO = 2,

        [Description("Instituição")]
        INSTITUICAO = 3,

        [Description("Unidade")]
        UNIDADE = 4,

        [Description("TipoContratacao")]
        TIPOCONTRATACAO = 5,

        [Description("OrgaoContratante")]
        ORGAOCONTRATANTE = 6,

        [Description("NivelCargo")]
        NIVELCARGO = 7,

        [Description("Cargo")]
        CARGO = 8

    }
}
