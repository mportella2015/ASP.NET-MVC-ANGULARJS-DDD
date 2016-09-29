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
    public class PedidoCompraService : EntityService<PedidoCompra>, IPedidoCompraService
    {
        private IEntityService<PedidoCompraItem> _pedidoCompraItemService = DependencyResolver.Current.GetService<IEntityService<PedidoCompraItem>>();
        private IEntityService<ConteudoTabelaDominio> _conteudoTabelaService = DependencyResolver.Current.GetService<IEntityService<ConteudoTabelaDominio>>();
        private ITabelaGenericaDominioService _tabelaGenericaService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();

        public PedidoCompraService(IUnitOfWork unitOfWork, IGenericRepository<PedidoCompra> repository)
            : base(unitOfWork, repository)
        {
        }

        public void AtualizarStatus(long? idPedido, int codStatus = 0)
        {
            if (!idPedido.HasValue || idPedido.Value == 0)
                return;

            var pedidoCompra = this.GetBy(idPedido.Value);
            var itensPedido = _pedidoCompraItemService.GetAllByFilter(x => x.IdPedidoCompra != 0 && x.IdPedidoCompra == idPedido);

            if (itensPedido.Any(x => x.IdStatusItemPedidoCompra != _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.StatusItemPedidoCompra, (int)StatusItemPedidoCompraEnum.Cancelado).Id))
                return;
            else
                pedidoCompra.IdStatusPedidoCompra =_tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.StatusPedidoCompra, (int)StatusPedidoCompraEnum.Cancelado).Id;

            _repository.Edit(pedidoCompra);
            _unitOfWork.Commit();
        }

        public long CriarNovoCodPedido()
        {
            var lastCode = this.GetAll();
            return lastCode.Any() ? lastCode.Max(x => x.CodPedido) + 1 : 1;
        }

        public override void SaveEntity(PedidoCompra entity)
        {
            entity.CodPedido = this.CriarNovoCodPedido();
            base.SaveEntity(entity);
        }
    }
}
