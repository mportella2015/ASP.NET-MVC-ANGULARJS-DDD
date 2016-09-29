using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Correios.Net;

namespace Agnus.Service.Implementations
{
    public class RealizadoService : EntityService<Realizado>, IRealizadoService
    {
        public RealizadoService(IUnitOfWork unitOfWork, IGenericRepository<Realizado> repository)
            : base(unitOfWork, repository)
        {
        }

        /// <summary>
        /// Somatório dos itens Realizados
        /// </summary>
        /// <param name="id">IdRealizado</param>
        /// <returns></returns>
        public decimal SomatorioItemRealizado(long id)
        {
            var realizado = GetBy(id);

            if (realizado == null)
                return 0;

            var resultado = realizado.PedidoCompraItens.Sum(x => x.ValorTotal);

            return resultado;
        }

        public decimal SomatorioCentroCusto(List<PedidoCompraItem> centroCusto)
        {
            var resultado = centroCusto.Sum(x => x.ValorTotal);

            return resultado;
        }

        public decimal SomatorioProjeto(List<PedidoCompraItem> projeto)
        {
            var resultado = projeto.Sum(x => x.ValorTotal);

            return resultado;
        }


        /// <summary>
        /// Verifica se o Somatório é abatido na nota Fiscal.
        /// </summary>
        /// <param name="id">IdRealizado</param>
        /// <returns></returns>
        public bool VerificaSubmeterAprovacao(int id)
        {
            var realizado = GetBy(id);

            if (realizado == null)
                return false;

            var somatorioItemRealizado = this.SomatorioItemRealizado(id);
            if (somatorioItemRealizado == 0)
                return false;

            var centroCusto = realizado.PedidoCompraItens.Where(x => x.PedidoCompra.CentroCusto != null).ToList();
            var projeto = realizado.PedidoCompraItens.Where(x => x.PedidoCompra.Projeto != null).ToList();

            var somatorioItemCentroCusto = this.SomatorioCentroCusto(centroCusto);
            var somatorioItemProjeto = this.SomatorioProjeto(projeto);

            if (somatorioItemCentroCusto != 0)
            {
                var retornoTotalCentroCusto = realizado.ValorNotaFiscal - somatorioItemCentroCusto;

                if (retornoTotalCentroCusto <= 0 && somatorioItemProjeto == 0)
                {
                    return true;
                }
            }

            if (somatorioItemProjeto != 0 && somatorioItemCentroCusto == 0)
            {
                var retornoTotalProjeto = realizado.ValorNotaFiscal - somatorioItemProjeto;

                if (retornoTotalProjeto <= 0)
                {
                    return true;
                }
            }
           

            var retornoTotal = realizado.ValorNotaFiscal - somatorioItemRealizado;

            if (retornoTotal == 0 || retornoTotal == realizado.ValorImposto)
            {
                return true;
            }

            return false;
        }

        public int CriarNovoNumRealizado()
        {
            return GetAll().Count() + 1;
        }
    }
}
