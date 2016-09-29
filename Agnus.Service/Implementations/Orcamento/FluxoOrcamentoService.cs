using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Framework.Helper;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    public class FluxoOrcamentoService
    {
        OrcamentoServiceManager _orcamentoServiceManager;

        ITabelaGenericaDominioService _dominioService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();
        IWorkFlowService _workFlowService = DependencyResolver.Current.GetService<IWorkFlowService>();

        public FluxoOrcamentoService(
            OrcamentoService orcamentoService
            )
        {
            _orcamentoServiceManager = new OrcamentoServiceManager(orcamentoService);
        }
        public void SubmeterOrcamentoAprovacao(ProjetoOrcamento orcamento)
        {

            this.AtualizarStatusOrcamento(orcamento, Agnus.Domain.Models.Enum.StatusOrcamentoEnum.EmAprovacao);
            var grupoAprovacao = this.SelecionarGrupoAprovacao(orcamento, TipoOrcamentoEnum.Original);
            this.VincularItensOrcamentoGrupoAprovacao(orcamento.OrcamentoTotal, grupoAprovacao);
            this.AtualizarStatusProjeto(orcamento.Projeto, Agnus.Domain.Models.Enum.StatusProjetoEnum.Aguardando_Aprovacao);
            this.DispararWorkFlow(orcamento, grupoAprovacao);
            ((OrcamentoService)_orcamentoServiceManager.OrcamentoService).EditarOrcamento(orcamento);
        }

        public void SubmeterAprovacaoOrcamentoManutencao(ProjetoOrcamento orcamento)
        {
            if (orcamento._PossuiAprovacaoPendente)
                throw new CustomServiceException("Já existe uma solicitação de alteração pendente de aprovação para este orçamento.");
            var grupoAprovacao = this.SelecionarGrupoAprovacao(orcamento, TipoOrcamentoEnum.Manutencao);
            this.VincularItensOrcamentoGrupoAprovacao(orcamento.OrcamentoTotal.Where(x => x.IdGrupoAprovacao == null).ToList(), grupoAprovacao);
            this.DispararWorkFlow(orcamento, grupoAprovacao);
            ((OrcamentoService)_orcamentoServiceManager.OrcamentoService).EditarOrcamento(orcamento);
        }

        private void AttachAll(ProjetoOrcamento orcamento)
        {
            _orcamentoServiceManager.OrcamentoService.AttachAny(orcamento.OrcamentoTotal);
        }

        private void DispararWorkFlow(ProjetoOrcamento orcamento, GrupoAprovacao grupoAprovacao)
        {
            var itemWS = this.CriarItemWorkFlowOrcamento(orcamento, grupoAprovacao);
            try
            {
                _workFlowService.ProcessWorkFlowItens(grupoAprovacao, itemWS, (int)DominioGenericoEnum.StatusOrcamento);
            }
            catch (Exception)
            { throw new Exception("Não foi possível iniciar o Workflow. Sua solicitação não pode ser enviada."); }
        }

        private ItemWorkFlowVO CriarItemWorkFlowOrcamento(ProjetoOrcamento orcamento, GrupoAprovacao grupoAprovacao)
        {
            return new ItemWorkFlowVO()
            {
                IdObjAprovacao = _dominioService.BuscarConteudoPor(Domain.Models.Enum.DominioGenericoEnum.ObjetoAprovacaoWorkflow, (int)ObjetoAprovacaoEnum.Orcamento).Id,
                IsAlcadaDireta = false,
                ValorAprovacao = 0,//TODO: Não entendi esta parte da lógica! Como pode ValorAprovacao ser inteiro?
                UltimaAlcada = 0,
                IdNucleo = orcamento.Projeto.IdNucleo,
                IdProjeto = orcamento.IdProjeto,
                Nivel = 1
            };
        }

        private void AtualizarStatusProjeto(Projeto projeto, Domain.Models.Enum.StatusProjetoEnum statusProjetoEnum)
        {
            projeto.StatusProjeto = _dominioService.BuscarConteudoPor(Domain.Models.Enum.DominioGenericoEnum.StatusProjeto, (int)statusProjetoEnum);
            projeto.IdStatusProjeto = projeto.StatusProjeto.Id;
            var projetoService = DependencyResolver.Current.GetService<IProjetoService>();
            projetoService.Save(projeto);
        }

        private void AtualizarStatusOrcamento(ProjetoOrcamento orcamento, Domain.Models.Enum.StatusOrcamentoEnum statusOrcamentoEnum)
        {
            orcamento.StatusOrcamento = _dominioService.BuscarConteudoPor(Domain.Models.Enum.DominioGenericoEnum.StatusOrcamento, (int)statusOrcamentoEnum);
            orcamento.IdStatusOrcamento = orcamento.StatusOrcamento.Id;
        }

        private GrupoAprovacao CriarGrupoAprovacao(ProjetoOrcamento orcamento, TipoOrcamentoEnum tipoOrcamento)
        {
            var grupoAprovacao = new GrupoAprovacao();
            grupoAprovacao.IdProjetoOrcamento = orcamento.Id;
            grupoAprovacao.ProjetoOrcamento = orcamento;            
            grupoAprovacao.TipoOrcamento = _dominioService.BuscarConteudoPor(Domain.Models.Enum.DominioGenericoEnum.TipoOrcamento, (int)tipoOrcamento);
            grupoAprovacao.IdTipoOrcamento = grupoAprovacao.TipoOrcamento.Id;                                    
            return grupoAprovacao;
        }

        private GrupoAprovacao SelecionarGrupoAprovacao(ProjetoOrcamento orcamento, TipoOrcamentoEnum tipoOrcamento)
        {
            GrupoAprovacao grupoAprovacao = tipoOrcamento == TipoOrcamentoEnum.Original ? orcamento.GrupoAprovacao.FirstOrDefault(z => z.TipoOrcamento.Codigo == (int)TipoOrcamentoEnum.Original) : this.CriarGrupoAprovacao(orcamento, tipoOrcamento);
            var valorAprovacao = tipoOrcamento == TipoOrcamentoEnum.Manutencao ? orcamento.OrcamentoTotal.Where(x => x.IdGrupoAprovacao == null).Sum(x => x.ValorTotal) : orcamento.OrcamentoTotal.Sum(x => x.ValorTotal);
            grupoAprovacao.ValorOrcamentoAprovar = valorAprovacao;
            grupoAprovacao.DataStatusOrcamento = DateTime.Now;
            grupoAprovacao.StatusOrcamento = _dominioService.BuscarConteudoPor(DominioGenericoEnum.StatusOrcamento, (int)StatusOrcamentoEnum.EmAprovacao);
            grupoAprovacao.IdStatusOrcamento = grupoAprovacao.StatusOrcamento.Id;
            return grupoAprovacao;
        }

        private void VincularItensOrcamentoGrupoAprovacao(ICollection<OrcamentoTotal> itensOrcamento, GrupoAprovacao grupoAprovacao)
        {
            foreach (var item in itensOrcamento)
                item.GrupoAprovacao = grupoAprovacao;
        }

        internal void AtivarFluxo(ParecerAprovacaoEnum parecerAprovacaoEnum, GrupoAprovacao grupoAprovacao, bool ehUltimoAprovador)
        {
            switch (parecerAprovacaoEnum)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    {
                        if (ehUltimoAprovador)
                            this.AtualizarOrcamentoAprovado(grupoAprovacao);
                    }
                    break;
                case ParecerAprovacaoEnum.Reprovado:
                    this.AtualizarOrcamentoReprovado(grupoAprovacao);
                    break;
                case ParecerAprovacaoEnum.Revisar:
                    this.AtualizarOrcamentoRevisao(grupoAprovacao);
                    break;
                default:
                    break;
            }
        }

        private void AtualizarOrcamentoRevisao(GrupoAprovacao grupoAprovacao)
        {
            this.AtualizarStatusProjeto(grupoAprovacao.ProjetoOrcamento.Projeto, Agnus.Domain.Models.Enum.StatusProjetoEnum.Registrado);
            _orcamentoServiceManager.AlterarStatusGrupoAprovacao(grupoAprovacao, StatusOrcamentoEnum.EmRevisao);
        }

        private void AtualizarOrcamentoReprovado(GrupoAprovacao grupoAprovacao)
        {
            var orcamento = grupoAprovacao.ProjetoOrcamento;
            _orcamentoServiceManager.AlterarStatusOrcamento(orcamento, StatusOrcamentoEnum.Recusado);
            orcamento.Projeto.StatusProjeto = _dominioService.BuscarConteudoPor(DominioGenericoEnum.StatusProjeto, (int)StatusProjetoEnum.Recusado);
            orcamento.Projeto.ProjetoHistoricoStatus.Add(new ProjetoHistoricoStatus { IdStatusProjeto = orcamento.Projeto.StatusProjeto.Id, TxtJustificativa = "Orçamento recusado." });
            _orcamentoServiceManager.AlterarStatusGrupoAprovacao(grupoAprovacao, StatusOrcamentoEnum.Recusado);
            _orcamentoServiceManager.OrcamentoService.Save(orcamento);
        }

        private void AtualizarOrcamentoAprovado(GrupoAprovacao grupoAprovacao)
        {
            var orcamento = grupoAprovacao.ProjetoOrcamento;
            if (grupoAprovacao.TipoOrcamento.Codigo == (int)TipoOrcamentoEnum.Original)
            {
                _orcamentoServiceManager.AlterarStatusOrcamento(orcamento, StatusOrcamentoEnum.Aprovado);
                var novoStatusProjeto = _orcamentoServiceManager.OrcamentoService.DefinirNivelAtivaWorkFlow(orcamento.Projeto.Nucleo) == 2 ? StatusProjetoEnum.Aguardando_Produtor : StatusProjetoEnum.Aguardando_distribuicao;
                _orcamentoServiceManager.AtualizarStatusProjeto(orcamento, novoStatusProjeto);
                orcamento.Projeto.ProjetoHistoricoStatus.Add(new ProjetoHistoricoStatus { IdStatusProjeto = orcamento.Projeto.StatusProjeto.Id, TxtJustificativa = "Orçamento Aprovado" });
                orcamento.ValorTotalOrcamento = grupoAprovacao._ValorOrcamentoTotal;
                _orcamentoServiceManager.CriarItensOrcamento(grupoAprovacao);
                _orcamentoServiceManager.AlterarStatusGrupoAprovacao(grupoAprovacao, StatusOrcamentoEnum.Aprovado);
                if (_orcamentoServiceManager.OrcamentoPossuiPeriodoDistribuido(grupoAprovacao.ProjetoOrcamento))
                    _orcamentoServiceManager.GerarItensParaProdutor(grupoAprovacao.ProjetoOrcamento);
            }
            else
            {
                orcamento.ValorTotalOrcamento = grupoAprovacao._ValorOrcamentoTotal;
                foreach (var ot in grupoAprovacao.OrcamentoTotal)
                {
                    var otOriginal = ot.GetSimilar();
                    var itemOrcamento = otOriginal.ItemOrcamentoCorrespondente;
                    foreach (var op in ot.OrcamentoPeriodo)
                        itemOrcamento.OrcamentoAtual.FirstOrDefault(x => x.NumMes == op.NumMes && x.NumAno == op.NumAno).ValorItemReal += op.ValorItem;
                    itemOrcamento.OrcamentoAtual.FirstOrDefault(x => x.NumMes == null && x.NumAno == null).ValorItemReal = itemOrcamento.OrcamentoAtual.Where(x => x.NumMes != null && x.NumAno != null).Sum(s => s.ValorItemReal);
                }
            }
            orcamento.DataAprovacao = DateTime.Now;
            ((OrcamentoService)_orcamentoServiceManager.OrcamentoService).EditarOrcamento(orcamento);
        }

        internal void VerificarConsistencia(ProjetoOrcamento orcamento)
        {
            var niveisPermissionamento = _orcamentoServiceManager.ObterNiveisPermissaoOrcamento(orcamento);
            if (niveisPermissionamento[0] == (int)StatusOrcamentoEnum.PreOrcamento && niveisPermissionamento[1] == (int)StatusProjetoEnum.Aguardando_distribuicao)
            {
                orcamento.OrcamentoTotal.ToList()
                    .ForEach(delegate(OrcamentoTotal ot)
                    {
                        var somatorioPeriodos = ot.OrcamentoPeriodo.Sum(z => z.ValorItem);
                        if (somatorioPeriodos != ot.ValorTotal)
                            throw new CustomServiceException("O orçamento não está totalmente distribuido. Por favor, reveja a distribuição antes de submeter a aprovação.");
                    });
            }
            else if (niveisPermissionamento[0] == (int)StatusOrcamentoEnum.PreOrcamento && niveisPermissionamento[1] == (int)StatusProjetoEnum.Registrado)
            {
                if (orcamento.OrcamentoTotal.Sum(x => x.ValorTotal) == default(decimal))
                    throw new CustomServiceException("Nenhum item teve valores adicionados.");
            }
        }
    }
}
