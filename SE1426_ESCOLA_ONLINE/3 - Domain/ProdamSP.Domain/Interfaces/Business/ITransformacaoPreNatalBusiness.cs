using System;
using System.Collections.Generic;
using System.Text;

using ProdamSP.Domain.Interfaces.Business.Common;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Models;
using System.Threading.Tasks;
using SE1426.ProdamSP.Domain.Models.Cadastro;

namespace ProdamSP.Domain.Interfaces.Business

{
    public interface ITransformacaoPreNatalBusiness:IBusiness<SolicitacaoMatriculaPreNatal, int>
    {
        Task<TransformacaoConsultaCriancaRetornoModel> ConsultarDadosCriancaSIGA(TransformacaoConsultaCriancaModel transformacaoConsultaCriancaModel);
        Task<SolicitacaoVagaRetornoModel> SolicitarVaga(SolicitacaoVagaModel solicitacaoVagaModel);

    
    }
}
