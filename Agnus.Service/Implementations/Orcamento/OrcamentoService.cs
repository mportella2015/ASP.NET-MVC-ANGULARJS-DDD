using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Framework.Helper;
using Agnus.Service.DTO;
using Agnus.Service.DTO.Orcamento;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    public partial class OrcamentoService : EntityService<ProjetoOrcamento>, IOrcamentoService
    {
        private IEntityService<OrcamentoTotal> _orcamentoTotalService = DependencyResolver.Current.GetService<IEntityService<OrcamentoTotal>>();
        OrcamentoServiceManager _manager;

        public OrcamentoService(
            IUnitOfWork unitOfWork,
            IGenericRepository<ProjetoOrcamento> repository)
            : base(unitOfWork, repository)
        {
            _manager = new OrcamentoServiceManager(this);
        }

        public IEnumerable<ProjetoOrcamento> GetAllBut(ProjetoOrcamento orcamento)
        {
            if (orcamento == null || orcamento.Id == default(long))
                return this.GetAll();
            return this.GetAllByFilter(x => x.Id != orcamento.Id);
        }

        public IEnumerable<ProjetoOrcamento> GetOrcamentosNucleo(Nucleo nucleo)
        {
            if (nucleo == null || nucleo.Id == default(long))
                throw new Exception("O Núcleo indicado inválido!");
            return this.GetAllByFilter(x => x.Projeto.IdNucleo == nucleo.Id);
        }

        public IEnumerable<dynamic> BuildOrcamentoViewer(IEnumerable<ProjetoOrcamento> orcamentos)
        {
            return _manager.BuildOrcamentoViewer(orcamentos);
        }

        public override ProjetoOrcamento Save(ProjetoOrcamento entity)
        {
            if (_manager.ProjetoPossuiOrcamentoAprovado(entity.IdProjeto))
                throw new CustomServiceException("Projeto já possui um orçamento aprovado.");
            _manager.VerificarRegraOrcamentoTemplate(entity);
            _manager.SalvarNovaVersao(entity);
            OrcamentoManager manager = _manager.CriarIntanciaManager(entity);
            if (manager != null)
                manager.CriarOrcamento(entity);
            this.Commit();
            return entity;
        }

        internal void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IEnumerable<OrcamentoTotal> SelecionarTodosItensOrcamentoTotal(long idOrcamento)
        {
            var orcamentoTotalService = this.GetEnityServiceInstance<OrcamentoTotal>();
            return orcamentoTotalService.GetAllByFilter(x => x.IdProjetoOrcamento == idOrcamento);
        }

        public bool VerificarRegraPersistencia(ProjetoOrcamento orcamento)
        {
            var statusOrcamentoPermitidos = new List<int>() { (int)StatusOrcamentoEnum.PreOrcamento };
            return orcamento.Projeto.StatusProjeto.Codigo == (int)StatusProjetoEnum.Registrado && statusOrcamentoPermitidos.Contains(orcamento.StatusOrcamento.Codigo);
        }

        public object AtualizarDados(ProjetoOrcamento orcamento, IList<LogAtualizacaoDTO> itensAtualizacao)
        {
            this.ExecutarAcaoSobreItem(itensAtualizacao, orcamento, _manager.AtualizarDadosOrcamentoTotal);
            var listaAdjacenciaManager = new ListaAdjacenciaManager();
            var lista = listaAdjacenciaManager.CriarListaAdjacencia(orcamento._OrcamentoTotalSemAlteracao.ToList());

            _manager.ManterFormulas(itensAtualizacao.Where(x => x.CampoFormulaAlterado).ToList(), lista, orcamento);
            return _manager.RecuperarIdContasDetalheAdicionado(itensAtualizacao.ToList(), listaAdjacenciaManager, lista);
        }


        public string MontarEscopoMergeVersoes(IEnumerable<ProjetoOrcamento> versoes)
        {
            var escopos = versoes.Select(z => z.Escopo);
            return string.Join("\n\n", escopos);
        }

        public int[] ObterNiveisPermissaoOrcamento(ProjetoOrcamento orcamento)
        {
            return _manager.ObterNiveisPermissaoOrcamento(orcamento);
        }


        public ProjetoOrcamento EditarOrcamento(ProjetoOrcamento orcamento)
        {
            var orcamentoEditado = _repository.Edit(orcamento);
            this.Commit();
            return orcamento;
        }


        public void EnviarAprovacao(ProjetoOrcamento orcamento)
        {
            var fluxoOrcamentoSevice = new FluxoOrcamentoService(this);
            fluxoOrcamentoSevice.VerificarConsistencia(orcamento);
            fluxoOrcamentoSevice.SubmeterOrcamentoAprovacao(orcamento);
        }

        public void EnviarAprovacaoOrcamentoManutencao(ProjetoOrcamento orcamento)
        {            
            var fluxoOrcamentoSevice = new FluxoOrcamentoService(this);            
            fluxoOrcamentoSevice.SubmeterAprovacaoOrcamentoManutencao(orcamento);
        }

        public ListaAdjacenciaDTO GerarGrafoOrcamento(ProjetoOrcamento orcamento)
        {
            return _manager.GerarGrafoOrcamento(orcamento, orcamento._OrcamentoTotalSemAlteracao.ToList());
        }

        public ListaAdjacenciaDTO GerarGrafoOrcamentoPorData(ProjetoOrcamento orcamento)
        {
            var manager = new ListaAdjacenciaManager();
            return _manager.GerarGrafoOrcamento(orcamento, orcamento._OrcamentoTotalSemAlteracao.ToList(), manager.BindPorPeriodo);
        }

        public ListaAdjacenciaDTO GerarGrafoProdutor(ProjetoOrcamento orcamento)
        {
            var manager = new ListaAdjacenciaManager();
            return _manager.GerarGrafoOrcamento(orcamento, orcamento.OrcamentoAtual.ToList(), manager.BindProdutorPorPeriodo);
        }

        public ListaAdjacenciaDTO GerarGrafoAlteracao(ProjetoOrcamento orcamento)
        {
            var manager = new ListaAdjacenciaManager();
            var grafo = _manager.GerarGrafoOrcamento(orcamento, orcamento._OrcamentoTotalAlteracoesSumarizadas.ToList(), manager.BindPorPeriodo);
            var alteracoes = orcamento._OrcamentoTotalComAlteracao;
            manager.AdicionarAlteracoes(orcamento,grafo, alteracoes);
            return grafo;
        }

        public void AtivarFluxoAposParecer(ParecerAprovacaoEnum parecerAprovacaoEnum, GrupoAprovacao grupoAprovacao, bool ehUltimoAprovador = false)
        {
            var fluxoOrcamentoSevice = new FluxoOrcamentoService(this);
            fluxoOrcamentoSevice.AtivarFluxo(parecerAprovacaoEnum, grupoAprovacao, ehUltimoAprovador);
        }

        public object ManterDadosOrcamentoPeriodo(ProjetoOrcamento orcamento, IList<LogAtualizacaoDTO> itensAtualizacao)
        {
            this.ExecutarAcaoSobreItem(itensAtualizacao, orcamento, _manager.ManterDadosOrcamentoPeriodo);
            return null;
        }

        public object ManterDadosOrcamentoProdutor(ProjetoOrcamento orcamento, IList<LogAtualizacaoDTO> itensAtualizacao)
        {
            this.ExecutarAcaoSobreItem(itensAtualizacao, orcamento, _manager.ManterDadosOrcamentoProdutor);
            return null;
        }

        public object FinalizarDistribuicaoProdutor(ProjetoOrcamento orcamento)
        {
            _manager.AtualizarStatusProjeto(orcamento, StatusProjetoEnum.Ativo);
            return null;
        }

        private void ExecutarAcaoSobreItem(IList<LogAtualizacaoDTO> itensAtualizacao, ProjetoOrcamento orcamento, Func<ProjetoOrcamento, LogAtualizacaoDTO, object> funcManutencao)
        {
            itensAtualizacao.Where(x => x.Id != default(long)).ToList().ForEach(x => funcManutencao(orcamento, x));
        }

        public object LiberarOrcamentoProdutor(ProjetoOrcamento orcamento)
        {
            if (!_manager.ValidarDistribuicao(orcamento.OrcamentoTotal))
                throw new CustomServiceException("A distribuição de valores no tempo está incorreta.");
            _manager.GerarItensParaProdutor(orcamento);
            _manager.AtualizarStatusProjeto(orcamento, StatusProjetoEnum.Aguardando_Produtor);
            return null;
        }


        public int DefinirNivelAtivaWorkFlow(Nucleo nucleo)
        {
            return nucleo.Codigo == 4 ? 2 : 1;
        }


        public object LiberarDistribuicao(ProjetoOrcamento orcamento)
        {
            _manager.LiberarDistribuicaoTempo(orcamento);
            return null;
        }


        public object ManterDadosOrcamentoAlteracaoReal(ProjetoOrcamento orcamento, IList<LogAtualizacaoDTO> itensAtualizacao)
        {
            if(orcamento._PossuiAprovacaoPendente)
                throw new CustomServiceException("Existe uma solicitação de alteração pendente de aprovação para este orçamento.");
            this.ExecutarAcaoSobreItem(itensAtualizacao, orcamento, _manager.ManterDadosOrcamentoAlteracaoReal);
            this.EditarOrcamento(orcamento);
            return null;
        }

        public void DeleteOrcamentoTotal(long id) 
        {
            try
            {
                var orcamentoTotal = _orcamentoTotalService.GetBy(id);
                _orcamentoTotalService.Delete(orcamentoTotal);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AdicionarNovosItens(List<OrcamentoTotal> orcamentosTotais)
        {
            orcamentosTotais.ForEach(x => _orcamentoTotalService.Save(x));
        }
    }
}
