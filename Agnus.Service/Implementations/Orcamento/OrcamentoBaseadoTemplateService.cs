using Conspiracao.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conspiracao.Service.Implementations;
using Conspiracao.Framework;
using Conspiracao.Service.Interfaces;
using Conspiracao.Domain.Models.Enum;

namespace Conspiracao.Service.Implementations
{
    public partial class OrcamentoService
    {        
        private void CriarOrcamentoBaseadoTemplate(ProjetoOrcamento entity)
        {            
            var grupoAproacao = this.CriarGrupoAprovacao();

            entity.GrupoAprovacao.Add(grupoAproacao);
            var etapas = this.SelecionarEtapas(entity.Fases);

            var templateService = this.GetEnityServiceInstance<Template>();
            var template = templateService.GetBy(entity.IdTemplateOrcamento);
            var itensGrupo = this.SelecionarItens(template.TemplateGrupos);

            var listaOrcamentoTotal = this.CriarItensOrcamentoTotal(etapas, itensGrupo);
            this.AdicionarOrcamentosTotais(entity, listaOrcamentoTotal);
            this.AdicionarGrupoAprovacao(listaOrcamentoTotal, grupoAproacao);
        }

        private void AdicionarGrupoAprovacao(List<OrcamentoTotal> listaOrcamentoTotal, GrupoAprovacao grupoAproacao)
        {
            listaOrcamentoTotal.AsParallel().ForAll(x => x.GrupoAprovacao = grupoAproacao);
        }

        private void AdicionarOrcamentosTotais(ProjetoOrcamento entity, List<OrcamentoTotal> listaOrcamentoTotal)
        {
            listaOrcamentoTotal.AsParallel().ForAll(x => entity.OrcamentoTotal.Add(x));
        }

        private List<OrcamentoTotal> CriarItensOrcamentoTotal(List<ProjetoOrcamentoEtapa> etapas, List<TemplateItem> itensGrupo)
        {
            var itensOrcamentoTotal = new List<OrcamentoTotal>();
            etapas.AsParallel().ForAll(e => itensGrupo.AsParallel().ForAll(i => itensOrcamentoTotal.Add(this.CriarOrcamentoTotal(e, i))));
            return itensOrcamentoTotal;
        }

        private OrcamentoTotal CriarOrcamentoTotal(ProjetoOrcamentoEtapa etapa, TemplateItem item)
        {
            var orcamentoTotal = new OrcamentoTotal()
            {
                NomeFase = etapa.Fase.NomeFase,
                NomeEtapa = etapa.NomeEtapa,
                CodGrupo = item.TemplateGrupo.Codigo,
                NomeGrupo = item.TemplateGrupo.Nome,
                CodItem = item.Codigo,
                NomeItem = item.Nome,
                IdItemPlanoContas = item.IdItemPlanoContas,
                TxtFormulaCalculo = item.Formula
            };
            var grupoAprovacaoService = this.GetEnityServiceInstance<OrcamentoTotal>();
            grupoAprovacaoService.SaveEntity(orcamentoTotal);
            return orcamentoTotal;
        }

        private List<TemplateItem> SelecionarItens(ICollection<TemplateGrupo> grupos)
        {
            var itens = (from g in grupos
                         from i in g.TemplateOrcamentoItems
                         select i).ToList();
            return itens;
        }
        private Template GetTemplate(long idtemplate)
        {
            var projetoBasicService = this.GetEnityServiceInstance<Template>();
            return projetoBasicService.GetBy(idtemplate);
        }                       
    }
}
