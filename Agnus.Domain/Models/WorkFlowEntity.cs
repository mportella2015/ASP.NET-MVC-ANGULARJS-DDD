using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Agnus.Domain.Models
{
    [Table("WorkFlowEntity")]
    public class WorkFlowEntity : KeyAuditableEntity
    {
        public WorkFlowEntity()
        {
            this.WorkFlows = new HashSet<WorkFlow>();
        }

        [ForeignKey("PedidoCompraItem")]
        public long? IdPedidoCompraItem { get; set; }
        public virtual PedidoCompraItem PedidoCompraItem { get; set; }

        [ForeignKey("ProjetoFundo")]
        public long? IdProjetoFundo { get; set; }
        public virtual ProjetoFundo ProjetoFundo { get; set; }

        [ForeignKey("SolicitacaoAdiantamento")]
        public long? IdSolicitacaoAdiantamento { get; set; }
        public virtual SolicitacaoAdiantamento SolicitacaoAdiantamento { get; set; }

        [ForeignKey("Realizado")]
        public long? IdRealizado { get; set; }
        public virtual Realizado Realizado { get; set; }

        [ForeignKey("SolicitacaoReembolsoIPC")]
        public long? IdSolicitacaoReembolsoIPC { get; set; }
        public virtual SolicitacaoReembolsoIPC SolicitacaoReembolsoIPC { get; set; }

        [ForeignKey("SolicitacaoCartao")]
        public long? IdSolicitacaoCartao { get; set; }
        public virtual SolicitacaoCartao SolicitacaoCartao { get; set; }

        [ForeignKey("GrupoAprovacao")]
        public long? IdGrupoAprovacao { get; set; }
        public virtual GrupoAprovacao GrupoAprovacao { get; set; }

        [ForeignKey("PrestacaoContasDetalhe")]
        public long? PrestacaoContasDetalhe_Id { get; set; }
        public virtual PrestacaoContasDetalhe PrestacaoContasDetalhe { get; set; }

        public virtual ICollection<WorkFlow> WorkFlows { get; set; }

        public void SetEntity(IWorkFlowEntity entity)
        {
            if (entity.GetType().BaseType.Name == "PedidoCompraItem" || entity.GetType().Name == "PedidoCompraItem")
                IdPedidoCompraItem = entity.IdEntity;

            if (entity.GetType().BaseType.Name == "ProjetoFundo" || entity.GetType().Name == "ProjetoFundo")
                IdProjetoFundo = entity.IdEntity;

            if (entity.GetType().BaseType.Name == "SolicitacaoAdiantamento" || entity.GetType().Name == "SolicitacaoAdiantamento")
                IdSolicitacaoAdiantamento = entity.IdEntity;

            if (entity.GetType().BaseType.Name == "Realizado" || entity.GetType().Name == "Realizado")
                IdRealizado = entity.IdEntity;

            if (entity.GetType().BaseType.Name == "SolicitacaoReembolsoIPC" || entity.GetType().Name == "SolicitacaoReembolsoIPC")
                IdSolicitacaoReembolsoIPC = entity.IdEntity;

            if (entity.GetType().BaseType.Name == "SolicitacaoCartao" || entity.GetType().Name == "SolicitacaoCartao")
                IdSolicitacaoCartao = entity.IdEntity;

            if (entity.GetType().BaseType.Name == "GrupoAprovacao" || entity.GetType().Name == "GrupoAprovacao")
                IdGrupoAprovacao = entity.IdEntity;

            if (entity.GetType().BaseType.Name == "PrestacaoContasDetalhe" || entity.GetType().Name == "PrestacaoContasDetalhe")
                PrestacaoContasDetalhe_Id = entity.IdEntity;
        }

        public IWorkFlowEntity GetEntity()
        {
            if (this.PedidoCompraItem != null)
                return PedidoCompraItem;

            if (this.ProjetoFundo != null)
                return this.ProjetoFundo;

            if (this.SolicitacaoAdiantamento != null)
                return this.SolicitacaoAdiantamento;

            if (this.Realizado != null)
                return this.Realizado;

            if (this.SolicitacaoReembolsoIPC != null)
                return this.SolicitacaoReembolsoIPC;

            if (this.SolicitacaoCartao != null)
                return this.SolicitacaoCartao;

            if (this.GrupoAprovacao != null)
                return this.GrupoAprovacao;

            if (this.PrestacaoContasDetalhe != null)
                return this.PrestacaoContasDetalhe;

            return null;
        }
    }
}
