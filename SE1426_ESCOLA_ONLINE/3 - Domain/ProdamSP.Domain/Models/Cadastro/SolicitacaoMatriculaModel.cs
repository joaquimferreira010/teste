using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.ProdamSP.Domain.Models.Cadastro
{
    public class SolicitacaoMatriculaModel
    {
        public int CdSolicitacaoMatricula { get; set; }
        public int CdAluno { get; set; }
        public string CdUnidadeEducacao { get; set; }
        public string TpOrigem { get; set; }
        public string StSolicitacao { get; set; }
        public DateTime DtStatusSolicitacao { get; set; }
        public string CdOvperador { get; set; }
        public DateTime DtAtlzTab { get; set; }
        public int? CdMicroRegiao { get; set; }
        public int? CdModalidadeEnsino { get; set; }
        public int? CdEtapaEnsino { get; set; }
        public int CdTipoPrograma { get; set; }
        public int? CdCicloEnsino { get; set; }
        public int? CdSerieEnsino { get; set; }
        public int? CdTipoMotivoRecusaMatricula { get; set; }
        public string DcObservacaoStatus { get; set; }
        public DateTime? DtProtocoloDesistenciaMatricula { get; set; }
        public int? AnLetivo { get; set; }
        public int CdTipoOpcaoTurnoHum { get; set; }
        public int CdTipoOpcaoTurnoDois { get; set; }
        public int CdTipoOpcaoTurnoTres { get; set; }
        public string InRecebimento { get; set; }
    }
}
