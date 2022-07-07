using System;
using System.Collections.Generic;
using System.Text;
using ProdamSP.Business.Common;
using ProdamSP.Domain.Entities;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Domain.Interfaces.Repository;
using ProdamSP.Domain.Interfaces.Repository.Common;
using ProdamSP.CrossCutting;
using System.Threading.Tasks;
using System.Linq;
using ProdamSP.Domain.Constants;
using ProdamSP.Domain.Models;
using SE1426.ProdamSP.Domain.Models.Cadastro;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ProdamSP.Domain.Models.Cadastro;
using ProdamSP.Domain.Models.Cadastro.Pessoa;
using System.IO;
using System.Reflection;

namespace ProdamSP.Business
{
    public class SolicitacaoMatriculaPreNatalBusiness : Business<SolicitacaoMatriculaPreNatal, int>, ISolicitacaoMatriculaPreNatalBusiness
    {
        private IUnitOfWork _uow;
        private ISolicitacaoMatriculaPreNatalRepository _solicitacaoMatriculaPreNatalRepository = null;
        private readonly IConfiguration _configuration;

        public SolicitacaoMatriculaPreNatalBusiness(ISolicitacaoMatriculaPreNatalRepository solicitacaoMatriculaPreNatalRepository,
            IUnitOfWork uow, IConfiguration configuration)
            : base(solicitacaoMatriculaPreNatalRepository)
        {
            _solicitacaoMatriculaPreNatalRepository = solicitacaoMatriculaPreNatalRepository;
            _uow = uow;
            _configuration = configuration;
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------
        //Consulta Pre-natal no Eol e no Siga 
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<DadosPreNatalEolSigaModelRetorno> PesquisarPreNatalEOLSIGA(string numeroCns, string numeroSisPreNatal)
        {
            try
            {
                if (!string.IsNullOrEmpty(numeroSisPreNatal)) {
                    //-->não considerar zeros à esquerda
                    Int64 numeroSisPreNatalInt = 0;
                    numeroSisPreNatalInt = Convert.ToInt64(numeroSisPreNatal);
                    numeroSisPreNatal = numeroSisPreNatalInt.ToString();
                    //-->
                }

                DadosPreNatalEolSigaModelRetorno objDadosPreNatalEolSigaModelRetorno = new DadosPreNatalEolSigaModelRetorno();

                //chamar metodo do EOL -------------------------------------------------------------------------------------------------------------------------
                SolicitacaoMatriculaPreNatalModelRetorno objsolicitacaoMatriculaPreNatalModelRetorno = await PesquisarPreNatalEOL(numeroCns, numeroSisPreNatal);

                if (objsolicitacaoMatriculaPreNatalModelRetorno == null) //erro - verificar *****
                {
                    objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                    objDadosPreNatalEolSigaModelRetorno.codRetorno = 1;
                    objDadosPreNatalEolSigaModelRetorno.msgRetorno = "Erro ao consultar solicitação -EOL";
                    objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = null;
                    objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = null;
                    return objDadosPreNatalEolSigaModelRetorno;
                }

                if (objsolicitacaoMatriculaPreNatalModelRetorno.codRetorno == 0)
                { //solicitacao EOL cadastrada
                    objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = objsolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal;
                    objDadosPreNatalEolSigaModelRetorno.codRetorno = 0;
                    objDadosPreNatalEolSigaModelRetorno.msgRetorno = null;
                    objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = null;
                    objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = null;
                }

                if (objsolicitacaoMatriculaPreNatalModelRetorno.codRetorno == 2) //solicitacao EOL desativada ou erro dos campos de entrada
                {
                    objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                    objDadosPreNatalEolSigaModelRetorno.codRetorno = 1;
                    objDadosPreNatalEolSigaModelRetorno.msgRetorno = objsolicitacaoMatriculaPreNatalModelRetorno.msgRetorno;
                    objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = null;
                    objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = null;
                }

                if (objsolicitacaoMatriculaPreNatalModelRetorno.codRetorno == 1) //solicitacao EOL nao cadastrada - pesquisar no SIGA
                {
                    //chamar metodo do SIGA -------------------------------------------------------------------------------------------------------------------------
                    PreNatalServiceBusiness preNatalService = new PreNatalServiceBusiness(_configuration);
                    DadosPreNatalSIGAModelRetorno objDadosPreNatalSIGAModelRetorno = await preNatalService.PesquisarPreNatal(numeroCns, numeroSisPreNatal);
                    if (objDadosPreNatalSIGAModelRetorno == null)
                    { //erro
                        objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                        objDadosPreNatalEolSigaModelRetorno.codRetorno = 1;
                        objDadosPreNatalEolSigaModelRetorno.msgRetorno = "Erro ao consultar solicitação-SIGA";
                        objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = null;
                        objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = null;
                        return objDadosPreNatalEolSigaModelRetorno;
                    }


                    //siga ok - codigo retorno = 1 e datas preenchidas
                    if (objDadosPreNatalSIGAModelRetorno.codRetorno == 1 &&
                        objDadosPreNatalSIGAModelRetorno.dataPrevisaoParto.HasValue &&
                        objDadosPreNatalSIGAModelRetorno.dataCadastroPreNatal.HasValue)
                    {
                        objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                        objDadosPreNatalEolSigaModelRetorno.codRetorno = 0;
                        objDadosPreNatalEolSigaModelRetorno.msgRetorno = null;
                        objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = objDadosPreNatalSIGAModelRetorno.dataPrevisaoParto;
                        objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = objDadosPreNatalSIGAModelRetorno.dataCadastroPreNatal;
                    }

                    //siga-erro - encontrado no SIGA , mas sem data de cadastro do pre-natal
                    if (objDadosPreNatalSIGAModelRetorno.codRetorno == 1 &&
                        objDadosPreNatalSIGAModelRetorno.dataPrevisaoParto.HasValue &&
                        !objDadosPreNatalSIGAModelRetorno.dataCadastroPreNatal.HasValue)
                    {
                        objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                        objDadosPreNatalEolSigaModelRetorno.codRetorno = 1;
                        objDadosPreNatalEolSigaModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_SIGA_PRE_NATAL_SEM_DATA_CADASTRO;
                        objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = objDadosPreNatalSIGAModelRetorno.dataPrevisaoParto;
                        objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = null;
                    }

                    //siga-erro - encontrado no SIGA , mas sem data de previsao do parto
                    if (objDadosPreNatalSIGAModelRetorno.codRetorno == 1 &&
                        !objDadosPreNatalSIGAModelRetorno.dataPrevisaoParto.HasValue &&
                        objDadosPreNatalSIGAModelRetorno.dataCadastroPreNatal.HasValue)
                    {
                        objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                        objDadosPreNatalEolSigaModelRetorno.codRetorno = 1;
                        objDadosPreNatalEolSigaModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_SIGA_PRE_NATAL_SEM_DATA_PREVISAO_PARTO;
                        objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = null;
                        objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = objDadosPreNatalSIGAModelRetorno.dataCadastroPreNatal;
                    }

                    //siga-erro - encontrado no SIGA , mas sem data de previsao do parto
                    if (objDadosPreNatalSIGAModelRetorno.codRetorno == 1 &&
                        !objDadosPreNatalSIGAModelRetorno.dataPrevisaoParto.HasValue &&
                        !objDadosPreNatalSIGAModelRetorno.dataCadastroPreNatal.HasValue)
                    {
                        objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                        objDadosPreNatalEolSigaModelRetorno.codRetorno = 1;
                        objDadosPreNatalEolSigaModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_SIGA_PRE_NATAL_SEM_DATA_CADASTRO_E_PREVISAO_PARTO;
                        objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = null; ;
                        objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = null;
                    }

                    /*
                    0.Código de pré-natal não localizado procure o posto de saúde mais próximo.
                    2.Código de pré-natal encontrado e vinculado corretamente a este CNS, porém com registro de interrupção do acompanhamento.
                    3.Código de pré-natal encontrado e está corretamente vinculado a este CNS, porém com registro data do parto
                    4.Código de pré-natal encontrado e está corretamente vinculado a este CNS, porém com data de nascimento prevista anterior à data atual.
                    5.Código de pré-natal encontrado, mas está vinculado a outro número de CNS
                    */

                    if (objDadosPreNatalSIGAModelRetorno.codRetorno != 1)
                    {
                        objDadosPreNatalEolSigaModelRetorno.codEOLPreNatal = null;
                        objDadosPreNatalEolSigaModelRetorno.codRetorno = 1;
                        objDadosPreNatalEolSigaModelRetorno.msgRetorno = objDadosPreNatalSIGAModelRetorno.msgRetorno;
                        objDadosPreNatalEolSigaModelRetorno.dataPrevisaoParto = null;
                        objDadosPreNatalEolSigaModelRetorno.dataCadastroPreNatal = null;
                    }
                }

                return objDadosPreNatalEolSigaModelRetorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------
        //--> Consulta no EOL 
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<SolicitacaoMatriculaPreNatalModelRetorno> PesquisarPreNatalEOL(string numeroCns, string numeroSisPreNatal)
        {
            if (!string.IsNullOrEmpty(numeroSisPreNatal))
            {
                //-->não considerar zeros à esquerda
                Int64 numeroSisPreNatalInt = 0;
                numeroSisPreNatalInt = Convert.ToInt64(numeroSisPreNatal);
                numeroSisPreNatal = numeroSisPreNatalInt.ToString();
                //-->
            }

            SolicitacaoMatriculaPreNatalModelRetorno objSolicitacaoMatriculaPreNatalModelRetorno = new SolicitacaoMatriculaPreNatalModelRetorno();

            try
            {
                //Consistências
                if (string.IsNullOrEmpty(numeroCns) && string.IsNullOrEmpty(numeroSisPreNatal))
                {
                    objSolicitacaoMatriculaPreNatalModelRetorno.codRetorno = 2;
                    objSolicitacaoMatriculaPreNatalModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_FILTRO_NAO_INFORMADO;
                    objSolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal = null;
                    return objSolicitacaoMatriculaPreNatalModelRetorno;
                }

                if (string.IsNullOrEmpty(numeroCns))
                {
                    objSolicitacaoMatriculaPreNatalModelRetorno.codRetorno = 2;
                    objSolicitacaoMatriculaPreNatalModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_NUMERO_CNS_OBRIGATORIO;
                    objSolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal = null;
                    return objSolicitacaoMatriculaPreNatalModelRetorno;
                }

                if (string.IsNullOrEmpty(numeroSisPreNatal))
                {
                    objSolicitacaoMatriculaPreNatalModelRetorno.codRetorno = 2;
                    objSolicitacaoMatriculaPreNatalModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_NUMERO_PRE_NATAL_OBRIGATORIO;
                    objSolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal = null;
                    return objSolicitacaoMatriculaPreNatalModelRetorno;
                }

                //validacoes do banco
                // var objSolicitacao = _solicitacaoMatriculaPreNatalRepository.Find(x => x.NrCnsResponsavel.Trim() == numeroCns.Trim() && x.NrPreNatal.Trim() == numeroSisPreNatal.Trim()).FirstOrDefault();

                var objSolicitacao = _solicitacaoMatriculaPreNatalRepository.ConsultarSolicitacao(numeroCns, numeroSisPreNatal);

                PopularSolicitacaoMatriculaPreNatalModelRetorno(objSolicitacao, ref objSolicitacaoMatriculaPreNatalModelRetorno);

                if (objSolicitacao.Result == null)
                {
                    objSolicitacaoMatriculaPreNatalModelRetorno.codRetorno = 1; //solicitacao nao cadastrada
                    objSolicitacaoMatriculaPreNatalModelRetorno.msgRetorno = null;// Mensagens.MS_PUB_CPT_005_SOLICITACAO_MATRICULA_PRE_NATAL_NAO_ENCONTRADO;
                    objSolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal = null;
                }
                else
                {
                    if (objSolicitacao.Result.DtCancelamentoInscricaoEol != null)
                    {
                        switch (objSolicitacao.Result.CdMotivoCancelamentoInscricao)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                objSolicitacaoMatriculaPreNatalModelRetorno.codRetorno = 2; //solicitacao desativada
                                objSolicitacaoMatriculaPreNatalModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_SOLICITACAO_MATRICULA_PRE_NATAL_DESATIVADO_SIGA;
                                objSolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal = null;
                                break;
                            case 16:
                                objSolicitacaoMatriculaPreNatalModelRetorno.codRetorno = 2; //solicitacao desativada
                                objSolicitacaoMatriculaPreNatalModelRetorno.msgRetorno = Mensagens.MS_PUB_CPT_005_SOLICITACAO_MATRICULA_PRE_NATAL_DESATIVADO_EOL;
                                objSolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal = null;
                                break;
                        }
                    }
                    else
                    {
                        objSolicitacaoMatriculaPreNatalModelRetorno.codRetorno = 0; //solicitacao cadastrada
                        objSolicitacaoMatriculaPreNatalModelRetorno.msgRetorno = null;
                        objSolicitacaoMatriculaPreNatalModelRetorno.codEOLPreNatal = objSolicitacao.Result.CdSolicitacaoMatriculaPreNatal;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objSolicitacaoMatriculaPreNatalModelRetorno;
        }



        private void PopularSolicitacaoMatriculaPreNatalModelRetorno(Task<SolicitacaoMatriculaPreNatal> objSolicitacao, ref SolicitacaoMatriculaPreNatalModelRetorno objSolicitacaoMatriculaPreNatalModelRetorno)
        {

            if (objSolicitacao.Result != null)
            {
                objSolicitacaoMatriculaPreNatalModelRetorno.cdSolicitacaoMatriculaPreNatal = objSolicitacao.Result.CdSolicitacaoMatriculaPreNatal;
                objSolicitacaoMatriculaPreNatalModelRetorno.nrCnsResponsavel = objSolicitacao.Result.NrCnsResponsavel;
                objSolicitacaoMatriculaPreNatalModelRetorno.nrPreNatal = objSolicitacao.Result.NrPreNatal;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtInicioPreNatal = objSolicitacao.Result.DtInicioPreNatal;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtInscricaoEol = objSolicitacao.Result.DtInscricaoEol;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtExclusaoInscricaoEol = objSolicitacao.Result.DtExclusaoInscricaoEol;
                objSolicitacaoMatriculaPreNatalModelRetorno.cdOrigemLocalAlteracao = objSolicitacao.Result.CdOrigemLocalAlteracao;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtNascimentoPrevista = objSolicitacao.Result.DtNascimentoPrevista;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtIntencaoMatricula = objSolicitacao.Result.DtIntencaoMatricula;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtUltimaConsultaGestante = objSolicitacao.Result.DtUltimaConsultaGestante;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtCancelamentoInscricaoEol = objSolicitacao.Result.DtCancelamentoInscricaoEol;
                objSolicitacaoMatriculaPreNatalModelRetorno.cdMotivoCancelamentoInscricao = objSolicitacao.Result.CdMotivoCancelamentoInscricao;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtTransformacaoCandidato = objSolicitacao.Result.DtTransformacaoCandidato;
                objSolicitacaoMatriculaPreNatalModelRetorno.cdSolicitacaoMatricula = objSolicitacao.Result.CdSolicitacaoMatricula;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtSolicitacaoMatricula = objSolicitacao.Result.DtSolicitacaoMatricula;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtEncaminhamentoMatricula = objSolicitacao.Result.DtEncaminhamentoMatricula;
                objSolicitacaoMatriculaPreNatalModelRetorno.nrCnsCrianca = objSolicitacao.Result.NrCnsCrianca;
                objSolicitacaoMatriculaPreNatalModelRetorno.dtAtualizacaoTabela = objSolicitacao.Result.DtAtualizacaoTabela;
                objSolicitacaoMatriculaPreNatalModelRetorno.cdOperador = objSolicitacao.Result.CdOperador;              
            }
        }

        public async Task<List<PreNatalModel>> PesquisarPreNataisEOL(string numeroCns, string numeroSisPreNatal, int codSolicMatricula = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(numeroSisPreNatal))
                {
                    //-->não considerar zeros à esquerda
                    Int64 numeroSisPreNatalInt = 0;
                    numeroSisPreNatalInt = Convert.ToInt64(numeroSisPreNatal);
                    numeroSisPreNatal = numeroSisPreNatalInt.ToString();
                    //-->
                }

                List<SolicitacaoMatriculaPreNatal> solicitacoes = await _solicitacaoMatriculaPreNatalRepository.ConsultarSolicitacoes(numeroCns, numeroSisPreNatal,  codSolicMatricula );

                List<PreNatalModel> preNatais = solicitacoes.ConvertAll(new Converter<SolicitacaoMatriculaPreNatal, PreNatalModel>(SolicitacaoMatriculaPreNatalToPreNatalModel));

                return preNatais;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private PreNatalModel SolicitacaoMatriculaPreNatalToPreNatalModel(SolicitacaoMatriculaPreNatal solicitacao)
        {
            PreNatalModel preNatalModel = new PreNatalModel();


            SolicitacaoMatriculaPreNatalModel matriculaPreNatal = new SolicitacaoMatriculaPreNatalModel();
            matriculaPreNatal.CdSolicitacaoMatriculaPreNatal = solicitacao.CdSolicitacaoMatriculaPreNatal;
            matriculaPreNatal.NrCnsResponsavel = solicitacao.NrCnsResponsavel;
            matriculaPreNatal.NrPreNatal = solicitacao.NrPreNatal;
            matriculaPreNatal.DtInicioPreNatal = solicitacao.DtInicioPreNatal;
            matriculaPreNatal.DtInscricaoEol = solicitacao.DtInscricaoEol;
            matriculaPreNatal.DtExclusaoInscricaoEol = solicitacao.DtExclusaoInscricaoEol;
            matriculaPreNatal.CdOrigemLocalAlteracao = solicitacao.CdOrigemLocalAlteracao;
            matriculaPreNatal.DtNascimentoPrevista = solicitacao.DtNascimentoPrevista;
            matriculaPreNatal.DtIntencaoMatricula = solicitacao.DtIntencaoMatricula;
            matriculaPreNatal.DtUltimaConsultaGestante = solicitacao.DtUltimaConsultaGestante;
            matriculaPreNatal.DtCancelamentoInscricaoEol = solicitacao.DtCancelamentoInscricaoEol;
            matriculaPreNatal.CdMotivoCancelamentoInscricao = solicitacao.CdMotivoCancelamentoInscricao;
            matriculaPreNatal.DtTansformacaoCandidato = solicitacao.DtTransformacaoCandidato;
            matriculaPreNatal.CdSolicitacaoMatricula = solicitacao.CdSolicitacaoMatricula;
            matriculaPreNatal.DtSolicitacaoMatricula = solicitacao.DtSolicitacaoMatricula;
            matriculaPreNatal.DtEncaminhamentoMatricula = solicitacao.DtEncaminhamentoMatricula;
            matriculaPreNatal.NrCnsCrianca = solicitacao.NrCnsCrianca;
            matriculaPreNatal.DtAtualizacaoTabela = solicitacao.DtAtualizacaoTabela;
            matriculaPreNatal.CdOperador = solicitacao.CdOperador;

            if (solicitacao.DtIntencaoMatricula != null)
            {
                var qtdDiasLimite = getDiasLimite().Result;

                if (!String.IsNullOrEmpty(qtdDiasLimite))
                {
                    matriculaPreNatal.DtLimiteInformarNascimento = solicitacao.DtNascimentoPrevista.AddDays(Int32.Parse(qtdDiasLimite) - 1);
                }
                
            }

            preNatalModel.MatriculaPreNatal = matriculaPreNatal;

            preNatalModel.Responsavel = convertResponsavelGeralToResponsavelGeralModel(solicitacao.NrCnsResponsavelNavigation.CdResponsavelGeralNavigation);

            preNatalModel.Aluno = convertAlunoToAlunoModel(solicitacao.CdSolicitacaoMatriculaNavigation.CdAlunoNavigation);

            preNatalModel.Matricula = convertMatriculaToMatriculaModel(solicitacao.CdSolicitacaoMatriculaNavigation);            

            return preNatalModel;
            //return null;
        }

        private async Task<String> getDiasLimite()
        {
            return await _solicitacaoMatriculaPreNatalRepository.ConsultarParametroGeral("nm_parametro_geral = 'qtd_dias_apos_nascimento'");
        }
        private ResponsavelGeralModel convertResponsavelGeralToResponsavelGeralModel(ResponsavelGeral dados)
        {
            if (dados == null)
            {
                return null;
            }

            ResponsavelGeralModel model = new ResponsavelGeralModel();

            model.CdResponsavelGeral = dados.CdResponsavelGeral;
            //model.CdTipoPessoaResponsavel = dados.CdTipoPessoaResponsavel;

            if (dados.CdCpfResponsavel.HasValue)
                model.CdCpfResponsavel = dados.CdCpfResponsavel.Value;

            model.InCpfResponsavelConferido = dados.InCpfResponsavelConferido;
            model.NmResponsavel = dados.NmResponsavel;
            model.NrRgResponsavel = dados.NrRgResponsavel;
            model.CdDigitoRgResponsavel = dados.CdDigitoRgResponsavel;
            model.SgUfRgResponsavel = dados.SgUfRgResponsavel;
            model.NmMaeResponsavel = dados.NmMaeResponsavel;

            if (dados.DtNascimentoMaeResponsavel.HasValue)
                model.DtNascimentoMaeResponsavel = dados.DtNascimentoMaeResponsavel.Value;

            if (dados.CdTipoDocumentoEstrangeiro.HasValue)
                model.CdTipoDocumentoEstrangeiro = dados.CdTipoDocumentoEstrangeiro.Value;

            model.NrDocumentoEstrangeiro = dados.NrDocumentoEstrangeiro;

            //model.DtInicio = dados.DtInicio;

            //if (dados.DtFim.HasValue)
            //    model.DtFim = dados.DtFim.Value;

            Endereco endereco = dados.CdEnderecoNavigation;

            model.CdEndereco = endereco.CiEndereco;
            model.TpLogradouro = endereco.TpLogradouro;
            model.NmLogradouro = endereco.NmLogradouro;
            model.CdNrEndereco = endereco.CdNrEndereco;
            model.DcComplementoEndereco = endereco.DcComplementoEndereco;
            model.NmBairro = endereco.NmBairro;
            model.CdCep = endereco.CdCep;
            model.TpLocalizacaoEndereco = endereco.TpLocalizacaoEndereco;
            model.CdMunicipio = endereco.CdMunicipio;
            model.DcTpLogradouro = endereco.TpLogradouroNavigation.DcTpLogradouro;

            if (dados.DispositivoComunicacaoResponsavel.Count > 0) { 
                DispositivoComunicacaoResponsavel dispositivoComunicacao = dados.DispositivoComunicacaoResponsavel.ToList<DispositivoComunicacaoResponsavel>()[0];

                model.CdDispositivoComunicacaoResponsavel = dispositivoComunicacao.CdDispositivoComunicacaoResponsavel;
                model.CdDddCelularResponsavel = dispositivoComunicacao.CdDddCelularResponsavel;
                model.NrCelularResponsavel = dispositivoComunicacao.NrCelularResponsavel;
                model.CdTipoTurnoCelular = dispositivoComunicacao.CdTipoTurnoCelular;
                model.CdDddTelefoneFixoResponsavel = dispositivoComunicacao.CdDddTelefoneFixoResponsavel;
                model.NrTelefoneFixoResponsavel = dispositivoComunicacao.NrTelefoneFixoResponsavel;
                model.CdTipoTurnoFixo = dispositivoComunicacao.CdTipoTurnoFixo;
                model.CdDddTelefoneComercialResponsavel = dispositivoComunicacao.CdDddTelefoneComercialResponsavel;
                model.NrTelefoneComercialResponsavel = dispositivoComunicacao.NrTelefoneComercialResponsavel;
                model.NrRamalTelefoneComercialResponsavel = dispositivoComunicacao.NrRamalTelefoneComercialResponsavel;
                model.CdTipoTurnoComercial = dispositivoComunicacao.CdTipoTurnoComercial;
                model.InAutorizaEnvioSms = dispositivoComunicacao.InAutorizaEnvioSms;
                model.NmEmailResponsavel = dispositivoComunicacao.NmEmailResponsavel;
            }

            return model;
        }

        private AlunoModel convertAlunoToAlunoModel(Aluno aluno)
        {
            if (aluno == null)
            {
                return null;
            }

            AlunoModel model = new AlunoModel();

            model.CdAluno = aluno.CdAluno;
            model.NmAluno = aluno.NmAluno;
            model.DtNascimentoAluno = aluno.DtNascimentoAluno;
            model.CdSexoAluno = aluno.CdSexoAluno;
            model.CdSexoMae = aluno.CdSexoMae;
            model.NmMaeAluno = aluno.NmMaeAluno;
            model.TpRacaCor = aluno.TpRacaCor;
            model.CdOrgaoEmissor = aluno.CdOrgaoEmissor;
            model.CdPaisMec = aluno.CdPaisMec;
            model.CdPaisOrigemMae = aluno.CdPaisOrigemMae.ToString();
            model.SgUfRgAluno = aluno.SgUfRgAluno;
            model.CdCpfAluno = aluno.CdCpfAluno;
            model.NrRgAluno = aluno.NrRgAluno;
            model.CdDigitoRgAluno = aluno.CdDigitoRgAluno;
            model.DtEmissaoRg = aluno.DtEmissaoRg;
            model.CdNacionalidadeMae = aluno.CdNacionalidadeMae;


            //Endereco endereco = aluno.CdEnderecoNavigation;

            //model.CdEndereco = endereco.CiEndereco;
            //model.TpLogradouro = endereco.TpLogradouro;
            //model.NmLogradouro = endereco.NmLogradouro;
            //model.CdNrEndereco = endereco.CdNrEndereco;
            //model.DcComplementoEndereco = endereco.DcComplementoEndereco;
            //model.NmBairro = endereco.NmBairro;
            //model.CdCep = endereco.CdCep;
            //model.TpLocalizacaoEndereco = endereco.TpLocalizacaoEndereco;
            //model.CdMunicipio = endereco.CdMunicipio;

            //DispositivoComunicacaoAluno dispositivoComunicacao = new DispositivoComunicacaoAluno();

            //model.CdDispositivoComunicacaoAluno = dispositivoComunicacao.CdDispositivoComunicacaoAluno;
            //model.CdDddCelularAluno = dispositivoComunicacao.CdDddCelularAluno;
            //model.NrCelularAluno = dispositivoComunicacao.NrCelularAluno;
            //model.CdTipoTurnoCelular = dispositivoComunicacao.CdTipoTurnoCelular;
            //model.CdDddTelefoneFixoAluno = dispositivoComunicacao.CdDddTelefoneFixoAluno;
            //model.NrTelefoneFixoAluno = dispositivoComunicacao.NrTelefoneFixoAluno;
            //model.CdTipoTurnoFixo = dispositivoComunicacao.CdTipoTurnoFixo;
            //model.CdDddTelefoneComercialAluno = dispositivoComunicacao.CdDddTelefoneComercialAluno;
            //model.NrTelefoneComercialAluno = dispositivoComunicacao.NrTelefoneComercialAluno;
            //model.NrRamalTelefoneComercialAluno = dispositivoComunicacao.NrRamalTelefoneComercialAluno;
            //model.CdTipoTurnoComercial = dispositivoComunicacao.CdTipoTurnoComercial;
            //model.InAutorizaEnvioSms = dispositivoComunicacao.InAutorizaEnvioSms;
            //model.NmEmailAluno = dispositivoComunicacao.NmEmailAluno;

            return model;

        }

        private SolicitacaoMatriculaModel convertMatriculaToMatriculaModel(SolicitacaoMatricula solicitacaoMatricula)
        {
            if (solicitacaoMatricula == null)
            {
                return null;
            }

            SolicitacaoMatriculaModel model = new SolicitacaoMatriculaModel();
            model.CdUnidadeEducacao = solicitacaoMatricula.CdUnidadeEducacao;
            model.TpOrigem = solicitacaoMatricula.TpOrigem;
            model.StSolicitacao = solicitacaoMatricula.StSolicitacao;
            model.DtStatusSolicitacao = solicitacaoMatricula.DtStatusSolicitacao;
            model.CdMicroRegiao = solicitacaoMatricula.CdMicroRegiao;
            model.CdModalidadeEnsino = solicitacaoMatricula.CdModalidadeEnsino;
            model.CdEtapaEnsino = solicitacaoMatricula.CdEtapaEnsino;
            model.CdCicloEnsino = solicitacaoMatricula.CdCicloEnsino;
            model.CdSerieEnsino = solicitacaoMatricula.CdSerieEnsino;
            model.CdTipoMotivoRecusaMatricula = solicitacaoMatricula.CdTipoMotivoRecusaMatricula;
            model.DcObservacaoStatus = solicitacaoMatricula.DcObservacaoStatus;
            model.DtProtocoloDesistenciaMatricula = solicitacaoMatricula.DtProtocoloDesistenciaMatricula;
            model.AnLetivo = solicitacaoMatricula.AnLetivo;            

            return model;
        }

        public async Task<PreNatalRetornoInclusaoModel> InserirMaePaulistanaPreNatal(PreNatalModel preNatalModel)
        {
            try
            {

                FoneticaBusiness fonetica = new FoneticaBusiness();
                preNatalModel.Aluno.NmAlunoFonetico = String.IsNullOrEmpty(preNatalModel.Aluno.NmAluno) ? "" : fonetica.Fonetizar(preNatalModel.Aluno.NmAluno);
                preNatalModel.Aluno.NmMaeAlunoFonetico = String.IsNullOrEmpty(preNatalModel.Aluno.NmMaeAluno) ? "" : fonetica.Fonetizar(preNatalModel.Aluno.NmMaeAluno);

                //PreNatalRetornoInclusaoModel retornoInclusao = new PreNatalRetornoInclusaoModel();
                //retornoInclusao.Mensagem = "Teste";
                //retornoInclusao.Retorno = "TRUE";
                
                var retornoInclusao = await _solicitacaoMatriculaPreNatalRepository.InserirMaePaulistanaPreNatal(preNatalModel);
                var baseUri = string.IsNullOrEmpty(preNatalModel.Base)?"": preNatalModel.Base;

                if (retornoInclusao.Retorno == "TRUE") {
                    try
                    {
                        await EnviarMensagemInscricaoMaePaulistana(preNatalModel.MatriculaPreNatal.NrCnsResponsavel, preNatalModel.MatriculaPreNatal.NrPreNatal, baseUri);

                    }
                    catch (Exception e)
                    {
                        retornoInclusao.Mensagem = retornoInclusao.Mensagem + " Ocorreu um erro no envio do email. Erro: " + e.Message;
                    }
                }
                
                return retornoInclusao;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PreNatalRetornoExclusaoModel> ExcluirMaePaulistanaPreNatal(PreNatalExclusaoModel preNatalExclusaoModel)
        {
            try
            {
                var retornoExclusao = await _solicitacaoMatriculaPreNatalRepository.ExcluirMaePaulistanaPreNatal(preNatalExclusaoModel);

                var baseUri = string.IsNullOrEmpty(preNatalExclusaoModel.Base) ? "" : preNatalExclusaoModel.Base;
                if (retornoExclusao.Retorno == "TRUE")
                {
                    try
                    {
                        await EnviarMensagemInscricaoMaePaulistana(preNatalExclusaoModel.NrCnsResponsavel, preNatalExclusaoModel.NrPreNatal, baseUri);

                    }
                    catch (Exception e)
                    {
                        retornoExclusao.Mensagem = retornoExclusao.Mensagem + " Ocorreu um erro no envio do email." + e.Message;
                    }
                }

                return retornoExclusao;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<StatusPreNatalRetornoModel> AtualizarStatusPreNatal(StatusPreNatalModel statusPreNatalModel)
        {
            try
            {
                var statusPreNatalRetornoModel = new StatusPreNatalRetornoModel();

                if (statusPreNatalModel == null) {
                    statusPreNatalRetornoModel.Retorno = "FALSE";
                    statusPreNatalRetornoModel.Mensagem  = "Não foram informados dados para atualização do status do Pré-Natal.";
                    return statusPreNatalRetornoModel;
                }

                statusPreNatalRetornoModel = await _solicitacaoMatriculaPreNatalRepository.AtualizarStatusMaePaulistanaPreNatal(statusPreNatalModel);

                return statusPreNatalRetornoModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
        public async Task<DadosCadastraisPreNatalAtualizacaoRetornoModel> AtualizarDadosCadastraisPreNatal(DadosCadastraisPreNatalAtualizacaoModel dadosCadastraisPreNatalAtualizacaoModel)
        {
            try
            {
                FoneticaBusiness fonetica = new FoneticaBusiness();
                dadosCadastraisPreNatalAtualizacaoModel.FoneticaNmMaeCrianca = String.IsNullOrEmpty(dadosCadastraisPreNatalAtualizacaoModel.NmMaeCrianca) ? "" : fonetica.Fonetizar(dadosCadastraisPreNatalAtualizacaoModel.NmMaeCrianca);

                var retorno= await _solicitacaoMatriculaPreNatalRepository.AtualizarDadosCadastraisPreNatal(dadosCadastraisPreNatalAtualizacaoModel);
 
                return retorno;
            }
            catch (Exception e)
            {
                throw e;
            }
        
    }

        public async Task<List<PreNatalTransformacaoModel>> PesquisarPreNataisTransformacaoEOL(string numeroCns, string numeroSisPreNatal)
        {
            try
            {
                List<PreNatalModel> preNatais = await PesquisarPreNataisEOL(numeroCns, numeroSisPreNatal);
                List<PreNatalTransformacaoModel> preNataisTransformacao = PopularPreNatalTransformacaoModel(preNatais);

                return preNataisTransformacao;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        private List<PreNatalTransformacaoModel> PopularPreNatalTransformacaoModel(List<PreNatalModel> listaPreNatal) {
            
            List<PreNatalTransformacaoModel> listaPreNatalTransformacao = new List<PreNatalTransformacaoModel>();

            if (listaPreNatal == null) {
                return listaPreNatalTransformacao;
            }

            
            var nrCrianca = 0;
            foreach (var item in listaPreNatal)
            {
                //adiciona dados da crianca/transformacao
                PreNatalTransformacaoModel preNatalTransformacaoModel = new PreNatalTransformacaoModel();

                nrCrianca = nrCrianca + 1;

                TransformacaoModel transformacaoModel = new TransformacaoModel();
                transformacaoModel.NrCrianca = nrCrianca;
                transformacaoModel.CdSolicitacaoMatriculaPreNatal = item.MatriculaPreNatal.CdSolicitacaoMatriculaPreNatal;
                transformacaoModel.CdSolicitacaoMatricula = item.MatriculaPreNatal.CdSolicitacaoMatricula;
                transformacaoModel.NrCnsCrianca = item.MatriculaPreNatal.NrCnsCrianca;

                if (!string.IsNullOrEmpty(item.MatriculaPreNatal.NrCnsCrianca))
                {
                    transformacaoModel.NrCnsCriancaFormatado = Convert.ToUInt64(item.MatriculaPreNatal.NrCnsCrianca).ToString(@"000 0000 0000 0000");
                }
                else
                {
                    transformacaoModel.NrCnsCriancaFormatado = "";
                }

                transformacaoModel.DtTransformacaoCandidato = item.MatriculaPreNatal.DtTansformacaoCandidato;

                transformacaoModel.InCnsCriancaPreenchida = false;
                if (!string.IsNullOrEmpty(item.MatriculaPreNatal.NrCnsCrianca) && transformacaoModel.DtTransformacaoCandidato != null)
                {
                    transformacaoModel.InCnsCriancaPreenchida = true;
                }

                var NmResponsavelEol = item.Responsavel.NmResponsavel.Trim();

                transformacaoModel.NrCnsResponsavel = item.MatriculaPreNatal.NrCnsResponsavel.Trim();
                transformacaoModel.NrPreNatal = item.MatriculaPreNatal.NrPreNatal.Trim();
                transformacaoModel.DtInscricaoEol = item.MatriculaPreNatal.DtInscricaoEol;

                //popular dados da mãe
                DadosMaeModel dadosMae = new DadosMaeModel();

                dadosMae.NmMaeCrianca = string.IsNullOrEmpty(item.Responsavel.NmResponsavel) ? "" : item.Responsavel.NmResponsavel;

                if (item.Responsavel.DtNascimentoMaeResponsavel.HasValue)
                    dadosMae.DtNascimentoMaeCrianca = item.Responsavel.DtNascimentoMaeResponsavel.Value;

                dadosMae.CdPaisOrigemMae = string.IsNullOrEmpty(item.Responsavel.CdPaisOrigemMae) ? "" : item.Responsavel.CdPaisOrigemMae;
                dadosMae.CdCpfResponsavel = item.Responsavel.CdCpfResponsavel;
                dadosMae.NrRgResponsavel = string.IsNullOrEmpty(item.Responsavel.NrRgResponsavel) ? "" : item.Responsavel.NrRgResponsavel; //ver se vai precisar do CdDigitoRgResponsavel
                dadosMae.SgUfRgResponsavel = string.IsNullOrEmpty(item.Responsavel.SgUfRgResponsavel) ? "" : item.Responsavel.SgUfRgResponsavel;
                dadosMae.NrDocumentoEstrangeiro = string.IsNullOrEmpty(item.Responsavel.SgUfRgResponsavel) ? "" : item.Responsavel.SgUfRgResponsavel;

                dadosMae.DcTpLogradouro = string.IsNullOrEmpty(item.Responsavel.DcTpLogradouro) ? "" : item.Responsavel.DcTpLogradouro;
                dadosMae.NmLogradouro = string.IsNullOrEmpty(item.Responsavel.NmLogradouro) ? "" : item.Responsavel.NmLogradouro;
                dadosMae.CdNrEndereco = string.IsNullOrEmpty(item.Responsavel.CdNrEndereco) ? "" : item.Responsavel.CdNrEndereco;
                dadosMae.DcComplementoEndereco = string.IsNullOrEmpty(item.Responsavel.DcComplementoEndereco) ? "" : item.Responsavel.DcComplementoEndereco;
                dadosMae.NmBairro = string.IsNullOrEmpty(item.Responsavel.NmBairro) ? "" : item.Responsavel.NmBairro;
                dadosMae.CdCep = item.Responsavel.CdCep;
                dadosMae.NmMunicipio = string.IsNullOrEmpty(item.Responsavel.NmMunicipio) ? "" : item.Responsavel.NmMunicipio;

                dadosMae.CdDddCelularMae = string.IsNullOrEmpty(item.Responsavel.CdDddCelularResponsavel) ? "" : item.Responsavel.CdDddCelularResponsavel;
                dadosMae.DcDispositivoCelularMae = string.IsNullOrEmpty(item.Responsavel.NrCelularResponsavel) ? "" : item.Responsavel.NrCelularResponsavel;

                dadosMae.CdDddComercialMae = string.IsNullOrEmpty(item.Responsavel.CdDddTelefoneComercialResponsavel) ? "" : item.Responsavel.CdDddTelefoneComercialResponsavel;
                dadosMae.DcDispositivoComercialMae = string.IsNullOrEmpty(item.Responsavel.NrTelefoneComercialResponsavel) ? "" : item.Responsavel.NrTelefoneComercialResponsavel;

                dadosMae.CdDddResidencialMae = string.IsNullOrEmpty(item.Responsavel.CdDddTelefoneFixoResponsavel) ? "" : item.Responsavel.CdDddTelefoneFixoResponsavel;
                dadosMae.DcDispositivoResidencialMae = string.IsNullOrEmpty(item.Responsavel.NrTelefoneFixoResponsavel) ? "" : item.Responsavel.NrTelefoneFixoResponsavel;

                dadosMae.EmailResponsavel = string.IsNullOrEmpty(item.Responsavel.NmEmailResponsavel) ? "" : item.Responsavel.NmEmailResponsavel;
                dadosMae.FoneticaNmMaeCrianca = "";

                transformacaoModel.DadosMae = dadosMae;


                var dadosCrianca = PreencherDadosCrianca(item);
                transformacaoModel.DadosCrianca = dadosCrianca;

                //adicionar na model preNatalTransformacao
                preNatalTransformacaoModel.Aluno = item.Aluno;
                preNatalTransformacaoModel.Responsavel = item.Responsavel;
                preNatalTransformacaoModel.Matricula = item.Matricula;
                preNatalTransformacaoModel.MatriculaPreNatal = item.MatriculaPreNatal;
                preNatalTransformacaoModel.Transformacao = transformacaoModel;

                listaPreNatalTransformacao.Add(preNatalTransformacaoModel);
            }

            return listaPreNatalTransformacao;
        }

        private DadosCriancaModel PreencherDadosCrianca(PreNatalModel preNatal)
        {
            if (preNatal == null)
            {
                return null;
            }
         
            var aluno = preNatal.Aluno;
            if (aluno == null)
            {
                return null;
            }

            DadosCriancaModel dadosCrianca = new DadosCriancaModel();
            dadosCrianca.NrCnsCrianca = string.IsNullOrEmpty(preNatal.MatriculaPreNatal.NrCnsCrianca) ? "" : preNatal.MatriculaPreNatal.NrCnsCrianca;
            dadosCrianca.NmCrianca = string.IsNullOrEmpty(aluno.NmAluno) ? "" : aluno.NmAluno;
            dadosCrianca.NmMae = string.IsNullOrEmpty(aluno.NmMaeAluno) ? "" : aluno.NmMaeAluno;
            //dadosCrianca.NmPai = "";

            dadosCrianca.DtNascimentoCrianca = aluno.DtNascimentoAluno;
            dadosCrianca.SexoCrianca = string.IsNullOrEmpty(aluno.CdSexoAluno) ? "" : aluno.CdSexoAluno;
            dadosCrianca.TpRacaCorCrianca = aluno.TpRacaCor;
            //dadosCrianca.CdPaisOrigemCrianca = "";

            try
            {
                dadosCrianca.CdCpfCrianca = Convert.ToDecimal(aluno.CdCpfAluno);
            }
            catch (Exception)
            {
                dadosCrianca.CdCpfCrianca = null;
            }

            dadosCrianca.NrRgCrianca = string.IsNullOrEmpty(aluno.NrRgAluno) ? "" : aluno.NrRgAluno;
            dadosCrianca.SgUfRgCrianca = string.IsNullOrEmpty(aluno.SgUfRgAluno) ? "" : aluno.SgUfRgAluno;
            dadosCrianca.NrDocumentoEstrangeiroCrianca = string.IsNullOrEmpty(aluno.NrDocumentoEstrangeiro) ? "" : aluno.NrDocumentoEstrangeiro;


                //Endereco
                dadosCrianca.DcTpLogradouroCrianca = string.IsNullOrEmpty(aluno.SgTituloLogradouro) ? "" : aluno.SgTituloLogradouro;
                dadosCrianca.NmLogradouroCrianca = string.IsNullOrEmpty(aluno.NmLogradouro) ? "" : aluno.NmLogradouro;
                dadosCrianca.CdNrEnderecoCrianca = string.IsNullOrEmpty(aluno.CdNrEndereco) ? "" : aluno.CdNrEndereco;
                dadosCrianca.DcComplementoEnderecoCrianca = string.IsNullOrEmpty(aluno.DcComplementoEndereco ) ? "" : aluno.DcComplementoEndereco;
                dadosCrianca.NmBairroCrianca = string.IsNullOrEmpty(aluno.NmBairro) ? "" : aluno.NmBairro;
                try
                {
                    dadosCrianca.CdCepCrianca = aluno.CdCep;
                }
                catch (Exception)
                {
                    dadosCrianca.CdCepCrianca = 0;
                }

                //dadosCrianca.NmMunicipioCrianca = "";

                //Contato
                //dadosCrianca.CdDddCelularCrianca = string.IsNullOrEmpty(aluno) ? "" : aluno.Contato.DddCelular;
                //dadosCrianca.DcDispositivoCelularCrianca = string.IsNullOrEmpty(aluno.TelefoneCelular) ? "" : aluno.Contato.TelefoneCelular;
                //dadosCrianca.CdDddComercialCrianca = string.IsNullOrEmpty(aluno.DddComercial) ? "" : aluno.Contato.DddComercial;
                //dadosCrianca.DcDispositivoComercialCrianca = string.IsNullOrEmpty(aluno.TelefoneComercial) ? "" : aluno.Contato.TelefoneComercial;
                //dadosCrianca.CdDddResidencialCrianca = string.IsNullOrEmpty(aluno.DddResidencial) ? "" : aluno.Contato.DddResidencial;
                //dadosCrianca.DcDispositivoResidencialCrianca = string.IsNullOrEmpty(aluno.TelefoneResidencial) ? "" : aluno.Contato.TelefoneResidencial;
                //dadosCrianca.EmailCrianca = string.IsNullOrEmpty(aluno.Email) ? "" : aluno.Email;

            dadosCrianca.FoneticaNmCrianca = aluno.NmAlunoFonetico;

            return dadosCrianca;

        }


        //--------------------enviar email ----------------------------------//

        /// <summary>
        /// </summary>
        public async Task EnviarMensagemInscricaoMaePaulistana(string numeroCns, string numeroSisPreNatal, string baseUri)
        {
            
            try
            {
                string solicitacoesMatriculas = "";
                List<SolicitacaoMatriculaPreNatal> preNatais = await _solicitacaoMatriculaPreNatalRepository.ConsultarSolicitacoes(numeroCns, numeroSisPreNatal, 0);
                

                if (preNatais != null && preNatais.Count() > 0)
                {
                    foreach (var item in preNatais)
                    {
                        solicitacoesMatriculas = solicitacoesMatriculas + item.CdSolicitacaoMatricula + ", ";
                    }
                }
                if (solicitacoesMatriculas != "") { solicitacoesMatriculas = solicitacoesMatriculas.Substring(0, solicitacoesMatriculas.Length - 2); }

                string qtd_dias = await _solicitacaoMatriculaPreNatalRepository.ConsultarParametroGeral("nm_parametro_geral = 'qtd_dias_apos_nascimento'");


                var email_responsavel = "";
                string enviar_email = await _solicitacaoMatriculaPreNatalRepository.ConsultarParametroGeral("nm_parametro_geral = 'Enviar_Email_Real_Mae_Paulistana'");
                if (enviar_email == "S")
                {
                    try
                    {
                        DispositivoComunicacaoResponsavel dispositivoComunicacao = preNatais[0].NrCnsResponsavelNavigation.CdResponsavelGeralNavigation.DispositivoComunicacaoResponsavel.ToList<DispositivoComunicacaoResponsavel>()[0];
                        email_responsavel = string.IsNullOrEmpty(dispositivoComunicacao.NmEmailResponsavel) ? "" : dispositivoComunicacao.NmEmailResponsavel;
                    }
                    catch (Exception)
                    {
                        email_responsavel = "";
                    }

                }
                else {
                    email_responsavel = "swilians@prodam.sp.gov.br;rdelgado@prodam.sp.gov.br";
                }
                //nao enviar email se nao tiver cadastrado
                if (email_responsavel == "") {
                    return;
                }

               var nomeDe = "maepaulistanacreche@sme.prefeitura.sp.gov.br";
                var emailDe = "maepaulistanacreche@sme.prefeitura.sp.gov.br";
                var emailPara = email_responsavel;
                var emailCC = "";
                var assuntoMensagem = "Inscrição Mãe-Paulistana Creche";
                var data_atual = DateTime.Now;
                var data_formatada = "São Paulo, " + Convert.ToDateTime(data_atual).ToString("dd 'de' MMMM 'de' yyyy", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")) + ".";

                baseUri = baseUri + "img/";//caminho das imagens
                var strHtml = "<html>";
                strHtml = strHtml + "<body>";
                strHtml = strHtml + "    <table width='800' align='center' style='font-family:sans-serif'>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='2'  width='10%' style='height:140px;'>";
                strHtml = strHtml + "                <img src='" + baseUri + "img_mae_paulistana_creche.png' >";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "            <td colspan='1' width='80%' align='right' style='height:140px;'>";
                strHtml = strHtml + "                <img src='" + baseUri + "logo_Prefeitura_Horizontal.png' >";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-family:sans-serif;font-size:16px;width:100%;height:100px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                <p>"+ data_formatada + "</p>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-family:sans-serif;font-size:16px;width:100%;height:100px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                <p>Cara (o) munícipe,</p>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-size:16px;width:100%;height:50px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                    <p>Recebemos sua inscrição do Programa Mãe Paulistana Creche para o pré-cadastro de vaga em creche, sob o(s) número(s) de solicitação:  " + solicitacoesMatriculas + ".</p>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-size:16px;width:100%;height:50px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                    <p>Esse cadastro permitirá atendimento com maior brevidade em uma das Unidades Educacionais da Rede Municipal de Ensino.</p>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-size:16px;width:100%;height:50px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                <p>Após o nascimento, no prazo máximo de " + qtd_dias + " dias, é necessário efetuar o registro do nascimento no programa Mãe-Paulistana Creche, para garantir sua participação.</p>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-size:16px;width:100%;height:100px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                <p>Em caso de dúvidas, envie email para maepaulistanacreche@sme.prefeitura.sp.gov.br que faremos contato.</p>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-size:16px;width:100%;height:100px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                <p>Atenciosamente,</p>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-size:16px;width:100%;height:50px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "                Secretaria Municipal de Educação<br />";
                strHtml = strHtml + "                Prefeitura da Cidade de São Paulo";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='3' style='font-size:16px;width:100%;height:15px;vertical-align:top;font-weight: bold'>";
                strHtml = strHtml + "               <hr width='100%' style='color:#118197'>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "        <tr>";
                strHtml = strHtml + "            <td colspan='1'  width='10%' style='height:100px;'>";
                strHtml = strHtml + "                <img src='" + baseUri + "logo_Educacao.png' >";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "            <td colspan='1'  align='left' width='10%' style='height:100px;'>";
                strHtml = strHtml + "                <img src='" + baseUri + "/logo_Saude.png' >";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "            <td colspan='1' width='80%' align='right' style='height:100px;'>";
                strHtml = strHtml + "                 <div class='col s12 m3 l2 right'><a class='email' href='mailto:maepaulistanacreche@sme.prefeitura.sp.gov.br' target='_blank' alt='E-mail de contato' title='E-mail' tabindex='82'><img src='" + baseUri + "icone_Email.png'>mailto:maepaulistanacreche@sme.prefeitura.sp.gov.br</a>";
                strHtml = strHtml + "                 <p><img src='" + baseUri + "icone_Telefone.png' title='Telefone' alt='Telefone' tabindex='83'><span class='alinhamento_vertical_top'>(11) 3396-0346</span></p></div>";
                strHtml = strHtml + "            </td>";
                strHtml = strHtml + "        </tr>";
                strHtml = strHtml + "    </table>";
                strHtml = strHtml + "</body>";
                strHtml = strHtml + "</html>";

                var deliveryMethod = _configuration["SMTP:DeliveryMethod"];
                var diretorio = _configuration["SMTP:PickupDirectoryLocation"];
                var host  = _configuration["SMTP:Host"];
                var port = string.IsNullOrEmpty(_configuration["SMTP:Port"])?0: Convert.ToInt32(_configuration["SMTP:Port"]);
                var enableSsl = string.IsNullOrEmpty(_configuration["SMTP:EnableSsl"])? false: (_configuration["SMTP:EnableSsl"].ToString().ToLower() == "true"? true:false);

                EnvioEmail.EnviarEmailGenerico(nomeDe, emailDe, emailPara, emailCC, assuntoMensagem, strHtml, null, deliveryMethod, diretorio, host, port, enableSsl);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }


}

