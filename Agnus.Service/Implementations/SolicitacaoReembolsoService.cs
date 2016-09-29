using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System.Linq;

namespace Agnus.Service.Implementations
{
    public class SolicitacaoReembolsoService : EntityService<SolicitacaoReembolso>, ISolicitacaoReembolsoService
    {
        public SolicitacaoReembolsoService(IUnitOfWork unitOfWork, IGenericRepository<SolicitacaoReembolso> repository)
            : base(unitOfWork, repository)
        {
        }

        public string GetIncentivado(SolicitacaoReembolso entity)
        {
            if (entity.Projeto != null)
            {
                var projRegistro = entity.Projeto.ProjetoRegistro.Any(x => x.DataAprovacao != null);
                var Incentivado = projRegistro ? "Sim" : "Não";
                return Incentivado;
            }

            return "N/A";
        }

        public decimal GetValorTotalEmAprovacao(SolicitacaoReembolso entity)
        {
            if (entity.SolicitacaoReembolsoIPC.Count != 0)
            {
                var valorTotalEmApovacao = entity.SolicitacaoReembolsoIPC
                    .Where(x => x.StatusItemReembolso.IdTabelaGenericaDominio == (int)DominioGenericoEnum.StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso
                        && x.StatusItemReembolso.Codigo == (int)StatusItemReembolsoEnum.EmAprovacao).Select(x => x.ValorDocumento).Sum();
                return valorTotalEmApovacao;
            }
            return 0m;
        }

        public decimal GetValorTotalAprovada(SolicitacaoReembolso entity)
        {
            if (entity.SolicitacaoReembolsoIPC.Count != 0)
            {
                var valorTotalAprovada = entity.SolicitacaoReembolsoIPC
                    .Where(x => x.StatusItemReembolso.IdTabelaGenericaDominio == (int)DominioGenericoEnum.StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso
                        && x.StatusItemReembolso.Codigo == (int)StatusItemReembolsoEnum.Aprovada).Select(x => x.ValorDocumento).Sum();
                return valorTotalAprovada;
            }
            return 0m;
        }

        public decimal GetValorTotalReprovada(SolicitacaoReembolso entity)
        {
            if (entity.SolicitacaoReembolsoIPC.Count != 0)
            {
                var valorTotalReprovada = entity.SolicitacaoReembolsoIPC
                    .Where(x => x.StatusItemReembolso.IdTabelaGenericaDominio == (int)DominioGenericoEnum.StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso
                        && x.StatusItemReembolso.Codigo == (int)StatusItemReembolsoEnum.Reprovada).Select(x => x.ValorDocumento).Sum();
                return valorTotalReprovada;
            }
            return 0m;
        }

        public decimal GetValorTotalEmRevisao(SolicitacaoReembolso entity)
        {
            if (entity.SolicitacaoReembolsoIPC.Count != 0)
            {
                var valorTotalEmRevisao = entity.SolicitacaoReembolsoIPC
                    .Where(x => x.StatusItemReembolso.IdTabelaGenericaDominio == (int)DominioGenericoEnum.StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso
                        && x.StatusItemReembolso.Codigo == (int)StatusItemReembolsoEnum.EmRevisao).Select(x => x.ValorDocumento).Sum();
                return valorTotalEmRevisao;
            }
            return 0m;
        }

        public decimal GetValorTotalCancelada(SolicitacaoReembolso entity)
        {
            if (entity.SolicitacaoReembolsoIPC.Count != 0)
            {
                var valorTotalCancelada = entity.SolicitacaoReembolsoIPC
                    .Where(x => x.StatusItemReembolso.IdTabelaGenericaDominio == (int)DominioGenericoEnum.StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso
                        && x.StatusItemReembolso.Codigo == (int)StatusItemReembolsoEnum.Cancelada).Select(x => x.ValorDocumento).Sum();
                return valorTotalCancelada;
            }
            return 0m;
        }

        public decimal GetValorTotalRegistrada(SolicitacaoReembolso entity)
        {
            if (entity.SolicitacaoReembolsoIPC.Count != 0)
            {
                var valorTotalRegistrada = entity.SolicitacaoReembolsoIPC
                    .Where(x => x.StatusItemReembolso.IdTabelaGenericaDominio == (int)DominioGenericoEnum.StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso
                        && x.StatusItemReembolso.Codigo == (int)StatusItemReembolsoEnum.Registrada).Select(x => x.ValorDocumento).Sum();
                return valorTotalRegistrada;
            }
            return 0m;
        }

        public decimal GetValorTotalReembolso(SolicitacaoReembolso entity)
        {
            if (entity.SolicitacaoReembolsoIPC.Count != 0)
            {
                var valorTotalReembolso = entity.SolicitacaoReembolsoIPC
                    .Where(x => x.StatusItemReembolso.IdTabelaGenericaDominio == (int)DominioGenericoEnum.StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso
                    && x.StatusItemReembolso.Codigo != (int)StatusItemReembolsoEnum.Cancelada
                    && x.StatusItemReembolso.Codigo != (int)StatusItemReembolsoEnum.Reprovada)
                    .Select(x => x.ValorDocumento).Sum();
                return valorTotalReembolso;
            }

            return 0m;
        }

        public int CriarNovoNumSolicitacao()
        {
            //return GetAll().Count() + 1;
            var ultimo = GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            if (ultimo != null)
                return (int)ultimo.Id + 1;
            else
                return 1;
        }
    }
}
