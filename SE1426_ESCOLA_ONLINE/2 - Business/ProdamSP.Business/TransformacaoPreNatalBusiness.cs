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
using ProdamSP.Domain.Models.Cadastro.Pessoa;
using ProdamSP.Domain.Enums;

namespace ProdamSP.Business
    {
        public class TransformacaoPreNatalBusiness : Business<SolicitacaoMatriculaPreNatal, int>, ITransformacaoPreNatalBusiness
        {
            private IUnitOfWork _uow;
            private ISolicitacaoMatriculaPreNatalRepository _solicitacaoMatriculaPreNatalRepository = null;
            private ISolicitacaoMatriculaPreNatalBusiness _solicitacaoMatriculaPreNatalBusiness = null;
            private readonly IConfiguration _configuration;
            private IPessoaServiceBusiness _pessoaServiceBusiness;

            public TransformacaoPreNatalBusiness(ISolicitacaoMatriculaPreNatalRepository solicitacaoMatriculaPreNatalRepository,
                ISolicitacaoMatriculaPreNatalBusiness solicitacaoMatriculaPreNatalBusiness,
                IUnitOfWork uow, IConfiguration configuration,
                IPessoaServiceBusiness pessoaServiceBusiness
              )
                : base(solicitacaoMatriculaPreNatalRepository)
            {
                _solicitacaoMatriculaPreNatalRepository = solicitacaoMatriculaPreNatalRepository;
                _solicitacaoMatriculaPreNatalBusiness = solicitacaoMatriculaPreNatalBusiness;
                _uow = uow;
                _configuration = configuration;
                _pessoaServiceBusiness = pessoaServiceBusiness;
            }


            //----------------------------------------------------------------------------------------------------------------------------------------------------
            //Consulta Cns da Crianca no Siga
            //----------------------------------------------------------------------------------------------------------------------------------------------------
            public async Task<TransformacaoConsultaCriancaRetornoModel> ConsultarDadosCriancaSIGA(TransformacaoConsultaCriancaModel transformacaoConsultaCriancaModel)
        {
            try
            {
                TransformacaoConsultaCriancaRetornoModel dadosRetorno = new TransformacaoConsultaCriancaRetornoModel();

                var numeroCns = transformacaoConsultaCriancaModel.NrCnsCrianca;
                var listaPessoaSIGAModel = await _pessoaServiceBusiness.PesquisarAsPessoaSIGA(numeroCns);
                if (listaPessoaSIGAModel == null) {
                    dadosRetorno.CodRetorno = 99;
                    dadosRetorno.MsgRetorno = "CNS da criança não encontrado.";
                    return dadosRetorno;
                }

                var pessoa = listaPessoaSIGAModel.FirstOrDefault();

                //consultar dados da mae
                var numeroCnsResponsavel = transformacaoConsultaCriancaModel.NrCnsResponsavel;
                var numeroSisPreNatal = transformacaoConsultaCriancaModel.NrPreNatal;
                var cdSolicitacaoMatricula = Convert.ToInt32(transformacaoConsultaCriancaModel.CdSolicitacaoMatricula);

                List<PreNatalModel> preNatais = await _solicitacaoMatriculaPreNatalBusiness.PesquisarPreNataisEOL(numeroCnsResponsavel, numeroSisPreNatal, cdSolicitacaoMatricula);
                if (preNatais == null) {
                    dadosRetorno.CodRetorno = 99;
                    dadosRetorno.MsgRetorno = "Dados do pré-natal não encontrado.";
                    return dadosRetorno;
                }

                //dados da mãe
                var preNatal = preNatais.Where(x => x.MatriculaPreNatal.CdSolicitacaoMatricula == cdSolicitacaoMatricula).FirstOrDefault();
                var dtInscricaoEol = preNatal.MatriculaPreNatal.DtInscricaoEol;
                
                
                if (dtInscricaoEol.HasValue)
                {
                    dadosRetorno.InNascimentoAnteriorInscricao = false;
                    if (pessoa.DataNascimento.HasValue) {
                        if (pessoa.DataNascimento.Value.Date < dtInscricaoEol.Value.Date) {
                            dadosRetorno.InNascimentoAnteriorInscricao = true;
                        }
                    }
                }
                else 
                {
                    dadosRetorno.CodRetorno = 99;
                    dadosRetorno.MsgRetorno = "Data de inscrição no eol inválida";
                    return dadosRetorno;
                }

                dadosRetorno.DadosMae = PreencherDadosMae(preNatal);
                dadosRetorno.DadosCrianca =PreencherDadosCrianca(pessoa);

                if (dadosRetorno.DadosMae.NmMaeCrianca != dadosRetorno.DadosCrianca.NmMae)
                {
                    dadosRetorno.InNomeMaeDiferente = true;
                }

                var cdCepMae = dadosRetorno.DadosMae.CdCep.HasValue ? dadosRetorno.DadosMae.CdCep.Value.ToString("00000000") : "";

                var cdCepCrianca = "";

                try
                {
                    cdCepCrianca = string.IsNullOrEmpty(pessoa.Endereco.Cep) ? "" : Convert.ToInt32(pessoa.Endereco.Cep).ToString("00000000");
                }
                catch (Exception)
                {
                    cdCepCrianca = "";
                }
                


                var nmLogradouroMae = string.IsNullOrEmpty(dadosRetorno.DadosMae.NmLogradouro) ? "" : dadosRetorno.DadosMae.NmLogradouro.Trim();
                var nmLogradouroCrianca = string.IsNullOrEmpty(pessoa.Endereco.Logradouro) ? "" : pessoa.Endereco.Logradouro.Trim();

                var cdNrEnderecoMae = string.IsNullOrEmpty(dadosRetorno.DadosMae.CdNrEndereco) ? "": dadosRetorno.DadosMae.CdNrEndereco.Trim();
                var cdNrEnderecoCrianca = string.IsNullOrEmpty(pessoa.Endereco.NumeroResidencia) ? "" : pessoa.Endereco.NumeroResidencia.Trim();
                
                var dcComplementoEnderecoMae = string.IsNullOrEmpty(dadosRetorno.DadosMae.DcComplementoEndereco) ? "" : dadosRetorno.DadosMae.DcComplementoEndereco.Trim();
                var dcComplementoEnderecoCrianca = string.IsNullOrEmpty(pessoa.Endereco.Complemento) ? "" : pessoa.Endereco.Complemento.Trim();


                if (cdCepMae != cdCepCrianca  ||
                    nmLogradouroMae != nmLogradouroCrianca ||
                    cdNrEnderecoMae != cdNrEnderecoCrianca ||
                    dcComplementoEnderecoMae != dcComplementoEnderecoCrianca)
                {
                    dadosRetorno.InEnderecoDiferente = true;
                }

                return dadosRetorno;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private DadosCriancaModel PreencherDadosCrianca(PessoaSIGAModel pessoa) {
            if (pessoa == null) {
                return null;
            }

            DadosCriancaModel dadosCrianca = new DadosCriancaModel();
            dadosCrianca.NrCnsCrianca = string.IsNullOrEmpty(pessoa.NumeroCns) ? "" : pessoa.NumeroCns;
            dadosCrianca.NmCrianca = string.IsNullOrEmpty(pessoa.Nome) ? "" : pessoa.Nome;
            dadosCrianca.NmMae = string.IsNullOrEmpty(pessoa.NomeMae) ? "" : pessoa.NomeMae;
            dadosCrianca.NmPai = string.IsNullOrEmpty(pessoa.NomePai) ? "" : pessoa.NomePai;
         
            dadosCrianca.DtNascimentoCrianca = pessoa.DataNascimento;
            dadosCrianca.SexoCrianca =  string.IsNullOrEmpty(pessoa.CodigoSexoSus)? "": pessoa.CodigoSexoSus;

            try
            {
                dadosCrianca.TpRacaCorCrianca = Convert.ToInt32(pessoa.CodigoRacaSus);
            }
            catch (Exception)
            {
                dadosCrianca.TpRacaCorCrianca = null;
            }
                
           
            dadosCrianca.CdPaisOrigemCrianca =  string.IsNullOrEmpty(pessoa.CodigoPaisNascimento)? "": pessoa.CodigoPaisNascimento;

            try
            {
                
                dadosCrianca.CdCpfCrianca = Convert.ToDecimal(pessoa.NumeroCpf);
            }
            catch (Exception)
            {
                dadosCrianca.CdCpfCrianca = null;
            }
            

            if (pessoa.Identidade != null)
            {
                dadosCrianca.NrRgCrianca = string.IsNullOrEmpty(pessoa.Identidade.Numero) ? "" : pessoa.Identidade.Numero;
                dadosCrianca.SgUfRgCrianca = Enum.GetName(typeof(SiglaUFEnum), Convert.ToInt32(pessoa.Identidade.CodigoUfSus));
            }

            if (pessoa.IdentidadeEstrangeiro != null) {
                dadosCrianca.NrDocumentoEstrangeiroCrianca = string.IsNullOrEmpty(pessoa.IdentidadeEstrangeiro.Numero) ? "" : pessoa.IdentidadeEstrangeiro.Numero;
            }


            if (pessoa.Endereco != null) {
                //Endereco
                dadosCrianca.DcTpLogradouroCrianca = string.IsNullOrEmpty(pessoa.Endereco.TipoLogradouro) ? "" : pessoa.Endereco.TipoLogradouro;
                dadosCrianca.NmLogradouroCrianca = string.IsNullOrEmpty(pessoa.Endereco.Logradouro) ? "" : pessoa.Endereco.Logradouro;
                dadosCrianca.CdNrEnderecoCrianca = string.IsNullOrEmpty(pessoa.Endereco.NumeroResidencia) ? "" : pessoa.Endereco.NumeroResidencia;
                dadosCrianca.DcComplementoEnderecoCrianca = string.IsNullOrEmpty(pessoa.Endereco.Complemento) ? "" : pessoa.Endereco.Complemento;
                dadosCrianca.NmBairroCrianca = string.IsNullOrEmpty(pessoa.Endereco.Bairro) ? "" : pessoa.Endereco.Bairro;
                try
                {
                    dadosCrianca.CdCepCrianca = string.IsNullOrEmpty(pessoa.Endereco.Cep) ? 0 : Convert.ToInt32(pessoa.Endereco.Cep);
                }
                catch (Exception)
                {
                    dadosCrianca.CdCepCrianca = 0;
                }

                dadosCrianca.NmMunicipioCrianca = "";
            }

            if (pessoa.Contato != null) {
                //Contato
                dadosCrianca.CdDddCelularCrianca = string.IsNullOrEmpty(pessoa.Contato.DddCelular) ? "" : pessoa.Contato.DddCelular;
                dadosCrianca.DcDispositivoCelularCrianca = string.IsNullOrEmpty(pessoa.Contato.TelefoneCelular) ? "" : pessoa.Contato.TelefoneCelular;
                dadosCrianca.CdDddComercialCrianca = string.IsNullOrEmpty(pessoa.Contato.DddComercial) ? "" : pessoa.Contato.DddComercial;
                dadosCrianca.DcDispositivoComercialCrianca = string.IsNullOrEmpty(pessoa.Contato.TelefoneComercial) ? "" : pessoa.Contato.TelefoneComercial;
                dadosCrianca.CdDddResidencialCrianca = string.IsNullOrEmpty(pessoa.Contato.DddResidencial) ? "" : pessoa.Contato.DddResidencial;
                dadosCrianca.DcDispositivoResidencialCrianca = string.IsNullOrEmpty(pessoa.Contato.TelefoneResidencial) ? "" : pessoa.Contato.TelefoneResidencial;
                dadosCrianca.EmailCrianca = string.IsNullOrEmpty(pessoa.Contato.Email) ? "" : pessoa.Contato.Email;
            }

            dadosCrianca.FoneticaNmCrianca =  "";

            return dadosCrianca;
        }

        
        public async Task<SolicitacaoVagaRetornoModel> SolicitarVaga(SolicitacaoVagaModel solicitacaoVagaModel)
        {
            try
            {
                FoneticaBusiness fonetica = new FoneticaBusiness();
                solicitacaoVagaModel.DadosMae.FoneticaNmMaeCrianca= String.IsNullOrEmpty(solicitacaoVagaModel.DadosMae.NmMaeCrianca) ? "" : fonetica.Fonetizar(solicitacaoVagaModel.DadosMae.NmMaeCrianca);
                solicitacaoVagaModel.DadosCrianca.FoneticaNmPaiCrianca = String.IsNullOrEmpty(solicitacaoVagaModel.DadosCrianca.NmPai) ? "" : fonetica.Fonetizar(solicitacaoVagaModel.DadosCrianca.NmPai);
                solicitacaoVagaModel.DadosCrianca.FoneticaNmCrianca = String.IsNullOrEmpty(solicitacaoVagaModel.DadosCrianca.NmCrianca) ? "" : fonetica.Fonetizar(solicitacaoVagaModel.DadosCrianca.NmCrianca);

                var retorno = await _solicitacaoMatriculaPreNatalRepository.SolicitarVaga(solicitacaoVagaModel);

                return retorno;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private DadosMaeModel PreencherDadosMae(PreNatalModel preNatal)
        {
            //popular dados da mãe
            if (preNatal == null) { return null; }

            var responsavel = preNatal.Responsavel;

            DadosMaeModel dadosMae = new DadosMaeModel();

            dadosMae.NmMaeCrianca = string.IsNullOrEmpty(responsavel.NmResponsavel) ? "" : responsavel.NmResponsavel.Trim();

            if (responsavel.DtNascimentoMaeResponsavel.HasValue)
                dadosMae.DtNascimentoMaeCrianca = responsavel.DtNascimentoMaeResponsavel.Value;

            dadosMae.CdPaisOrigemMae = string.IsNullOrEmpty(responsavel.CdPaisOrigemMae) ? "" : responsavel.CdPaisOrigemMae;
            
            dadosMae.CdCpfResponsavel = responsavel.CdCpfResponsavel;
            
            dadosMae.NrRgResponsavel = string.IsNullOrEmpty(responsavel.NrRgResponsavel) ? "" : responsavel.NrRgResponsavel; 
            dadosMae.SgUfRgResponsavel = string.IsNullOrEmpty(responsavel.SgUfRgResponsavel) ? "" : responsavel.SgUfRgResponsavel;
            dadosMae.NrDocumentoEstrangeiro = string.IsNullOrEmpty(responsavel.NrDocumentoEstrangeiro) ? "" : responsavel.NrDocumentoEstrangeiro;

            dadosMae.DcTpLogradouro = string.IsNullOrEmpty(responsavel.DcTpLogradouro) ? "" : responsavel.DcTpLogradouro;
            dadosMae.NmLogradouro = string.IsNullOrEmpty(responsavel.NmLogradouro) ? "" : responsavel.NmLogradouro;
            dadosMae.CdNrEndereco = string.IsNullOrEmpty(responsavel.CdNrEndereco) ? "" : responsavel.CdNrEndereco;
            dadosMae.DcComplementoEndereco = string.IsNullOrEmpty(responsavel.DcComplementoEndereco) ? "" : responsavel.DcComplementoEndereco;
            dadosMae.NmBairro = string.IsNullOrEmpty(responsavel.NmBairro) ? "" : responsavel.NmBairro;
            if (responsavel.CdCep.HasValue)
            {
                dadosMae.CdCep = Convert.ToInt32(responsavel.CdCep);
            }
            
            
            dadosMae.NmMunicipio = string.IsNullOrEmpty(responsavel.NmMunicipio) ? "" : responsavel.NmMunicipio;

            dadosMae.CdDddCelularMae = string.IsNullOrEmpty(responsavel.CdDddCelularResponsavel) ? "" : responsavel.CdDddCelularResponsavel;
            dadosMae.DcDispositivoCelularMae = string.IsNullOrEmpty(responsavel.NrCelularResponsavel) ? "" : responsavel.NrCelularResponsavel;

            dadosMae.CdDddComercialMae = string.IsNullOrEmpty(responsavel.CdDddTelefoneComercialResponsavel) ? "" : responsavel.CdDddTelefoneComercialResponsavel;
            dadosMae.DcDispositivoComercialMae = string.IsNullOrEmpty(responsavel.NrTelefoneComercialResponsavel) ? "" : responsavel.NrTelefoneComercialResponsavel;

            dadosMae.CdDddResidencialMae = string.IsNullOrEmpty(responsavel.CdDddTelefoneFixoResponsavel) ? "" : responsavel.CdDddTelefoneFixoResponsavel;
            dadosMae.DcDispositivoResidencialMae = string.IsNullOrEmpty(responsavel.NrTelefoneFixoResponsavel) ? "" : responsavel.NrTelefoneFixoResponsavel;

            dadosMae.EmailResponsavel = string.IsNullOrEmpty(responsavel.NmEmailResponsavel) ? "" : responsavel.NmEmailResponsavel;
            dadosMae.FoneticaNmMaeCrianca = "";

            return dadosMae;

        }
    }

}