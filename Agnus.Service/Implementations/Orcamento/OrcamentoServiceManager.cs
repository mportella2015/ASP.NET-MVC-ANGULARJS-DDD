using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework.Helper;
using Agnus.Service.DTO;
using Agnus.Service.DTO.Orcamento;
using Agnus.Service.Implementations;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    internal class OrcamentoServiceManager
    {
        IOrcamentoService _service;
        public IOrcamentoService OrcamentoService { get { return _service; } private set { } }
        Dictionary<long, long> _monitorAlteracaoId;


        public OrcamentoServiceManager(IOrcamentoService service)
        {
            _service = service;
            _monitorAlteracaoId = new Dictionary<long, long>();
        }
        internal void VerificarEtapas(ICollection<ProjetoOrcamentoFase> fases)
        {
            if (fases == null || fases.Count == 0)
                throw new CustomServiceException("Ao menos uma Fase de estar cadastrada!");
            foreach (var fase in fases)
            {
                if (fase.Etapas == null || fase.Etapas.Count == 0)
                    fase.Etapas = new List<ProjetoOrcamentoEtapa>() { new ProjetoOrcamentoEtapa() { NomeEtapa = "ÚNICA" } };
                foreach (var etapa in fase.Etapas)
                    etapa.Fase = fase;
            }
        }

        internal void VerificarFases(ProjetoOrcamento entity)
        {
            if (entity.Fases != null)
            {
                var fasesEmpty = entity.Fases.Where(x => string.IsNullOrEmpty(x.NomeFase)).ToList();
                foreach (var fase in fasesEmpty)
                    entity.Fases.Remove(fase);
                if (entity.Fases.Count == 0)
                    entity.Fases = new List<ProjetoOrcamentoFase>() { new ProjetoOrcamentoFase() { NomeFase = "ÚNICA" } };
            }
        }

        internal void VerificarRegraOrcamentoTemplate(ProjetoOrcamento entity)
        {
            if (!this.ValidarRegraOrcamentoTemplate(entity))
                throw new CustomServiceException("Para cadastrar uma nova versão deve-se escolher o Template ou uma grupo de orçamentos como base.");

        }

        internal bool ValidarRegraOrcamentoTemplate(ProjetoOrcamento entity)
        {
            return (entity.IdTemplateOrcamento != null && entity.OrcamentosBase.Count == 0) || (entity.OrcamentosBase.Count > 0 && entity.IdTemplateOrcamento == null);
        }



        internal void AttachEntitys(ProjetoOrcamento entity)
        {
            _service.AttachAny(entity.Midias);
            _service.AttachAny(entity.Territorios);
            if (entity.OrcamentosBase.Count > 0)
            {
                var orcamentoBaseList = entity.OrcamentosBase.ToList();
                orcamentoBaseList.RemoveAll(item => item == null);
                entity.OrcamentosBase = orcamentoBaseList.ToArray();

                _service.AttachAny(entity.OrcamentosBase);
            }
        }

        internal OrcamentoManager CriarIntanciaManager(ProjetoOrcamento entity)
        {
            if (this.OrcamentoComBaseTemplate(entity))
                return new OrcamentoBaseadoTemplate((OrcamentoService)_service);
            else if (this.OrcamentoComBaseOrcamentos(entity))
                return new OrcamentoBaseadoOrcamentos((OrcamentoService)_service);
            return null;
        }

        internal bool OrcamentoComBaseTemplate(ProjetoOrcamento entity)
        {
            return entity.IdTemplateOrcamento.HasValue;
        }

        internal bool OrcamentoComBaseOrcamentos(ProjetoOrcamento entity)
        {
            return entity.OrcamentosBase.Any();
        }

        internal object AtualizarDadosOrcamentoTotal(ProjetoOrcamento orcamento, LogAtualizacaoDTO itemAtualizacao)
        {
            var orcamentoTotalService = _service.GetEnityServiceInstance<OrcamentoTotal>();
            var orcamentoSimilar = orcamentoTotalService.GetBy(itemAtualizacao.IdOrcamentoSimilar);
            var orcamentoTotal = itemAtualizacao.Id > 0 ? orcamento.OrcamentoTotal.FirstOrDefault(x => x.Id == itemAtualizacao.Id) : this.CriarOrcamentoSimilar(orcamentoSimilar);
            if (itemAtualizacao.ValorTotalAlterado)
                orcamentoTotal.AlterarValorTotal(itemAtualizacao.ValorTotal.HasValue ? itemAtualizacao.ValorTotal.Value : 0);
            if (itemAtualizacao.IsDetalhe && !string.IsNullOrEmpty(itemAtualizacao.ValorNome))
                orcamentoTotal.NomeDetalhe = string.IsNullOrEmpty(itemAtualizacao.ValorNome) ? orcamentoTotal.NomeDetalhe : itemAtualizacao.ValorNome;
            else
                orcamentoTotal.NomeItem = string.IsNullOrEmpty(itemAtualizacao.ValorNome) ? orcamentoTotal.NomeItem : itemAtualizacao.ValorNome;
            if (itemAtualizacao.ValorObseracaoAlterado)
                orcamentoTotal.TxtObservacao = itemAtualizacao.ValorObservacao;
            orcamentoTotal.DirecionarFormulaPai = itemAtualizacao.AtualizarIdPai;
            orcamentoTotal.IdVirtualSubstituicao = itemAtualizacao.Id;
            try
            {
                orcamentoTotalService.Save(orcamentoTotal);
                if (itemAtualizacao.IdOrcamentoSimilar != 0 && itemAtualizacao.IsDetalhe)
                    _monitorAlteracaoId.Add(itemAtualizacao.Id, orcamentoTotal.Id);
            }
            catch (Exception)
            {
                throw new Exception(
                    string.Format("Não foi possível atualizar o item '{0}/{1}/{2}/{3}{4}'",
                    orcamentoTotal.NomeFase,
                    orcamentoTotal.NomeEtapa,
                    orcamentoTotal.NomeGrupo,
                    orcamentoTotal.NomeItem,
                    string.IsNullOrEmpty(orcamentoTotal.NomeDetalhe) ? string.Empty : orcamentoTotal.NomeDetalhe));
            }
            return null;
        }

        private OrcamentoTotal CriarOrcamentoSimilar(OrcamentoTotal similar)
        {
            return new OrcamentoTotal
            {
                NomeGrupo = similar.NomeGrupo,
                CodGrupo = similar.CodGrupo,
                NomeItem = similar.NomeItem,
                CodItem = similar.CodItem,
                NomeFase = similar.NomeFase,
                NomeEtapa = similar.NomeEtapa,
                IdProjetoOrcamento = similar.IdProjetoOrcamento,
                IdGrupoAprovacao = similar.IdGrupoAprovacao,
                IdItemPlanoContas = similar.IdItemPlanoContas,
                IsNew = true
            };
        }

        internal void AlterarStatusOrcamento(ProjetoOrcamento orcamento, StatusOrcamentoEnum statusOrcamento)
        {
            var _dominioService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();
            orcamento.StatusOrcamento = _dominioService.BuscarConteudoPor(DominioGenericoEnum.StatusOrcamento, (int)statusOrcamento);
            orcamento.IdStatusOrcamento = orcamento.StatusOrcamento.Id;
            orcamento.DataStatusOrcamento = DateTime.Now;
        }

        internal void AlterarStatusGrupoAprovacao(GrupoAprovacao grupoAprovacao, StatusOrcamentoEnum statusOrcamento)
        {
            var _dominioService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();
            grupoAprovacao.StatusOrcamento = _dominioService.BuscarConteudoPor(DominioGenericoEnum.StatusOrcamento, (int)statusOrcamento);
            grupoAprovacao.IdStatusOrcamento = grupoAprovacao.StatusOrcamento.Id;
            grupoAprovacao.DataStatusOrcamento = DateTime.Now;
        }

        internal void CriarItensOrcamento(GrupoAprovacao grupoAprovacao)
        {
            var listaAdjacencia = _service.GerarGrafoOrcamentoPorData(grupoAprovacao.ProjetoOrcamento);
            var group =
                from o in grupoAprovacao.OrcamentoTotal
                group o by new
                {
                    o.NomeFase,
                    o.NomeEtapa,
                    o.NomeGrupo,
                    o.NomeItem,
                    o.IdItemPlanoContas,
                    o.IdProjetoOrcamento
                };

            foreach (var item in group)
            {
                var itemOrcamento = new ItemOrcamento();
                if (item.Key.IdItemPlanoContas.HasValue)
                    itemOrcamento.IdItemPlanoContas = item.Key.IdItemPlanoContas.Value;
                itemOrcamento.NomeFase = item.Key.NomeFase;
                itemOrcamento.NomeEtapa = item.Key.NomeEtapa;
                itemOrcamento.NomeGrupo = item.Key.NomeGrupo;
                itemOrcamento.NomeItem = item.Key.NomeItem;
                itemOrcamento.OrcamentoAtual.Add(new OrcamentoAtual() { ValorItemReal = item.ToList().Sum(x => x.ValorTotal), IdProjetoOrcamento = item.Key.IdProjetoOrcamento });
                grupoAprovacao.ProjetoOrcamento.ItemOrcamento.Add(itemOrcamento);
            }
        }


        internal object ManterDadosOrcamentoPeriodo(ProjetoOrcamento orcamento, LogAtualizacaoDTO logAtualizacao)
        {
            var orcamentoTotal = orcamento.OrcamentoTotal.FirstOrDefault(x => x.Id == logAtualizacao.Id);
            //if (logAtualizacao.ValorMeses.Sum(x => x.Valor) != orcamentoTotal.ValorTotal)
            //    throw new CustomServiceException("O valor total do item deve ser aplicado em pelo menos um mês do período orçamentário.");
            if (orcamentoTotal != null)
            {
                logAtualizacao.ValorMeses
                    .ForEach(delegate(MesPeriodoOrcamentoDTO mes)
                    {
                        if (!string.IsNullOrEmpty(mes.Mes))
                            this.ManterOrcamentoPeriodo(mes, orcamentoTotal);
                    });
            }
            return null;
        }

        private void ManterOrcamentoPeriodo(MesPeriodoOrcamentoDTO mes, OrcamentoTotal orcamentoTotal)
        {
            var date = Util.ConverterStringDataPraDatetime(mes.Mes.Replace('_', '/'));
            var orcamentoPeriodo = orcamentoTotal.OrcamentoPeriodo.FirstOrDefault(x => x.NumMes == date.Month && x.NumAno == date.Year);
            if (orcamentoPeriodo == null)
                orcamentoPeriodo = this.CriarOrcamentoPeriodo(orcamentoTotal, date);
            orcamentoPeriodo.ValorItem = mes.Valor.HasValue ? mes.Valor.Value : 0;
            orcamentoPeriodo.Percentual = mes.Percentual;
            var orcamentoPeriodoService = _service.GetEnityServiceInstance<OrcamentoPeriodo>();
            orcamentoPeriodoService.Save(orcamentoPeriodo);
        }

        private void ManterOrcamentoAtual(MesPeriodoOrcamentoDTO mes, ItemOrcamento itemOrcamento, OrcamentoAtual orcamentoAtualTotal = null)
        {
            var date = Util.ConverterStringDataPraDatetime(mes.Mes.Replace("p_", "").Replace('_', '/'));
            var orcamentoAtual = itemOrcamento.GetOrcamentoAtualPorData(date.Month, date.Year);
            var valorAnterior = orcamentoAtual.ValorItemProdutor;
            orcamentoAtual.ValorItemProdutor = mes.Valor.HasValue ? mes.Valor.Value : 0;
            var orcamentoAtualService = _service.GetEnityServiceInstance<OrcamentoAtual>();
            orcamentoAtualService.Save(orcamentoAtual);
            if (orcamentoAtualTotal != null)
            {
                orcamentoAtualTotal.ValorItemProdutor += (orcamentoAtual.ValorItemProdutor - valorAnterior);
                orcamentoAtualService.Save(orcamentoAtualTotal);
            }
        }



        internal OrcamentoPeriodo CriarOrcamentoPeriodo(OrcamentoTotal orcamentoTotal, DateTime date)
        {
            var orcamentoPeriodo = new OrcamentoPeriodo();
            orcamentoPeriodo.NumAno = date.Year;
            orcamentoPeriodo.NumMes = date.Month;
            orcamentoPeriodo.DataInicio = new DateTime(date.Year, date.Month, 1);
            orcamentoPeriodo.DataFim = orcamentoPeriodo.DataInicio.AddMonths(1).AddDays(-1);
            orcamentoPeriodo.IdOrcamentoTotal = orcamentoTotal.Id;
            return orcamentoPeriodo;
        }

        internal int[] ObterNiveisPermissaoOrcamento(ProjetoOrcamento orcamento)
        {
            var statusPermissoes = new List<int>();
            if (this.VerificarSeOrcamentoEstaDisputa(orcamento))
            {
                statusPermissoes.Add(orcamento.StatusOrcamento.Codigo);
                statusPermissoes.Add(orcamento.Projeto.StatusProjeto.Codigo);
                statusPermissoes.Add(orcamento.Projeto.Nucleo.Codigo == (int)NucleoEnum.Cinema ? 1 : 0);
            }
            else
                statusPermissoes = new List<int>() { 0, 0, 0 };
            return statusPermissoes.ToArray();
        }

        private bool VerificarSeOrcamentoEstaDisputa(ProjetoOrcamento orcamento)
        {
            var possiveisStatusProjeto = new List<int>();
            var possiveisStatusOrcamento = new List<int>();
            possiveisStatusOrcamento.AddRange(new List<int>() { (int)StatusOrcamentoEnum.PreOrcamento, (int)StatusOrcamentoEnum.EmRevisao });
            possiveisStatusProjeto.Add((int)StatusProjetoEnum.Registrado);
            if (orcamento.Projeto.Nucleo.Codigo == (int)NucleoEnum.Cinema)
                possiveisStatusProjeto.Add((int)StatusProjetoEnum.Aguardando_distribuicao);
            return possiveisStatusProjeto.Contains(orcamento.Projeto.StatusProjeto.Codigo) || !possiveisStatusOrcamento.Contains(orcamento.StatusOrcamento.Codigo);
        }

        internal bool ProjetoPossuiOrcamentoAprovado(long idProjeto)
        {
            var pjService = DependencyResolver.Current.GetService<IProjetoService>();
            return pjService.ExisteOrcamentoAprovado(idProjeto);            
        }

        internal IEnumerable<dynamic> BuildOrcamentoViewer(IEnumerable<ProjetoOrcamento> orcamentos)
        {
            var orcamentoViewer = new List<dynamic>();
            if (orcamentos != null && orcamentos.Count() >= 0)
            {
                orcamentos.ToList().ForEach(
                    delegate(ProjetoOrcamento orcamento)
                    {
                        dynamic d =
                            new
                            {
                                Id = orcamento.Id,
                                Viewer = string.Format("{0} - {1} - {2} - {3} - {4} - {5}",
                                            orcamento.DataInicioOrcamento.ToShortDateString(),
                                            orcamento.DataFimOrcamento.ToShortDateString(),
                                            orcamento.Escopo,
                                            orcamento.StatusOrcamento.Texto,
                                            orcamento.DataAprovacao.HasValue ? orcamento.DataAprovacao.Value.ToShortDateString() : string.Empty,
                                            orcamento.ValorTotalOrcamento)
                            };
                        orcamentoViewer.Add(d);
                    });
            }
            return orcamentoViewer;
        }

        internal List<string> GetNomesPeriodos(ProjetoOrcamento orcamento)
        {
            var strNomes = new List<string>();
            var numMeses = ((orcamento.DataFimOrcamento.Year - orcamento.DataInicioOrcamento.Year) * 12) + orcamento.DataFimOrcamento.Month - orcamento.DataInicioOrcamento.Month;
            for (int i = 0; i <= numMeses; i++)
            {
                var data = orcamento.DataInicioOrcamento.AddMonths(i);
                strNomes.Add(data.ToString("MMM/yyyy"));
            }
            return strNomes;
        }

        internal ListaAdjacenciaDTO GerarGrafoOrcamento<T>(ProjetoOrcamento orcamento, List<T> itens, Func<ListaAdjacenciaDTO, ProjetoOrcamento, List<string>, object> funcBindPeriodo = null)
        {
            var manager = new ListaAdjacenciaManager();
            var listaAdjacencia = manager.CriarListaAdjacencia(itens);
            if (funcBindPeriodo != null)
            {
                var nomesPeriodo = this.GetNomesPeriodos(orcamento);
                funcBindPeriodo(listaAdjacencia, orcamento, nomesPeriodo);
            }
            return listaAdjacencia;
        }

        internal object ManterDadosOrcamentoProdutor(ProjetoOrcamento orcamento, LogAtualizacaoDTO logAtualizacao)
        {
            var itemOrcamento = orcamento.ItemOrcamento.FirstOrDefault(x => x.Id == logAtualizacao.Id);
            var orcamentoAtualTotal = itemOrcamento.OrcamentoAtual.FirstOrDefault(x => x.NumMes == null && x.NumAno == null);
            if (logAtualizacao.ValorMeses.Sum(x => x.Valor) > orcamentoAtualTotal.ValorItemReal)
                throw new CustomServiceException("O valor distribuido para o produtor não deve ser maior que o valor total do item.");
            logAtualizacao.ValorMeses
                .ForEach(delegate(MesPeriodoOrcamentoDTO mes)
                {
                    this.ManterOrcamentoAtual(mes, itemOrcamento, orcamentoAtualTotal);
                });
            if (logAtualizacao.PercentualProdutorAlterado)
            {
                itemOrcamento.Percentual = logAtualizacao.PercentualProdutor;
                var itemOrcamentoService = DependencyResolver.Current.GetService<IEntityService<ItemOrcamento>>();
                itemOrcamentoService.Save(itemOrcamento);
            }
            return null;
        }

        internal void AtualizarStatusProjeto(ProjetoOrcamento orcamento, StatusProjetoEnum statusProjeto)
        {
            var _dominioService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();
            orcamento.Projeto.StatusProjeto = _dominioService.BuscarConteudoPor(DominioGenericoEnum.StatusProjeto, (int)statusProjeto);
            orcamento.Projeto.IdStatusProjeto = orcamento.Projeto.StatusProjeto.Id;
            var projetoService = DependencyResolver.Current.GetService<IProjetoService>();
            projetoService.Save(orcamento.Projeto);
        }

        internal void SalvarNovaVersao(ProjetoOrcamento entity)
        {
            var faseService = DependencyResolver.Current.GetService<IEntityService<ProjetoOrcamentoFase>>();
            var etapaService = DependencyResolver.Current.GetService<IEntityService<ProjetoOrcamentoEtapa>>();
            this.VerificarFases(entity);
            this.VerificarEtapas(entity.Fases);
            var bkpFases = entity.Fases.ToList();
            entity.Fases.Clear();
            this.AttachEntitys(entity);
            _service.SaveEntity(entity);
            ((OrcamentoService)_service).Commit();
            foreach (var fase in bkpFases)
            {
                var nFase = new ProjetoOrcamentoFase() { IdOrcamento = entity.Id, NomeFase = fase.NomeFase };
                foreach (var etapa in fase.Etapas)
                    nFase.Etapas.Add(new ProjetoOrcamentoEtapa() { IdFase = fase.Id, NomeEtapa = etapa.NomeEtapa, Fase = nFase });
                faseService.Save(nFase);
            }
        }

        internal void ManterFormulas(List<LogAtualizacaoDTO> list, ListaAdjacenciaDTO lista, ProjetoOrcamento orcamento)
        {
            var fxManager = new FxManager();
            var orcamentoTotalService = _service.GetEnityServiceInstance<OrcamentoTotal>();
            list.ForEach(delegate(LogAtualizacaoDTO itemAtualizacao)
            {
                var idEntidade = _monitorAlteracaoId.ContainsKey(itemAtualizacao.Id) ? _monitorAlteracaoId[itemAtualizacao.Id] : itemAtualizacao.Id;
                var orcamentoTotal = orcamento.OrcamentoTotal.FirstOrDefault(x => x.Id == idEntidade);
                if (itemAtualizacao.CampoFormulaAlterado && orcamentoTotal != null)
                {
                    orcamentoTotal.TxtFormulaCalculo = itemAtualizacao.Formula == null ? null : fxManager.ConverterObjectoFormula(orcamento, itemAtualizacao.Formula, lista);
                    orcamentoTotalService.Save(orcamentoTotal);
                }
            });
        }

        internal object RecuperarIdContasDetalheAdicionado(List<LogAtualizacaoDTO> list, ListaAdjacenciaManager listaAdjacenciaManager, ListaAdjacenciaDTO lista)
        {
            var listaItensPaiAtualizado = new List<dynamic>();
            list.Where(z => z.AtualizarId).ToList().ForEach(delegate(LogAtualizacaoDTO itemAtualizacao)
            {
                var item = lista.Listas.FirstOrDefault(x => x.No.IdVirtualSubstituicao == itemAtualizacao.Id);
                if (item != null)
                    listaItensPaiAtualizado.Add(new { idNovo = item.No.Estrutura.Id, idAntigo = itemAtualizacao.Id });
                if (itemAtualizacao.AtualizarIdPai)
                {
                    item = lista.Listas.FirstOrDefault(x => x.NosAdjacentes.Any(z => z.Estrutura.Id == itemAtualizacao.Id));
                    if (item != null)
                        listaItensPaiAtualizado.Add(new { idNovo = item.No.Estrutura.Id, idAntigo = itemAtualizacao.Id });
                }
            });
            return listaItensPaiAtualizado;
        }

        internal void GerarItensParaProdutor(ProjetoOrcamento orcamento)
        {
            var itens =
                from ot in orcamento.OrcamentoTotal
                from op in ot.OrcamentoPeriodo
                group op by new
                {
                    ot.NomeFase,
                    ot.NomeEtapa,
                    ot.NomeGrupo,
                    ot.NomeItem,
                    op.NumAno,
                    op.NumMes
                };

            foreach (var item in itens)
            {
                var itemOrcamento = orcamento.ItemOrcamento.FirstOrDefault(x => x.NomeGrupo == item.Key.NomeGrupo && x.NomeFase == item.Key.NomeFase && x.NomeEtapa == item.Key.NomeEtapa && x.NomeItem == item.Key.NomeItem);
                var orcAtual = new OrcamentoAtual()
                    {
                        NumAno = item.Key.NumAno,
                        NumMes = item.Key.NumMes,
                        ValorItemReal = item.Sum(z => z.ValorItem),
                        IdProjetoOrcamento = orcamento.Id,
                        IdItemOrcamento = itemOrcamento.Id
                    };
                itemOrcamento.OrcamentoAtual.Add(orcAtual);
            }
        }

        internal bool ValidarDistribuicao(ICollection<OrcamentoTotal> orcamentosTotais)
        {
            return !orcamentosTotais.Any(z => z.ValorTotal != z.OrcamentoPeriodo.Sum(x => x.ValorItem));
        }

        internal void LiberarDistribuicaoTempo(ProjetoOrcamento orcamento)
        {
            this.AtualizarStatusProjeto(orcamento, StatusProjetoEnum.Aguardando_Aprovacao);
        }

        internal bool OrcamentoPossuiPeriodoDistribuido(ProjetoOrcamento projetoOrcamento)
        {
            return projetoOrcamento.OrcamentoTotal.Any(x => x.OrcamentoPeriodo.Any());
        }

        internal object ManterDadosOrcamentoAlteracaoReal(ProjetoOrcamento orcamento, LogAtualizacaoDTO logAtualizacao)
        {
            var orcamentoTotalBase = orcamento.OrcamentoTotal.FirstOrDefault(z => z.Id == logAtualizacao.Id);
            var itemOrcamento = orcamentoTotalBase.ItemOrcamentoCorrespondente;
            var orcTotalAlteracao = this.SelecionarReflexoAlteracaoOrcamentoTotal(orcamentoTotalBase);
            logAtualizacao.ValorMeses
                .ForEach(delegate(MesPeriodoOrcamentoDTO m)
                {
                    var strMes = m.Mes.Replace("p_", "").Replace('_', '/');
                    var date = Util.ConverterStringDataPraDatetime(strMes);
                    var orcamentoAtual = itemOrcamento.OrcamentoAtual.FirstOrDefault(z => z.NumAno == date.Year && z.NumMes == date.Month);
                    if (orcamentoAtual != null && orcamentoAtual.ValorUsoProdutor > (m.Valor + orcamentoAtual.ValorItemReal))
                        throw new CustomServiceException(
                            string.Format("Em {0}:{1}:{2}:{3}, no mes {4} o valor está menor do que o já utilizado pelo produtor.",
                                itemOrcamento.NomeFase,
                                itemOrcamento.NomeEtapa,
                                itemOrcamento.NomeGrupo,
                                itemOrcamento.NomeItem,
                                strMes));
                    var mesAlteracao = this.SelecionarReflexoAlteracaoOrcamentoPeriodo(orcTotalAlteracao, date);
                    mesAlteracao.ValorItem = m.Valor.HasValue ? m.Valor.Value : 0;
                    orcTotalAlteracao.OrcamentoPeriodo.Add(mesAlteracao);
                });
            orcTotalAlteracao.ValorTotal = orcTotalAlteracao.OrcamentoPeriodo.Sum(x => x.ValorItem);
            return null;
        }

        private OrcamentoTotal SelecionarReflexoAlteracaoOrcamentoTotal(OrcamentoTotal similar)
        {
            var alteracao = similar.ProjetoOrcamento.OrcamentoTotal
                .FirstOrDefault(x =>
                    x.Id != similar.Id &&
                    x.IdGrupoAprovacao == null &&
                    x.NomeFase == similar.NomeFase &&
                    x.NomeEtapa == similar.NomeEtapa &&
                    x.NomeGrupo == similar.NomeGrupo &&
                    x.NomeItem == similar.NomeItem &&
                    x.NomeDetalhe == similar.NomeDetalhe);
            if (alteracao == null)
            {
                alteracao = this.CriarOrcamentoSimilar(similar);
                alteracao.IdGrupoAprovacao = null;
                similar.ProjetoOrcamento.OrcamentoTotal.Add(alteracao);
            }
            return alteracao;
        }

        private OrcamentoPeriodo SelecionarReflexoAlteracaoOrcamentoPeriodo(OrcamentoTotal orcTotal, DateTime date)
        {
            var alteracao = orcTotal.OrcamentoPeriodo.FirstOrDefault(x => x.NumMes == date.Month && x.NumAno == date.Year);
            if (alteracao == null)
                alteracao = this.CriarOrcamentoPeriodo(orcTotal, date);
            return alteracao;
        }
    }
}
