using ProdamSP.Domain.Interfaces.Repository;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Interfaces.Repository.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Data.Common;
using ProdamSP.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace ProdamSP.Data.RepositoryImpl
{

    public class SolicitacaoMatriculaPreNatalRepository : RepositoryBase<SolicitacaoMatriculaPreNatal, int>, ISolicitacaoMatriculaPreNatalRepository
    {
        IUnitOfWork _unitOfWork;
        public SolicitacaoMatriculaPreNatalRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<SolicitacaoMatriculaPreNatal> ConsultarSolicitacao(string numeroCns, string numeroSisPreNatal)
        {
            var objSolicitacaoMatriculaPreNatal = _context.SolicitacaoMatriculaPreNatal
                                                         .Where(x => x.NrCnsResponsavel.Trim() == numeroCns.Trim() 
                                                                    && Convert.ToInt64(x.NrPreNatal.Trim()) == Convert.ToInt64(numeroSisPreNatal.Trim()))
                                                         .OrderBy(x => x.CdSolicitacaoMatriculaPreNatal)
                                                         .FirstOrDefault();
           
            return objSolicitacaoMatriculaPreNatal;
        }

        public async Task<List<SolicitacaoMatriculaPreNatal>> ConsultarSolicitacoes(string numeroCns, string numeroSisPreNatal, int codSolicMatricula = 0)
        {

            List<SolicitacaoMatriculaPreNatal> listSolicitacaoMatriculaPreNatal;
            

            if (codSolicMatricula != 0)
            {

                listSolicitacaoMatriculaPreNatal = _context.SolicitacaoMatriculaPreNatal
                                .Where(s => s.NrCnsResponsavel == numeroCns
                                            && Convert.ToInt64(s.NrPreNatal.Trim()) == Convert.ToInt64(numeroSisPreNatal.Trim())
                                            && s.CdSolicitacaoMatricula == codSolicMatricula
                                            && s.CdSolicitacaoMatriculaNavigation.CdAlunoNavigation.DtExclusao == null)
                                .Include(s => s.NrCnsResponsavelNavigation)
                                    .ThenInclude(respCns => respCns.CdResponsavelGeralNavigation)
                                    .ThenInclude(respGeral => respGeral.CdEnderecoNavigation)
                                    .ThenInclude(tipoLog => tipoLog.TpLogradouroNavigation)
                                .Include(s => s.CdSolicitacaoMatriculaNavigation)
                                    .ThenInclude(solicitacaoMatricula => solicitacaoMatricula.CdAlunoNavigation)
                                .Include(s => s.NrCnsResponsavelNavigation)
                                    .ThenInclude(respCns => respCns.CdResponsavelGeralNavigation)
                                    .ThenInclude(respGeral => respGeral.DispositivoComunicacaoResponsavel)
                                .ToList<SolicitacaoMatriculaPreNatal>();

            }
            else
            {
                listSolicitacaoMatriculaPreNatal = _context.SolicitacaoMatriculaPreNatal
                                                .Where(s => s.NrCnsResponsavel == numeroCns && Convert.ToInt64(s.NrPreNatal.Trim()) == Convert.ToInt64(numeroSisPreNatal.Trim())
                                                            && s.CdSolicitacaoMatriculaNavigation.CdAlunoNavigation.DtExclusao == null)
                                                .Include(s => s.NrCnsResponsavelNavigation)
                                                    .ThenInclude(respCns => respCns.CdResponsavelGeralNavigation)
                                                    .ThenInclude(respGeral => respGeral.CdEnderecoNavigation)
                                                    .ThenInclude(tipoLog => tipoLog.TpLogradouroNavigation)
                                                .Include(s => s.CdSolicitacaoMatriculaNavigation)
                                                    .ThenInclude(solicitacaoMatricula => solicitacaoMatricula.CdAlunoNavigation)
                                                .Include(s => s.NrCnsResponsavelNavigation)
                                                    .ThenInclude(respCns => respCns.CdResponsavelGeralNavigation)
                                                    .ThenInclude(respGeral => respGeral.DispositivoComunicacaoResponsavel)
                                                .ToList<SolicitacaoMatriculaPreNatal>();

            }     

            return listSolicitacaoMatriculaPreNatal;
        }

        public async Task<PreNatalRetornoInclusaoModel> InserirMaePaulistanaPreNatal(PreNatalModel preNatalModel)
        {
            if (preNatalModel == null)
            {
                throw new Exception("O objeto pré-natal não foi populado.");
            }
            if (preNatalModel.MatriculaPreNatal == null)
            {
                throw new Exception("O objeto pré-natal não foi populado - matrícula.");
            }
            if (preNatalModel.Responsavel == null)
            {
                throw new Exception("O objeto pré-natal não foi populado - responsável");
            }
            if (preNatalModel.Aluno == null)
            {
                throw new Exception("O objeto pré-natal não foi populado - aluno");
            }

            var matriculaPreNatal = preNatalModel.MatriculaPreNatal;
            var responsavel = preNatalModel.Responsavel;
            var aluno = preNatalModel.Aluno;

            PreNatalRetornoInclusaoModel objPreNatalRetornoInclusaoModel = new PreNatalRetornoInclusaoModel();

            var objDb = _context.Database.GetDbConnection();

            try
            {
                if (!string.IsNullOrEmpty(matriculaPreNatal.NrPreNatal))
                {
                    //-->não considerar zeros à esquerda
                    Int64 numeroSisPreNatalInt = 0;
                    numeroSisPreNatalInt = Convert.ToInt64(matriculaPreNatal.NrPreNatal);
                    matriculaPreNatal.NrPreNatal = numeroSisPreNatalInt.ToString();
                    //-->
                }
                
                //using (var objDb = _context.Database.GetDbConnection())
                //{ 

                if (objDb.State != ConnectionState.Open)
                {
                    objDb.Open();
                }

                var objCmdStoredProc = objDb.CreateCommand();
                    objCmdStoredProc.CommandType = CommandType.StoredProcedure;
                    objCmdStoredProc.CommandText = "p_rot_ins_mae_paulistana_pre_natal";

                    objCmdStoredProc.CommandTimeout = 300;

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@usuario", String.IsNullOrEmpty(preNatalModel.Usuario) ? "" : preNatalModel.Usuario));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@senha", String.IsNullOrEmpty(preNatalModel.Senha) ? "" : preNatalModel.Senha));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@qtde_crianca_gestacao", preNatalModel.QtdeCriancaGestacao.ToString()));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_mae_crianca", String.IsNullOrEmpty(aluno.NmMaeAluno) ? "" : aluno.NmMaeAluno));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_nascimento_prevista", (matriculaPreNatal.DtNascimentoPrevista == DateTime.MinValue ? "" : matriculaPreNatal.DtNascimentoPrevista.ToString("yyyyMMdd"))));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@tp_raca_cor", (!aluno.TpRacaCor.HasValue ? "" : aluno.TpRacaCor.ToString())));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_pais_origem_mae", String.IsNullOrEmpty(aluno.CdPaisOrigemMae) ? "" : aluno.CdPaisOrigemMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_tp_logradouro", string.IsNullOrEmpty(responsavel.DcTpLogradouro) ? "" : responsavel.DcTpLogradouro));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_logradouro", String.IsNullOrEmpty(responsavel.NmLogradouro) ? "" : responsavel.NmLogradouro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_nr_endereco", String.IsNullOrEmpty(responsavel.CdNrEndereco) ? "" : responsavel.CdNrEndereco));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_complemento_endereco", String.IsNullOrEmpty(responsavel.DcComplementoEndereco) ? "" : responsavel.DcComplementoEndereco));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_bairro", String.IsNullOrEmpty(responsavel.NmBairro) ? "" : responsavel.NmBairro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_municipio", String.IsNullOrEmpty(responsavel.NmMunicipio) ? "" : responsavel.NmMunicipio));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cep", responsavel.CdCep == 0 ? "" : String.Format("{0:00000000}", responsavel.CdCep.ToString())));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cpf_responsavel", (Convert.ToInt64(responsavel.CdCpfResponsavel) == 0 ? "" : Convert.ToInt64(responsavel.CdCpfResponsavel).ToString())));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@email_responsavel", String.IsNullOrEmpty(responsavel.NmEmailResponsavel) ? "" : responsavel.NmEmailResponsavel));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_residencial_mae", String.IsNullOrEmpty(responsavel.CdDddTelefoneFixoResponsavel) ? "" : responsavel.CdDddTelefoneFixoResponsavel.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_residencial_mae", String.IsNullOrEmpty(responsavel.NrTelefoneFixoResponsavel) ? "" : responsavel.NrTelefoneFixoResponsavel.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_comercial_mae", String.IsNullOrEmpty(responsavel.CdDddTelefoneComercialResponsavel) ? "" : responsavel.CdDddTelefoneComercialResponsavel.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_comercial_mae", String.IsNullOrEmpty(responsavel.NrTelefoneComercialResponsavel) ? "" : responsavel.NrTelefoneComercialResponsavel.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_celular_mae", String.IsNullOrEmpty(responsavel.CdDddCelularResponsavel) ? "" : responsavel.CdDddCelularResponsavel.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_celular_mae", String.IsNullOrEmpty(responsavel.NrCelularResponsavel) ? "" : responsavel.NrCelularResponsavel.Trim()));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_cns_responsavel", String.IsNullOrEmpty(matriculaPreNatal.NrCnsResponsavel) ? "" : matriculaPreNatal.NrCnsResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_pre_natal", String.IsNullOrEmpty(matriculaPreNatal.NrPreNatal) ? "" : matriculaPreNatal.NrPreNatal));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_inicio_pre_natal", (!matriculaPreNatal.DtInicioPreNatal.HasValue ? "" : Convert.ToDateTime(matriculaPreNatal.DtInicioPreNatal).ToString("yyyyMMdd"))));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_origem_local_alteracao", matriculaPreNatal.DcOrigemLocalAlteracao.ToString()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_intencao_matricula", (matriculaPreNatal.DtIntencaoMatricula == DateTime.MinValue ? "" : matriculaPreNatal.DtIntencaoMatricula.ToString("yyyyMMdd"))));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_rg_responsavel", String.IsNullOrEmpty(responsavel.NrRgResponsavel) ? "" : responsavel.NrRgResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@sg_uf_rg_responsavel", String.IsNullOrEmpty(responsavel.SgUfRgResponsavel) ? "" : responsavel.SgUfRgResponsavel));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_avo_crianca", String.IsNullOrEmpty(responsavel.NmMaeResponsavel) ? "" : responsavel.NmMaeResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_nascimento_mae_crianca", (responsavel.DtNascimentoMaeResponsavel == DateTime.MinValue ? "" : Convert.ToDateTime(responsavel.DtNascimentoMaeResponsavel).ToString("yyyyMMdd"))));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_documento_estrangeiro", (String.IsNullOrEmpty(responsavel.NrDocumentoEstrangeiro) ? "" : responsavel.NrDocumentoEstrangeiro)));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@fonetica_nm_mae_crianca", String.IsNullOrEmpty(aluno.NmMaeAlunoFonetico) ? "" : aluno.NmMaeAlunoFonetico));


                    
                using (IDataReader objDrResults = objCmdStoredProc.ExecuteReader())
                {
                    while (objDrResults.Read())
                    {
                        objPreNatalRetornoInclusaoModel.Retorno = objDrResults["retorno"].ToString();
                        objPreNatalRetornoInclusaoModel.Mensagem = objDrResults["mensagem"].ToString();
                        objPreNatalRetornoInclusaoModel.CdAluno = objDrResults["cd_aluno"].ToString();
                        objPreNatalRetornoInclusaoModel.CdResponsavelGeral = objDrResults["cd_responsavel_geral"].ToString();
                        objPreNatalRetornoInclusaoModel.CdSolicitacaoMatriculaPreNatal = objDrResults["cd_solicitacao_matricula_pre_natal"].ToString();
                        objPreNatalRetornoInclusaoModel.CdSolicitacaoMatricula = objDrResults["cd_solicitacao_matricula"].ToString();
                    }
                }
                objDb.Close();
                //}
            }
            catch (Exception ex)
            {
                objDb.Close();
                throw ex;
            }

            return objPreNatalRetornoInclusaoModel;
        }

        public async Task<PreNatalRetornoExclusaoModel> ExcluirMaePaulistanaPreNatal(PreNatalExclusaoModel preNatalExclusaoModel)
        {

            if (preNatalExclusaoModel == null)
            {
                throw new Exception("Dados para exclusão do pré-natal inválidos");
            }

            PreNatalRetornoExclusaoModel objPreNatalRetornoExclusaoModel = new PreNatalRetornoExclusaoModel();
            var objDb = _context.Database.GetDbConnection();
            try
            {
                //using (var objDb = _context.Database.GetDbConnection())
                //{
                if (objDb.State != ConnectionState.Open)
                {
                    objDb.Open();
                }
                var objCmdStoredProc = objDb.CreateCommand();
                    objCmdStoredProc.CommandType = CommandType.StoredProcedure;
                    objCmdStoredProc.CommandText = "p_rot_del_mae_paulistana_pre_natal";

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@usuario", String.IsNullOrEmpty(preNatalExclusaoModel.Usuario) ? "" : preNatalExclusaoModel.Usuario));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@senha", String.IsNullOrEmpty(preNatalExclusaoModel.Senha) ? "" : preNatalExclusaoModel.Senha));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@qtde_crianca_gestacao", preNatalExclusaoModel.QtdeCriancaGestacao.ToString()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_mae_crianca", String.IsNullOrEmpty(preNatalExclusaoModel.NmMaeCrianca) ? "" : preNatalExclusaoModel.NmMaeCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_nascimento_prevista", (preNatalExclusaoModel.DtNascimentoPrevista == DateTime.MinValue ? "" : preNatalExclusaoModel.DtNascimentoPrevista.ToString("yyyyMMdd"))));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_motivo_cancelamento_inscricao", String.IsNullOrEmpty(preNatalExclusaoModel.CdMotivoCancelamentoInscricao) ? "" : preNatalExclusaoModel.CdMotivoCancelamentoInscricao));

                    

                using (IDataReader objDrResults = objCmdStoredProc.ExecuteReader())
                {
                    while (objDrResults.Read())
                    {
                        objPreNatalRetornoExclusaoModel.Retorno = objDrResults["retorno"].ToString();
                        objPreNatalRetornoExclusaoModel.Mensagem = objDrResults["mensagem"].ToString();
                    }
                }

                objDb.Close();
            //    }
            }
            catch (Exception ex)
            {
                objDb.Close();
                throw ex;
            }

            return objPreNatalRetornoExclusaoModel;
        }


        public async Task<StatusPreNatalRetornoModel> AtualizarStatusMaePaulistanaPreNatal(StatusPreNatalModel statusPreNatalModel)
        {

            if (statusPreNatalModel == null)
            {
                throw new Exception("Dados para atualização do status do pré-natal inválidos");
            }

            StatusPreNatalRetornoModel objStatusPreNatalRetornoModel = new StatusPreNatalRetornoModel();

            try
            {
                if (!string.IsNullOrEmpty(statusPreNatalModel.NumeroSISPRENATAL))
                {
                    //-->não considerar zeros à esquerda
                    Int64 numeroSisPreNatalInt = 0;
                    numeroSisPreNatalInt = Convert.ToInt64(statusPreNatalModel.NumeroSISPRENATAL);
                    statusPreNatalModel.NumeroSISPRENATAL = numeroSisPreNatalInt.ToString();
                    //-->
                }

                using (var objDb = _context.Database.GetDbConnection())
                {
                    objDb.Open();
                    var objCmdStoredProc = objDb.CreateCommand();
                    objCmdStoredProc.CommandType = CommandType.StoredProcedure;
                    objCmdStoredProc.CommandText = "p_rot_upd_mae_paulistana_pre_natal_atualizar_status";

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@usuario", String.IsNullOrEmpty(statusPreNatalModel.Usuario) ? "" : statusPreNatalModel.Usuario));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@senha", String.IsNullOrEmpty(statusPreNatalModel.Senha) ? "" : statusPreNatalModel.Senha));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_cns_responsavel", String.IsNullOrEmpty(statusPreNatalModel.NumeroCns) ? "" : statusPreNatalModel.NumeroCns));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_pre_natal", String.IsNullOrEmpty(statusPreNatalModel.NumeroSISPRENATAL) ? "" : statusPreNatalModel.NumeroSISPRENATAL));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_nascimento_prevista", (statusPreNatalModel.DataPrevisaoParto == null) ? "" : Convert.ToDateTime(statusPreNatalModel.DataPrevisaoParto).ToString("yyyyMMdd")));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_ultima_consulta_gestante", (statusPreNatalModel.DataUltimaConsulta == null) ? "" : Convert.ToDateTime(statusPreNatalModel.DataUltimaConsulta).ToString("yyyyMMdd")));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_cancelamento_inscricao_eol", (statusPreNatalModel.DataInterrupcao == null) ? "" : Convert.ToDateTime(statusPreNatalModel.DataInterrupcao).ToString("yyyyMMdd")));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_motivo_cancelamento_inscricao", (string.IsNullOrEmpty(statusPreNatalModel.CodigoMotivoInterrupcao) ? "" : statusPreNatalModel.CodigoMotivoInterrupcao)));

                    using (IDataReader objDrResults = objCmdStoredProc.ExecuteReader())
                    {
                        while (objDrResults.Read())
                        {
                            objStatusPreNatalRetornoModel.Retorno = objDrResults["retorno"].ToString();
                            objStatusPreNatalRetornoModel.Mensagem = objDrResults["mensagem"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objStatusPreNatalRetornoModel;
        }


        public async Task<DadosCadastraisPreNatalAtualizacaoRetornoModel> AtualizarDadosCadastraisPreNatal(DadosCadastraisPreNatalAtualizacaoModel dadosCadastraisPreNatalAtualizacaoModel)
        {

            if (dadosCadastraisPreNatalAtualizacaoModel == null)
            {
                throw new Exception("Dados cadastrais para atualização do pré-natal inválidos");
            }

            DadosCadastraisPreNatalAtualizacaoRetornoModel objDadosCadastraisPreNatalAtualizacaoRetornoModel = new DadosCadastraisPreNatalAtualizacaoRetornoModel();

            try
            {
                using (var objDb = _context.Database.GetDbConnection())
                {
                    objDb.Open();
                    var objCmdStoredProc = objDb.CreateCommand();
                    objCmdStoredProc.CommandType = CommandType.StoredProcedure;
                    objCmdStoredProc.CommandText = "p_rot_upd_mae_paulistana_pre_natal_dados_cadastrais";

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@usuario", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.Usuario) ? "" : dadosCadastraisPreNatalAtualizacaoModel.Usuario));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@senha", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.Senha) ? "" : dadosCadastraisPreNatalAtualizacaoModel.Senha));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_cns_responsavel", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NrCnsResponsavel) ? "" : dadosCadastraisPreNatalAtualizacaoModel.NrCnsResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_mae_crianca", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NmMaeCrianca) ? "" : dadosCadastraisPreNatalAtualizacaoModel.NmMaeCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_nascimento_mae_crianca", (dadosCadastraisPreNatalAtualizacaoModel.DtNascimentoMaeCrianca == null) ? "" : Convert.ToDateTime(dadosCadastraisPreNatalAtualizacaoModel.DtNascimentoMaeCrianca).ToString("yyyyMMdd")));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_pais_origem_mae", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.CdPaisOrigemMae) ? "" : dadosCadastraisPreNatalAtualizacaoModel.CdPaisOrigemMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cpf_responsavel", (Convert.ToInt64(dadosCadastraisPreNatalAtualizacaoModel.CdCpfResponsavel) == 0 ? "" : Convert.ToInt64(dadosCadastraisPreNatalAtualizacaoModel.CdCpfResponsavel).ToString())));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_rg_responsavel", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NrRgResponsavel) ? "" : dadosCadastraisPreNatalAtualizacaoModel.NrRgResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@sg_uf_rg_responsavel", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.SgUfRgResponsavel) ? "" : dadosCadastraisPreNatalAtualizacaoModel.SgUfRgResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_tp_logradouro", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.DcTpLogradouro) ? "" : dadosCadastraisPreNatalAtualizacaoModel.DcTpLogradouro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_logradouro", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NmLogradouro) ? "" : dadosCadastraisPreNatalAtualizacaoModel.NmLogradouro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_nr_endereco", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.CdNrEndereco) ? "" : dadosCadastraisPreNatalAtualizacaoModel.CdNrEndereco));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_complemento_endereco", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.DcComplementoEndereco) ? "" : dadosCadastraisPreNatalAtualizacaoModel.DcComplementoEndereco));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_bairro", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NmBairro) ? "" : dadosCadastraisPreNatalAtualizacaoModel.NmBairro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cep", dadosCadastraisPreNatalAtualizacaoModel.CdCep == 0 ? "" : String.Format("{0:00000000}", dadosCadastraisPreNatalAtualizacaoModel.CdCep.ToString())));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_municipio", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NmMunicipio) ? "" : dadosCadastraisPreNatalAtualizacaoModel.NmMunicipio));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_celular_mae", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.CdDddCelularMae) ? "" : dadosCadastraisPreNatalAtualizacaoModel.CdDddCelularMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_celular_mae", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.DcDispositivoCelularMae) ? "" : dadosCadastraisPreNatalAtualizacaoModel.DcDispositivoCelularMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_comercial_mae", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.CdDddComercialMae) ? "" : dadosCadastraisPreNatalAtualizacaoModel.CdDddComercialMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_comercial_mae", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.DcDispositivoComercialMae) ? "" : dadosCadastraisPreNatalAtualizacaoModel.DcDispositivoComercialMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_residencial_mae", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.CdDddResidencialMae) ? "" : dadosCadastraisPreNatalAtualizacaoModel.CdDddResidencialMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_residencial_mae", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.DcDispositivoResidencialMae) ? "" : dadosCadastraisPreNatalAtualizacaoModel.DcDispositivoResidencialMae));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@email_responsavel", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.EmailResponsavel) ? "" : dadosCadastraisPreNatalAtualizacaoModel.EmailResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_documento_estrangeiro", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NrDocumentoEstrangeiro) ? "" : dadosCadastraisPreNatalAtualizacaoModel.NrDocumentoEstrangeiro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@fonetica_nm_mae_crianca", String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.FoneticaNmMaeCrianca) ? "" : dadosCadastraisPreNatalAtualizacaoModel.FoneticaNmMaeCrianca));

                   using (IDataReader objDrResults = objCmdStoredProc.ExecuteReader())
                    {
                        while (objDrResults.Read())
                        {
                            objDadosCadastraisPreNatalAtualizacaoRetornoModel.Retorno = objDrResults["retorno"].ToString();
                            objDadosCadastraisPreNatalAtualizacaoRetornoModel.Mensagem = objDrResults["mensagem"].ToString();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objDadosCadastraisPreNatalAtualizacaoRetornoModel;
        }


        public async Task<SolicitacaoVagaRetornoModel> SolicitarVaga(SolicitacaoVagaModel solicitacaoVagaModel)
        {

            if (solicitacaoVagaModel == null)
            {
                throw new Exception("Dados para solicitação de vaga inválidos");
            }

            SolicitacaoVagaRetornoModel objSolicitacaoVagaRetornoModel = new SolicitacaoVagaRetornoModel();

            try
            {
                if (!string.IsNullOrEmpty(solicitacaoVagaModel.NrPreNatal))
                {
                    //-->não considerar zeros à esquerda
                    Int64 numeroSisPreNatalInt = 0;
                    numeroSisPreNatalInt = Convert.ToInt64(solicitacaoVagaModel.NrPreNatal);
                    solicitacaoVagaModel.NrPreNatal = numeroSisPreNatalInt.ToString();
                    //-->
                }

                var dadosMae = solicitacaoVagaModel.DadosMae;
                var dadosCrianca = solicitacaoVagaModel.DadosCrianca;

                using (var objDb = _context.Database.GetDbConnection())
                {
                    objDb.Open();
                    var objCmdStoredProc = objDb.CreateCommand();
                    objCmdStoredProc.CommandType = CommandType.StoredProcedure;
                    objCmdStoredProc.CommandText = "p_rot_upd_mae_paulistana_pre_natal_transformacao";

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@usuario", String.IsNullOrEmpty(solicitacaoVagaModel.Usuario) ? "" : solicitacaoVagaModel.Usuario));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@senha", String.IsNullOrEmpty(solicitacaoVagaModel.Senha) ? "" : solicitacaoVagaModel.Senha));

                    //--Atributos(geral)
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_pre_natal", String.IsNullOrEmpty(solicitacaoVagaModel.NrPreNatal) ? "" : solicitacaoVagaModel.NrPreNatal));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_solicitacao_matricula", String.IsNullOrEmpty(solicitacaoVagaModel.CdSolicitacaoMatricula) ? "" : solicitacaoVagaModel.CdSolicitacaoMatricula));

                    //--Atributos(responsavel - mãe)
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_cns_responsavel", String.IsNullOrEmpty(solicitacaoVagaModel.NrCnsResponsavel) ? "" : solicitacaoVagaModel.NrCnsResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_mae_crianca", String.IsNullOrEmpty(dadosMae.NmMaeCrianca) ? "" : dadosMae.NmMaeCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_nascimento_mae_crianca", (dadosMae.DtNascimentoMaeCrianca.HasValue ? Convert.ToDateTime(dadosMae.DtNascimentoMaeCrianca).ToString("yyyyMMdd") : "")));

                    //documento mae
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_pais_origem_mae", String.IsNullOrEmpty(dadosMae.CdPaisOrigemMae) ? "" : dadosMae.CdPaisOrigemMae));
                    if (dadosMae.CdCpfResponsavel.HasValue)
                    {
                        objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cpf_responsavel", (Convert.ToInt64(dadosMae.CdCpfResponsavel) == 0 ? "" : Convert.ToInt64(dadosMae.CdCpfResponsavel).ToString())));
                    }
                    else {
                        objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cpf_responsavel", ""));
                    }
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_rg_responsavel", String.IsNullOrEmpty(dadosMae.NrRgResponsavel) ? "" : dadosMae.NrRgResponsavel.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@sg_uf_rg_responsavel", String.IsNullOrEmpty(dadosMae.SgUfRgResponsavel) ? "" : dadosMae.SgUfRgResponsavel));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_documento_estrangeiro_responsavel", String.IsNullOrEmpty(dadosMae.NrDocumentoEstrangeiro) ? "" : dadosMae.NrDocumentoEstrangeiro));

                    //endereco mae
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_tp_logradouro_mae", String.IsNullOrEmpty(dadosMae.DcTpLogradouro) ? "" : dadosMae.DcTpLogradouro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_logradouro_mae", String.IsNullOrEmpty(dadosMae.NmLogradouro) ? "" : dadosMae.NmLogradouro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_nr_endereco_mae", String.IsNullOrEmpty(dadosMae.CdNrEndereco) ? "" : dadosMae.CdNrEndereco));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_complemento_endereco_mae", String.IsNullOrEmpty(dadosMae.DcComplementoEndereco) ? "" : dadosMae.DcComplementoEndereco));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_bairro_mae", String.IsNullOrEmpty(dadosMae.NmBairro) ? "" : dadosMae.NmBairro));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cep_mae", dadosMae.CdCep == 0 || dadosMae.CdCep == null ? "" : String.Format("{0:00000000}", dadosMae.CdCep.ToString())));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_municipio_mae", String.IsNullOrEmpty(dadosMae.NmMunicipio) ? "" : dadosMae.NmMunicipio));

                    //contato mae
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_celular_mae", String.IsNullOrEmpty(dadosMae.CdDddCelularMae) ? "" : dadosMae.CdDddCelularMae.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_celular_mae", String.IsNullOrEmpty(dadosMae.DcDispositivoCelularMae) ? "" : dadosMae.DcDispositivoCelularMae.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_comercial_mae", String.IsNullOrEmpty(dadosMae.CdDddComercialMae) ? "" : dadosMae.CdDddComercialMae.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_comercial_mae", String.IsNullOrEmpty(dadosMae.DcDispositivoComercialMae) ? "" : dadosMae.DcDispositivoComercialMae.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_residencial_mae", String.IsNullOrEmpty(dadosMae.CdDddResidencialMae) ? "" : dadosMae.CdDddResidencialMae.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_residencial_mae", String.IsNullOrEmpty(dadosMae.DcDispositivoResidencialMae) ? "" : dadosMae.DcDispositivoResidencialMae.Trim()));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@email_responsavel", String.IsNullOrEmpty(dadosMae.EmailResponsavel) ? "" : dadosMae.EmailResponsavel));


                    //--Atributos(crianca)
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_cns_crianca", String.IsNullOrEmpty(dadosCrianca.NrCnsCrianca) ? "" : dadosCrianca.NrCnsCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_crianca", String.IsNullOrEmpty(dadosCrianca.NmCrianca) ? "" : dadosCrianca.NmCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_pai_crianca", String.IsNullOrEmpty(dadosCrianca.NmPai) ? "" : dadosCrianca.NmPai));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dt_nascimento_crianca", (!dadosCrianca.DtNascimentoCrianca.HasValue ? "" : Convert.ToDateTime(dadosCrianca.DtNascimentoCrianca).ToString("yyyyMMdd"))));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_sexo_crianca", String.IsNullOrEmpty(dadosCrianca.SexoCrianca) ? "" : dadosCrianca.SexoCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@tp_raca_cor_crianca", (!dadosCrianca.TpRacaCorCrianca.HasValue ? "" : dadosCrianca.TpRacaCorCrianca.ToString())));

                    //documento crianca
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_pais_origem_crianca", String.IsNullOrEmpty(dadosCrianca.CdPaisOrigemCrianca) ? "" : dadosCrianca.CdPaisOrigemCrianca));
                    if (dadosCrianca.CdCpfCrianca.HasValue){
                        objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cpf_crianca", (Convert.ToInt64(dadosCrianca.CdCpfCrianca) == 0 ? "" : Convert.ToInt64(dadosCrianca.CdCpfCrianca).ToString())));
                    }
                    else {
                        objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cpf_crianca", ""));
                    }
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_rg_crianca", String.IsNullOrEmpty(dadosCrianca.NrRgCrianca) ? "" : dadosCrianca.NrRgCrianca.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@sg_uf_rg_crianca", String.IsNullOrEmpty(dadosCrianca.SgUfRgCrianca) ? "" : dadosCrianca.SgUfRgCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nr_documento_estrangeiro_crianca", String.IsNullOrEmpty(dadosCrianca.NrDocumentoEstrangeiroCrianca) ? "" : dadosCrianca.NrDocumentoEstrangeiroCrianca));

                    //endereco crianca
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_tp_logradouro_crianca", String.IsNullOrEmpty(dadosCrianca.DcTpLogradouroCrianca) ? "" : dadosCrianca.DcTpLogradouroCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_logradouro_crianca", String.IsNullOrEmpty(dadosCrianca.NmLogradouroCrianca) ? "" : dadosCrianca.NmLogradouroCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_nr_endereco_crianca", String.IsNullOrEmpty(dadosCrianca.CdNrEnderecoCrianca) ? "" : dadosCrianca.CdNrEnderecoCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_complemento_endereco_crianca", String.IsNullOrEmpty(dadosCrianca.DcComplementoEnderecoCrianca) ? "" : dadosCrianca.DcComplementoEnderecoCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_bairro_crianca", String.IsNullOrEmpty(dadosCrianca.NmBairroCrianca) ? "" : dadosCrianca.NmBairroCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_cep_crianca", dadosCrianca.CdCepCrianca == 0 || dadosCrianca.CdCepCrianca == null ? "" : String.Format("{0:00000000}", dadosCrianca.CdCepCrianca.ToString())));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@nm_municipio_crianca", String.IsNullOrEmpty(dadosCrianca.NmMunicipioCrianca) ? "" : dadosCrianca.NmMunicipioCrianca));

                    //contato crianca
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_celular_crianca", String.IsNullOrEmpty(dadosCrianca.CdDddCelularCrianca) ? "" : dadosCrianca.CdDddCelularCrianca.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_celular_crianca", String.IsNullOrEmpty(dadosCrianca.DcDispositivoCelularCrianca) ? "" : dadosCrianca.DcDispositivoCelularCrianca.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_comercial_crianca", String.IsNullOrEmpty(dadosCrianca.CdDddComercialCrianca) ? "" : dadosCrianca.CdDddComercialCrianca.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_comercial_crianca", String.IsNullOrEmpty(dadosCrianca.DcDispositivoComercialCrianca) ? "" : dadosCrianca.DcDispositivoComercialCrianca.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@cd_ddd_residencial_crianca", String.IsNullOrEmpty(dadosCrianca.CdDddResidencialCrianca) ? "" : dadosCrianca.CdDddResidencialCrianca.Trim()));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@dc_dispositivo_residencial_crianca", String.IsNullOrEmpty(dadosCrianca.DcDispositivoResidencialCrianca) ? "" : dadosCrianca.DcDispositivoResidencialCrianca.Trim()));

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@email_crianca", String.IsNullOrEmpty(dadosCrianca.EmailCrianca) ? "" : dadosCrianca.EmailCrianca));

                    //Fonetica
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@fonetica_nm_mae_crianca", String.IsNullOrEmpty(dadosMae.FoneticaNmMaeCrianca) ? "" : dadosMae.FoneticaNmMaeCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@fonetica_nm_pai_crianca", String.IsNullOrEmpty(dadosCrianca.FoneticaNmPaiCrianca) ? "" : dadosCrianca.FoneticaNmPaiCrianca));
                    objCmdStoredProc.Parameters.Add(new SqlParameter("@fonetica_nm_crianca", String.IsNullOrEmpty(dadosCrianca.FoneticaNmCrianca) ? "" : dadosCrianca.FoneticaNmCrianca));

                    using (IDataReader objDrResults = objCmdStoredProc.ExecuteReader())
                    {
                        while (objDrResults.Read())
                        {
                            objSolicitacaoVagaRetornoModel.Retorno = objDrResults["retorno"].ToString();
                            objSolicitacaoVagaRetornoModel.Mensagem = objDrResults["mensagem"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objSolicitacaoVagaRetornoModel;
        }


        public async Task<string> ConsultarParametroGeral(string nome_parametro)
        {
            //nome_parametro = "nm_parametro_geral = 'qtd_dias_permissão_alterar_intencao_matricula'';
            //cadastro ->"nm_parametro_geral = 'qtd_dias_apos_nascimento'"

            var vl_parametro = "";
            var objDb = _context.Database.GetDbConnection();
            try
            {
                
                //using (var objDb = _context.Database.GetDbConnection())
                //{
                    if (objDb.State != ConnectionState.Open)
                    {
                        objDb.Open();
                    }

                    var objCmdStoredProc = objDb.CreateCommand();
                    objCmdStoredProc.CommandType = CommandType.StoredProcedure;
                    objCmdStoredProc.CommandText = "p_sel_parametro_geral_all";

                    objCmdStoredProc.Parameters.Add(new SqlParameter("@sql_filtro", String.IsNullOrEmpty(nome_parametro) ? "" : nome_parametro));

                using (IDataReader objDrResults = objCmdStoredProc.ExecuteReader())
                {
                    while (objDrResults.Read())
                    {
                        vl_parametro = objDrResults["vl_parametro_geral"].ToString();
                    }
                }

                objDb.Close();
                //}
            }

            catch (Exception ex)
            {
                objDb.Close();
                throw ex;
            }

            return vl_parametro;
        }

    }
}


