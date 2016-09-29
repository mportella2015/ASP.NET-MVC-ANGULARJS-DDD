using Conspiracao.Domain.Models;
using Conspiracao.Domain.Models.Enum;
using Conspiracao.Framework;
using Conspiracao.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conspiracao.Service.Implementations
{
    public partial class OrcamentoService : EntityService<ProjetoOrcamento>, IOrcamentoService
    {
        public OrcamentoService(
            IUnitOfWork unitOfWork,
            IGenericRepository<ProjetoOrcamento> repository)
            : base(unitOfWork, repository)
        {

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
                                            orcamento.DataInicioOrcamento,
                                            orcamento.DataFimOrcamento,
                                            orcamento.Escopo,
                                            orcamento.StatusOrcamento.Texto,
                                            orcamento.DataAprovacao,
                                            orcamento.ValorTotalOrcamento)
                            };
                        orcamentoViewer.Add(d);
                    });
            }
            return orcamentoViewer;
        }

        public override void SaveEntity(ProjetoOrcamento entity)
        {
            this.VerificarRegraOrcamentoTemplate(entity);
            this.VerificarFases(entity);
            this.VerificarEtapas(entity.Fases);                        
            base.SaveEntity(entity);
        }

        private void VerificarEtapas(ICollection<ProjetoOrcamentoFase> fases)
        {
            if (fases == null || fases.Count == 0)
                throw new CustomServiceException("Ao menos uma Fase de estar cadastrada!");
            foreach (var fase in fases.Where(x => x.Etapas == null || x.Etapas.Count == 0))
                fase.Etapas = new List<ProjetoOrcamentoEtapa>() { new ProjetoOrcamentoEtapa() { NomeEtapa = "ÚNICA" } };
        }

        private void VerificarFases(ProjetoOrcamento entity)
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

        private void VerificarRegraOrcamentoTemplate(ProjetoOrcamento entity)
        {
            //TODO: Verificar Regras relacionadas a template e Orçamento. Deve Lançar uma exeção                        
        }

        private void AttachEntitys(ProjetoOrcamento entity)
        {
            this.AttachAny(entity.Midias);
            this.AttachAny(entity.Territorios);
        }

        public override ProjetoOrcamento Save(ProjetoOrcamento entity)
        {
            this.SaveEntity(entity);
            if (entity.IdTemplateOrcamento != default(long))
                this.CriarOrcamentoBaseadoTemplate(entity);
            this.AttachEntitys(entity);
            this._unitOfWork.Commit();
            return entity;
        }
    }
}
