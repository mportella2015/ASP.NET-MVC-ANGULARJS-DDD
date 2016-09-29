using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Framework.Helper;
using Agnus.Service.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    public class WorkFlowService : EntityService<WorkflowAprovacao>, IWorkFlowService
    {
        private IEntityService<WorkFlow> _workflowService = DependencyResolver.Current.GetService<IEntityService<WorkFlow>>();
        private IEntityService<ProjetoPessoa> _projetoPessoaService = DependencyResolver.Current.GetService<IEntityService<ProjetoPessoa>>();
        private IEntityService<CentroCusto> _centroCustoService = DependencyResolver.Current.GetService<IEntityService<CentroCusto>>();
        private IEntityService<Nucleo> _nucleoService = DependencyResolver.Current.GetService<IEntityService<Nucleo>>();
        private IEntityService<WorkFlowEntity> _workflowEntityService = DependencyResolver.Current.GetService<IEntityService<WorkFlowEntity>>();
        private ITabelaGenericaDominioService _tabelaGenericaService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();
        private IEntityService<OrcamentoProdutorUso> _orcamentoCompUsoService = DependencyResolver.Current.GetService<IEntityService<OrcamentoProdutorUso>>();
        private IEntityService<OrcamentoAtual> _orcamentoAtualService = DependencyResolver.Current.GetService<IEntityService<OrcamentoAtual>>();
        private IEntityService<Pessoa> _pessoaService = DependencyResolver.Current.GetService<IEntityService<Pessoa>>();
        private IEntityService<PessoaPapel> _pessoaPapelService = DependencyResolver.Current.GetService<IEntityService<PessoaPapel>>();

        public WorkFlowService(IUnitOfWork unitOfWork, IGenericRepository<WorkflowAprovacao> repository)
            : base(unitOfWork, repository)
        {
        }

        public void ProcessWorkFlowItens(IWorkFlowEntity wfEntity, ItemWorkFlowVO itemWs, long dominioStatusFilho, int? dominioStatusPai = null, int? codStatusPai = null, bool todosItensReprovados = false)
        {

            var wfaDataSource = this.BuscarConfiguracoesWorkFlow(itemWs);

            Func<WorkflowAprovacao, bool> predicate;
            if (!itemWs.IsAlcadaDireta)
                predicate = x => x.ValorAlcada > itemWs.UltimaAlcada;
            else
                predicate = x => x.ValorAlcada >= itemWs.ValorAprovacao;

            if (wfEntity != null)
            {
                int codStatusFilho = 0;
                var wf = GetWorkFlow(predicate, wfaDataSource, wfEntity, itemWs);
                var parecer = _tabelaGenericaService.BuscarConteudoPorId(DominioGenericoEnum.ParecerAprovação, Convert.ToInt32(itemWs.IdParecerAprovacao));
                var enumParecer = parecer != null ? (ParecerAprovacaoEnum)parecer.Codigo : ParecerAprovacaoEnum.Aprovado;
                if (wf != null && wf.IdParecerAprovacao.HasValue)
                {
                    if (itemWs.IsProjetoFundo)
                    {
                        codStatusFilho = wfEntity.GetCodStatusItemBy(enumParecer);
                    }
                    else
                    {

                        if (enumParecer == ParecerAprovacaoEnum.Aprovado)
                        {
                            enumParecer = ParecerAprovacaoEnum.EmAprovacao;
                        }
                        else if (enumParecer == ParecerAprovacaoEnum.Revisar)
                        {
                            var wfEdit = _workflowService.GetBy(itemWs.IdWf.Value);
                            wfEdit.DataParecer = DateTime.Now;
                            wfEdit.IdPessoaAprovador = wf.IdPessoaAprovador;
                            _workflowService.Save(wfEdit);
                        }
                        codStatusFilho = wfEntity.GetCodStatusItemBy(enumParecer);
                    }
                }
                else
                {
                    if (wf == null)
                    {
                        var listPaiFilho = StatusPaiFilhoEmRevisao(wfEntity.GetType().BaseType.Name);

                        if (enumParecer == ParecerAprovacaoEnum.Revisar)
                        {
                            codStatusPai = listPaiFilho[0];
                            codStatusFilho = listPaiFilho[1];

                            var wfEdit = _workflowService.GetBy(itemWs.IdWf.Value);
                            wfEdit.DataParecer = DateTime.Now;
                            _workflowService.Save(wfEdit);
                        }
                        else
                        {
                            codStatusFilho = enumParecer == ParecerAprovacaoEnum.Reprovado ? wfEntity.StatusReprovado : wfEntity.StatusAprovado;
                            if (wfEntity.GetType().BaseType.Name == "Realizado")
                            {
                                codStatusPai = codStatusFilho;
                            }
                            else
                            {
                                if (itemWs.HasStatusRevisao && enumParecer == ParecerAprovacaoEnum.Aprovado)
                                {
                                    codStatusPai = listPaiFilho[0];
                                    codStatusFilho = wfEntity.StatusEmAprovacao;
                                }
                                else
                                {
                                    codStatusPai = wfEntity.StatusCodPaiFinalizado;
                                }
                            }
                        }

                        dominioStatusPai = wfEntity.StatusDominioPai;
                        if (wfEntity.ItemOrcamentoEntity != null)
                            ProcessarRegrasOrcamentos(wfEntity);
                    }
                    else
                    {
                        codStatusFilho = wfEntity.StatusEmAprovacao;
                    }
                }

                var statusFilho = _tabelaGenericaService.BuscarConteudoPor((DominioGenericoEnum)dominioStatusFilho, codStatusFilho);
                var textoStatus = statusFilho.GetType().GetProperty("Texto").GetValue(statusFilho);
                var statusIsReprovado = textoStatus.Equals("Reprovado") || textoStatus.Equals("Reprovada");
                wfEntity.SetStatusItem(statusFilho, statusIsReprovado || codStatusPai == wfEntity.StatusCodPaiFinalizado);

                if (todosItensReprovados)
                {
                    dominioStatusPai = wfEntity.StatusDominioPai;
                    codStatusPai = wfEntity.StatusCodPaiFinalizado;
                }

                if (dominioStatusPai.HasValue && codStatusPai.HasValue) UpdateStatusPai(wfEntity, dominioStatusPai.Value, codStatusPai.Value, todosItensReprovados);

                if (itemWs.IdWf.HasValue)
                {
                    var wfEdit = _workflowService.GetBy(itemWs.IdWf.Value);
                    wfEdit.DataParecer = DateTime.Now;
                    wfEdit.TxtJustificativa = itemWs.TxtJustificativa;
                    wfEdit.IdParecerAprovacao = itemWs.IdParecerAprovacao;
                    _workflowService.Save(wfEdit);
                    if (statusIsReprovado) return;
                }

                if (wf != null)
                {
                    EnviarEmailWf(wf.IdPessoaAprovador);
                    if (!(itemWs.HasStatusRevisao))
                        _workflowService.Save(wf);
                }
                _unitOfWork.Commit();
            }
        }

        public List<int> StatusPaiFilhoEmRevisao(string nomeObjetoAprovacao)
        {
            var listCodPaiFilho = new List<int>();
            switch (nomeObjetoAprovacao)
            {
                case "SolicitacaoReembolsoIPC":
                    listCodPaiFilho.Add((int)StatusReembolsoEnum.EmRevisao);
                    listCodPaiFilho.Add((int)StatusItemReembolsoEnum.EmRevisao);
                    break;
                case "Realizado":
                    var codStatusRealizado = (int)StatusRealizadoEnum.EmRevisao;
                    listCodPaiFilho.Add(codStatusRealizado);
                    listCodPaiFilho.Add(codStatusRealizado);
                    break;
                case "SolicitacaoAdiantamento":
                    listCodPaiFilho.Add((int)StatusSolicitacaoAdiantamentoEnum.EmRevisao);
                    listCodPaiFilho.Add((int)StatusSolicitacaoAdiantamentoEnum.EmRevisao);
                    break;
                case "PedidoCompraItem":
                    listCodPaiFilho.Add((int)StatusPedidoCompraEnum.EmRevisao);
                    listCodPaiFilho.Add((int)StatusItemPedidoCompraEnum.EmRevisao);
                    break;
                case "PrestacaoContasDetalhes":
                    listCodPaiFilho.Add((int)StatusPrestacaoContasEnum.EmRevisao);
                    listCodPaiFilho.Add((int)StatusPrestacaoContasDetalhesEnum.EmRevisao);
                    break;
                case "GrupoAprovacao":
                    listCodPaiFilho.Add((int)StatusOrcamentoEnum.EmRevisao);
                    listCodPaiFilho.Add((int)StatusOrcamentoEnum.EmRevisao);
                    break;

                default:
                    return null;
                    break;
            }
            return listCodPaiFilho;
        }

        public IEnumerable<WorkflowAprovacao> BuscarConfiguracoesWorkFlow(ItemWorkFlowVO itemWs)
        {
            return GetAllByFilter(x => x.IdObjetoAprovacao == itemWs.IdObjAprovacao && x.NumNivel == itemWs.Nivel);
        }

        private void UpdateStatusFilho(long dominioStatusFilho, int codStatusFilho, int? codStatusPai, IWorkFlowEntity wfEntity)
        {
            var statusFilho = _tabelaGenericaService.BuscarConteudoPor((DominioGenericoEnum)dominioStatusFilho, codStatusFilho);
            wfEntity.SetStatusItem(statusFilho, statusFilho.GetType().GetProperty("Texto").GetValue(statusFilho) == "Reprovado" || codStatusPai == wfEntity.StatusCodPaiFinalizado);
        }

        public void EnviarEmailWf(long idPessoa)
        {
            if (idPessoa != 0)
            {
                var pessoa = _pessoaService.GetBy(idPessoa);
                var url = string.Format("{0}/{1}/{2}/ParecerPedidoCompra", HttpContext.Current.Request.Url.AbsoluteUri.Split('/'));
                var corpo = pessoa.NomePessoa + ", <br/> há uma pendência que precisa de seu parecer.<br/><a href='" + url + "'>Link para a página</a>.";
                pessoa.UsuarioSistema.ToList().ForEach(x => Email.EnvioEmailService(x.TxtEmail, corpo, assunto: "Conspiração Web - Pendência"));
            }
        }

        public void UpdateStatusPai(IWorkFlowEntity wfEntity, int dominioStatusPai, int codStatusPai, bool todosItensReprovados)
        {
            codStatusPai = todosItensReprovados ? wfEntity.StatusCodPaiFinalizado : codStatusPai;
            var status = _tabelaGenericaService.BuscarConteudoPor((DominioGenericoEnum)dominioStatusPai, codStatusPai);
            wfEntity.SetStatusPai(status);
        }

        public WorkFlow GetWorkFlow(Func<WorkflowAprovacao, bool> dataFilter, IEnumerable<WorkflowAprovacao> wfaDataSource, IWorkFlowEntity wfEntity, ItemWorkFlowVO itemWs)
        {
            wfaDataSource = wfaDataSource.Where(dataFilter).OrderBy(x => x.ValorAlcada);
            if (wfaDataSource.Any())
            {
                var wfa = wfaDataSource.FirstOrDefault();
                var proxNivel = DefinirProximoNivel(wfaDataSource.Count(), wfa.ValorAlcada.Value, itemWs);
                var aprovador = BuscarDadosAprovador(wfa.Pessoa, wfa.PapelAprovador, itemWs);

                if (itemWs.IdAprovador.HasValue)
                {
                    aprovador = _pessoaService.GetBy(itemWs.IdAprovador.Value);
                }


                if (!proxNivel.HasValue && aprovador == null) return null;

                var wfObjEntity = new WorkFlowEntity();
                wfObjEntity.SetEntity(wfEntity);
                _workflowEntityService.Save(wfObjEntity);

                var wf = new WorkFlow();
                wf.IdPessoaAprovador = aprovador.Id;
                if (proxNivel.HasValue)
                {
                    wf.NumProximoNivel = proxNivel.Value;
                }
                wf.NumNivel = itemWs.Nivel;
                wf.ValorAlcada = wfa.ValorAlcada;
                wf.ValorSLA = wfa.ValorSLAHora.HasValue ? wfa.ValorSLAHora.Value : 0;
                wf.IdWorkflowAprovacao = wfa.Id;
                wf.DataNotificacao = DateTime.Now;
                wf.IdParecerAprovacao = itemWs.IdParecerAprovacao;
                wf.WorkFlowEntity = wfObjEntity;
                wf.TxtJustificativa = itemWs.TxtJustificativa;

                return wf;
            }
            return null;
        }

        public int? DefinirProximoNivel(int count, decimal? valorAlcada, ItemWorkFlowVO itemWs)
        {
            return count > 1 && valorAlcada.Value < itemWs.ValorAprovacao ? itemWs.Nivel : BuscarProximoNivel(itemWs);
        }

        public int? BuscarProximoNivel(ItemWorkFlowVO itemWs)
        {
            //var wfa = GetAllByFilter(x => x.IdObjetoAprovacao == itemWs.IdObjAprovacao && x.NumNivel > itemWs.Nivel && x.IndAtivo).FirstOrDefault();
            var wfa = GetAllByFilter(x => x.IdObjetoAprovacao == itemWs.IdObjAprovacao).OrderBy(z => z.NumNivel).FirstOrDefault(x => x.NumNivel > itemWs.Nivel && x.IndAtivo);
            if (wfa != null && itemWs.PassosWorkflowList != null && itemWs.PassosWorkflowList.Count > 0 && itemWs.PassosWorkflowList[wfa.NumNivel - 1] == 0)
            {
                //itemWs.Nivel++;              
                var nItem = new ItemWorkFlowVO { Nivel = itemWs.Nivel + 1, PassosWorkflowList = itemWs.PassosWorkflowList, IdObjAprovacao = itemWs.IdObjAprovacao };
                return BuscarProximoNivel(nItem);
            }
            return wfa != null ? (int?)wfa.NumNivel : null;

            //var wfa = GetAllByFilter(x => x.IdObjetoAprovacao == itemWs.IdObjAprovacao && x.NumNivel > itemWs.Nivel).FirstOrDefault();
            //return wfa != null ? (int?)wfa.NumNivel : null;
        }

        public Pessoa BuscarDadosAprovador(Pessoa aprovador, ConteudoTabelaDominio papel, ItemWorkFlowVO itemWs)
        {
            if (aprovador != null) return aprovador;

            if (itemWs.IdProjeto.HasValue)
            {
                var papelCoord = _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.PapelPessoaProjeto, (int)PapelEnum.CoordenadorProducao).Id;
                var projetoPessoa = _projetoPessoaService.GetAllByFilter(x => x.IdProjeto == itemWs.IdProjeto.Value && x.IdPapel == papelCoord).FirstOrDefault();
                if (projetoPessoa != null) return projetoPessoa.Pessoa;
            }

            if (itemWs.IdCentroCusto.HasValue)
            {
                var centroCusto = _centroCustoService.GetAllQuery(x => x.Id == itemWs.IdCentroCusto.Value).FirstOrDefault();
                var centroCustoPessoa = _pessoaService.GetAllByFilter(x => x.CodPessoa == centroCusto.CodPessoaResponsavelCC).FirstOrDefault();
                if (centroCustoPessoa != null) return centroCustoPessoa;
            }

            if (itemWs.IdNucleo.HasValue)
            {
                var nucleo = _nucleoService.GetAllByFilter(x => x.Id == itemWs.IdNucleo.Value).FirstOrDefault();
                switch ((PapelEnum)papel.Codigo)
                {
                    case PapelEnum.DiretorCena://diretor assina cheque//no documento está diretor mas o mesmo não existe no banco
                        if (nucleo.PessoaDiretor != null) return nucleo.PessoaDiretor;
                        break;
                    case PapelEnum.CoordenadorProducao://produtor//no documento está coordenador mas o mesmo não existe no banco
                        if (nucleo.PessoaCoordenador != null) return nucleo.PessoaCoordenador;
                        break;
                        throw new Exception("Não foi possível encontrar o próximo aprovador, contate o administrador.");
                }
            }

            if (aprovador == null && papel != null)
            {
                return _pessoaPapelService
                                           .GetAllQuery(x => x.IdPapel == papel.Id)
                                           .Select(x => x.Pessoa)
                                           .OrderBy(x => x.NumAprovacoesPendentes)
                                           .FirstOrDefault();
            }

            return null;
        }

        public void ProcessarRegrasOrcamentos(IWorkFlowEntity wfEntity)
        {
            //TODO TIRAR DUVIDAS ORÇAMENTO
            var orcamentosVigentes = wfEntity.ItemOrcamentoEntity.OrcamentoAtual
                                                        .Where(x => x.ProjetoOrcamento != null && x.ProjetoOrcamento.DataAprovacao.HasValue &&
                                                                            (DateTime.Now >= x.DataInicio && DateTime.Now <= x.DataFim));
            orcamentosVigentes.ToList().ForEach(x => ComprometerOrcamentoProdutor(x, wfEntity));
        }

        public void ComprometerOrcamentoProdutor(OrcamentoAtual orcAtual, IWorkFlowEntity wfEntity)
        {
            var obj = new OrcamentoProdutorUso();
            obj.NumIdObjetoUso = wfEntity.IdEntity;
            obj.ValorUso = wfEntity.ValorUsoEntity;
            obj.OrcamentoAtual = orcAtual;
            obj.ObjetoComprometimentoOrcamento = _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.ObjetoComprometimentoOrcamento, wfEntity.CodObjetoUso);
            _orcamentoCompUsoService.Save(obj);

            orcAtual.ValorUsoProdutor += wfEntity.ValorUsoEntity;
            _orcamentoAtualService.AttachEntity(orcAtual);
            _orcamentoAtualService.Save(orcAtual);
        }
    }
}
