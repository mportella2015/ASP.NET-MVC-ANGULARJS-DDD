using Agnus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    public class OrcamentoBaseadoOrcamentos : OrcamentoManager
    {
        Dictionary<long,long> _referenciaIds;
        public OrcamentoBaseadoOrcamentos(OrcamentoService orcamentoService)
            : base(orcamentoService)
        {
            _referenciaIds = new Dictionary<long, long>();
        }

        public override void CriarOrcamento(ProjetoOrcamento orcamento)
        {
            var grupoAproacao = this.CriarGrupoAprovacao();
            orcamento.GrupoAprovacao.Add(grupoAproacao);
            var etapas = this.SelecionarEtapas(orcamento.Fases);
            var orcamentosBase = this.SelecionarOrcamentos(orcamento);
            var orcamentosTotaisBase = this.SelecionarOrcamentosTotais(orcamentosBase);
            var orcamentosTotais = this.CriarItensOrcamentoTotal(etapas, orcamentosTotaisBase);
            this.RebindFormulas(orcamentosTotais, orcamentosTotaisBase);
            this.JuntarOrcamentosParteEmpresa(orcamentosTotais);
            //this.AdicionarOrcamentosTotais(orcamento, orcamentosTotais);
            this.AdicionarGrupoAprovacao(orcamentosTotais, grupoAproacao);
            //this.PersistirSemOrcamentosBase(orcamento, orcamentosBase);
            _OrcamentoService.Commit();
        }

        private void RebindFormulas(List<OrcamentoTotal> orcamentosTotais, List<OrcamentoTotal> orcamentosTotaisBase)
        {
            var fxManager = new FxManager();
            foreach (var orc in orcamentosTotais.Where(z => !string.IsNullOrEmpty(z.TxtFormulaCalculo)))
            {
                orc.TxtFormulaCalculo = fxManager.TransformarGrupoSomatorio(orc.TxtFormulaCalculo);
                this.AlterarReferencias(orc,orcamentosTotais,fxManager);
            }            
        }

        private void AlterarReferencias(OrcamentoTotal orc, List<OrcamentoTotal> orcamentosTotais,FxManager fxManager)
        {
            var refers = fxManager.SepararVariaveis(orc.TxtFormulaCalculo);
            foreach (var r in refers)
            {
                var idOld = r.Replace("{", "").Replace("}","");
                var _ref = _referenciaIds.FirstOrDefault(x => x.Value == long.Parse(idOld));
                orc.TxtFormulaCalculo = orc.TxtFormulaCalculo.Replace(idOld, _ref.Key.ToString());
            }            
        }

        private List<ProjetoOrcamento> SelecionarOrcamentos(ProjetoOrcamento orcamento)
        {
            var ids = orcamento.OrcamentosBase.Select(z => z.Id).ToList();
            var orcamentos = this._OrcamentoService.GetAllByFilter(x => ids.Contains(x.Id)).ToList();
            return orcamentos;
        }

        private List<OrcamentoTotal> SelecionarOrcamentosTotais(List<ProjetoOrcamento> orcamentosBase)
        {
            var orcamentoTotalService = DependencyResolver.Current.GetService<IEntityService<OrcamentoTotal>>();
            var ids = orcamentosBase.Select(x => x.Id);
            var orcamentosTotais = orcamentoTotalService.GetAllByFilter(x => ids.Contains(x.IdProjetoOrcamento) && x.CodGrupo != _codigoGrupoParteEmpresa).ToList();
            return this.MergeOrcamentos(orcamentosTotais);
        }

        private List<OrcamentoTotal> MergeOrcamentos(List<OrcamentoTotal> orcamentosTotais)
        {
            var itensAgrupados = this.AgruparItensOrcamentosBase(orcamentosTotais);
            var itensMerge = this.MergeItens(itensAgrupados);
            return itensMerge;
        }

        private List<OrcamentoTotal> MergeItens(List<OrcamentoTotal> itensAgrupados)
        {
            var detalheCount = 0;
            var itensMerge = new List<OrcamentoTotal>();
            var group =
                from o in itensAgrupados
                group o by new
                {
                    o.CodGrupo,
                    o.NomeGrupo,
                    o.CodItem,
                    o.NomeItem
                } into g
                select g;
            foreach (var item in group)            
                foreach (var orc in item.ToList())
                {
                    var o = new OrcamentoTotal()
                    {
                        CodGrupo = item.Key.CodGrupo,
                        CodItem = item.Key.CodItem,
                        NomeGrupo = item.Key.NomeGrupo,
                        NomeItem = item.Key.NomeItem,                        
                        ValorTotal = orc.ValorTotal,
                        IdItemPlanoContas = orc.IdItemPlanoContas,
                        //TxtFormulaCalculo = orc.TxtFormulaCalculo,
                        Id = orc.Id
                    };
                    if (item.Count() > 1)
                        o.NomeDetalhe = "DETALHE " + (++detalheCount);
                    itensMerge.Add(o);
                }
            return itensMerge;            
        }

        private List<OrcamentoTotal> AgruparItensOrcamentosBase(List<OrcamentoTotal> orcamentosTotais)
        {
            var orcamentosTotaisMerge =
                (from o in orcamentosTotais
                 group o by new
                 {
                     o.CodGrupo,
                     o.NomeGrupo,
                     o.CodItem,
                     o.NomeItem,
                     o.ValorTotal,
                     o.IdItemPlanoContas,
                     o.TxtFormulaCalculo
                 } into g
                 select new OrcamentoTotal
                 {
                     CodGrupo = g.Key.CodGrupo,
                     NomeGrupo = g.Key.NomeGrupo,
                     CodItem = g.Key.CodItem,
                     NomeItem = g.Key.NomeItem,
                     ValorTotal = g.Key.ValorTotal,
                     IdItemPlanoContas = g.Key.IdItemPlanoContas,
                     TxtFormulaCalculo = g.Key.TxtFormulaCalculo,
                     Id = g.Any(x=>!string.IsNullOrEmpty(x.TxtFormulaCalculo)) ? g.FirstOrDefault(x => !string.IsNullOrEmpty(x.TxtFormulaCalculo)).Id : 0
                 }).ToList();
            return orcamentosTotaisMerge;
        }

        private List<OrcamentoTotal> CriarItensOrcamentoTotal(List<ProjetoOrcamentoEtapa> etapas, List<OrcamentoTotal> itensOrcamentoTotal)
        {
            var itens = new List<OrcamentoTotal>();
            etapas.ForEach(e => itensOrcamentoTotal.ForEach(i => itens.Add(this.CriarOrcamentoTotal(e, i))));
            return itens;
        }

        private OrcamentoTotal CriarOrcamentoTotal(ProjetoOrcamentoEtapa etapa, OrcamentoTotal item)
        {
            var orcamentoTotal = new OrcamentoTotal()
            {
                NomeFase = etapa.Fase.NomeFase,
                NomeEtapa = etapa.NomeEtapa,
                CodGrupo = item.CodGrupo,
                NomeGrupo = item.NomeGrupo,
                CodItem = item.CodItem,
                NomeItem = item.NomeItem,
                NomeDetalhe = item.NomeDetalhe,
                IdItemPlanoContas = item.IdItemPlanoContas,
                ValorTotal = item.ValorTotal,
                TxtFormulaCalculo = item.TxtFormulaCalculo,
                IdProjetoOrcamento = etapa.Fase.IdOrcamento
            };
            this._GrupoAprovacaoService.Save(orcamentoTotal);
            _referenciaIds.Add(orcamentoTotal.Id, item.Id);
            return orcamentoTotal;
        }

        private void PersistirSemOrcamentosBase(ProjetoOrcamento orcamento, List<ProjetoOrcamento> orcamentosBase)
        {
            _OrcamentoService.DetachList(orcamento.OrcamentosBase);
            _OrcamentoService.Commit();
            orcamento.OrcamentosBase = orcamentosBase;
        }
    }
}
