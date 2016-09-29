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
    public abstract class OrcamentoManager
    {
        protected OrcamentoService _OrcamentoService { get; private set; }
        ITabelaGenericaDominioService _tabelaGenericaService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();
        protected IEntityService<OrcamentoTotal> _GrupoAprovacaoService { get; private set; }

        protected string _codigoGrupoParteEmpresa { get; private set; }

        public OrcamentoManager(OrcamentoService orcamentoService)
        {
            _OrcamentoService = orcamentoService;
            this._GrupoAprovacaoService = DependencyResolver.Current.GetService < IEntityService<OrcamentoTotal>>();
            //TODO:Pegar esta informação do Web.Config
            _codigoGrupoParteEmpresa = string.Empty;
        }

        protected GrupoAprovacao CriarGrupoAprovacao()
        {
            var statusOrcamento = _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.StatusOrcamento, (int)StatusOrcamentoEnum.PreOrcamento);
            var tipoOrcamento = _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.TipoOrcamento, (int)TipoOrcamentoEnum.Original);
            if (statusOrcamento == null)
                throw new CustomServiceException("Status do Orçamento não encontrado!");
            if (tipoOrcamento == null)
                throw new CustomServiceException("Tipo de Orçamentonão encontrado!");
            var now = DateTime.Now;
            var grupoAprovacao = new GrupoAprovacao()
            {
                DataCadastro = now,
                IdStatusOrcamento = statusOrcamento.Id,
                DataStatusOrcamento = now,
                IdTipoOrcamento = tipoOrcamento.Id,
                ValorOrcamentoAprovar = 0
            };
            var grupoAprovacaoService = _OrcamentoService.GetEnityServiceInstance<GrupoAprovacao>();
            grupoAprovacaoService.SaveEntity(grupoAprovacao);
            return grupoAprovacao;
        }

        protected List<ProjetoOrcamentoEtapa> SelecionarEtapas(ICollection<ProjetoOrcamentoFase> fases)
        {
            var etapasOrcamento = (from f in fases
                                   from e in f.Etapas
                                   select e).ToList();
            return etapasOrcamento;
        }

        

        protected void AdicionarGrupoAprovacao(List<OrcamentoTotal> listaOrcamentoTotal, GrupoAprovacao grupoAproacao)
        {
            listaOrcamentoTotal.AsParallel().ForAll(x => x.GrupoAprovacao = grupoAproacao);
        }

        protected void AdicionarOrcamentosTotais(ProjetoOrcamento entity, List<OrcamentoTotal> listaOrcamentoTotal)
        {
            listaOrcamentoTotal.ForEach(x => entity.OrcamentoTotal.Add(x));
        }

        protected void JuntarOrcamentosParteEmpresa(List<OrcamentoTotal> listaOrcamentoTotal)
        {
            var orcamentosParteEmpresa = _GrupoAprovacaoService.GetAllByFilter(x => x.CodGrupo == _codigoGrupoParteEmpresa);
            if (orcamentosParteEmpresa != null && orcamentosParteEmpresa.Count() > 0)
                listaOrcamentoTotal.AddRange(orcamentosParteEmpresa);            
        }
        
        public abstract void CriarOrcamento(ProjetoOrcamento orcamento);        


    }
}
