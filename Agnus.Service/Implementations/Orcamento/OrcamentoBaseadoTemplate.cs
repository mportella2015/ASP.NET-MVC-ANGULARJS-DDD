using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    public class OrcamentoBaseadoTemplate : OrcamentoManager
    {
        IEntityService<Template> _templateService;        

        public OrcamentoBaseadoTemplate(OrcamentoService orcamentoService)
            : base(orcamentoService)
        {
            this._templateService = this._OrcamentoService.GetEnityServiceInstance<Template>();
        }

        public override void CriarOrcamento(Domain.Models.ProjetoOrcamento orcamento)
        {
            var grupoAproacao = this.CriarGrupoAprovacao();

            orcamento.GrupoAprovacao.Add(grupoAproacao);
            var etapas = this.SelecionarEtapas(orcamento.Fases);

            var template = _templateService.GetBy(orcamento.IdTemplateOrcamento.Value);
            var itensGrupo = this.SelecionarItens(template.TemplateGrupos);

            var listaOrcamentoTotal = this.CriarItensOrcamentoTotal(etapas, itensGrupo);
            this.JuntarOrcamentosParteEmpresa(listaOrcamentoTotal);
            this.AdicionarOrcamentosTotais(orcamento, listaOrcamentoTotal);
            this.AdicionarGrupoAprovacao(listaOrcamentoTotal, grupoAproacao);
        }

        private OrcamentoTotal CriarOrcamentoTotal(ProjetoOrcamentoEtapa etapa, TemplateItem item)
        {             
            var orcamentoTotal = new OrcamentoTotal();
            orcamentoTotal.NomeFase = etapa.Fase.NomeFase;          
            orcamentoTotal.NomeEtapa = etapa.NomeEtapa;            
            orcamentoTotal.CodGrupo = item.TemplateGrupo.Codigo;
            orcamentoTotal.NomeGrupo = item.TemplateGrupo.Nome;            
            orcamentoTotal.CodItem = item.Codigo;           
            orcamentoTotal.NomeItem = item.Nome;
            orcamentoTotal.IdItemPlanoContas = item.IdItemPlanoContas;
            orcamentoTotal.TxtFormulaCalculo = item.Formula;            
            this._GrupoAprovacaoService.SaveEntity(orcamentoTotal);
            return orcamentoTotal;
        }

        private List<OrcamentoTotal> CriarItensOrcamentoTotal(List<ProjetoOrcamentoEtapa> etapas, List<TemplateItem> itensGrupo)
        {
            var itensOrcamentoTotal = new List<OrcamentoTotal>();
            etapas.ForEach(e => itensGrupo.ForEach(i => itensOrcamentoTotal.Add(this.CriarOrcamentoTotal(e, i))));
            return itensOrcamentoTotal;
        }

        private List<TemplateItem> SelecionarItens(ICollection<TemplateGrupo> grupos)
        {
            var itens = (from g in grupos
                         from i in g.TemplateOrcamentoItems
                         select i).ToList();
            return itens;
        }
    }
}
