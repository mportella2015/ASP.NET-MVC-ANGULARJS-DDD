//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Agnus.Domain.Models
{
    using Agnus.Domain;
    using Agnus.Domain.Models;
    using Agnus.Domain.Models.Enum;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [Table("PedidoCompraItem")]
    public partial class PedidoCompraItem : KeyAuditableEntity, IWorkFlowEntity
    {
        public PedidoCompraItem()
        {
            this.WorkFlowEntity = new HashSet<WorkFlowEntity>();
        }

        public int? NumBoleta { get; set; }
        public DateTime? DataBoleta { get; set; }

        [DataMember]
        public int NumItem { get; set; }

        [DataMember]
        public string TxtComplemento { get; set; }

        [DataMember]
        public int QtdPedidoCompra { get; set; }

        [DataMember]
        public decimal ValorUnitario { get; set; }

        [DataMember]
        public decimal ValorTotal { get; set; }

        [DataMember]
        public decimal ValorDesconto { get; set; }

        [DataMember]
        public decimal? PercentualDesconto { get; set; }

        [DataMember]
        public DateTime DatStatusItemPedidoCompra { get; set; }

        [DataMember]
        public bool IndPrevistoOrcamento { get; set; }

        [DataMember]
        public string TxtLocalEntrega { get; set; }

        [DataMember]
        public DateTime? DataEntrega { get; set; }

        [DataMember]
        public TimeSpan? HorarioEntrega { get; set; }

        [DataMember]
        public string TxtCondicoesEspeciais { get; set; }

        [DataMember]
        public DateTime? DataInicioServico { get; set; }

        [DataMember]
        public DateTime? DataFimServico { get; set; }

        [ForeignKey("Realizado")]
        [DataMember]
        public long? IdRealizado { get; set; }
        public virtual Realizado Realizado { get; set; }

        [DataMember]
        public long? IdOrcamentoTotal { get; set; }

        [ForeignKey("Fornecedor")]
        [DataMember]
        public long? IdFornecedor { get; set; }

        [DataMember]
        public virtual Fornecedor Fornecedor { get; set; }
        
        [ForeignKey("FornecedorSocio")]
        [DataMember]
        public long? IdFornecedorSocio { get; set; }

        [DataMember]
        public virtual FornecedorSocio FornecedorSocio { get; set; }

        [ForeignKey("PedidoCompra")]
        [DataMember]
        public long? IdPedidoCompra { get; set; }

        [DataMember]
        public virtual PedidoCompra PedidoCompra { get; set; }

        [ForeignKey("TipoMaterial")]
        [DataMember]
        public long? IdTipoMaterial { get; set; }
        public virtual TipoMaterial TipoMaterial { get; set; }

        [ForeignKey("StatusItemPedidoCompra")]
        [DataMember]
        public long? IdStatusItemPedidoCompra { get; set; }
        public virtual ConteudoTabelaDominio StatusItemPedidoCompra { get; set; }

        [ForeignKey("FormaPagamento")]
        [DataMember]
        public long? IdFormaPagamento { get; set; }
        public virtual ConteudoTabelaDominio FormaPagamento { get; set; }

        [ForeignKey("PrazoPagamento")]
        [DataMember]
        public long? IdPrazoPagamento { get; set; }
        public virtual ConteudoTabelaDominio PrazoPagamento { get; set; }

        [ForeignKey("UnidadeMedida")]
        [DataMember]
        public long? IdUnidadeMedida { get; set; }
        public virtual ConteudoTabelaDominio UnidadeMedida { get; set; }

        [ForeignKey("TipoServico")]
        [DataMember]
        public long? IdTipoServico { get; set; }
        public virtual TipoServico TipoServico { get; set; }

        [ForeignKey("ItemPlanoContas")]
        [DataMember]
        public long? IdItemPlanoContas { get; set; }
        public virtual ItemPlanoContas ItemPlanoContas { get; set; }
        //public virtual ItemPlanoContas ItemPlanoContas { get; set; }

        [ForeignKey("ItemOrcamento")]
        [DataMember]
        public long? IdItemOrcamento { get; set; }
        public virtual ItemOrcamento ItemOrcamento { get; set; }

        public virtual ICollection<WorkFlowEntity> WorkFlowEntity { get; set; }

        //public string NumeroPedidoFornecedor { get; set; }

        [DataMember]
        public long NumeroOrdemCompraFornecedor { get; set; }

        public bool IsRealizado { get { return this.Realizado != null; } }

        public long IdEntity
        {
            get { return this.Id; }
        }

        public ItemOrcamento ItemOrcamentoEntity
        {
            get { return this.ItemOrcamento; }
        }

        public decimal ValorUsoEntity
        {
            get { return this.ValorTotal; }
        }

        public int CodObjetoUso
        {
            get { return (int)ObjetoComprometimentoUsoEnum.ItemPedido; }
        }

        public void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus)
        {
            this.StatusItemPedidoCompra = StatusItem;
            if (updateDataStatus)
                this.DatStatusItemPedidoCompra = DateTime.Now;
        }

        public void SetStatusPai(ConteudoTabelaDominio StatusPai)
        {
            if (!IsRealizado)
                this.PedidoCompra.StatusPedidoCompra = StatusPai;
            else
                this.Realizado.StatusRealizado = StatusPai;
        }

        public int GetCodStatusItemBy(ParecerAprovacaoEnum parecer)
        {
            switch (parecer)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    return (int)StatusItemPedidoCompraEnum.Aprovado;
                case ParecerAprovacaoEnum.Reprovado:
                    return (int)StatusItemPedidoCompraEnum.Reprovado;
                case ParecerAprovacaoEnum.EmAprovacao:
                    return (int)StatusItemPedidoCompraEnum.EmAprovacao;
                case ParecerAprovacaoEnum.Revisar:
                    return (int)StatusItemPedidoCompraEnum.EmRevisao;
                default:
                    break;
            }
            throw new Exception("N�o foi poss�vel encontrar um status para este item!");
        }

        public int StatusAprovado
        {
            get { return (int)StatusItemPedidoCompraEnum.Aprovado; }
        }

        public int StatusEmAprovacao
        {
            get { return (int)StatusItemPedidoCompraEnum.EmAprovacao; }
        }

        public int StatusDominioPai
        {
            get { return !this.IsRealizado ? (int)DominioGenericoEnum.StatusPedidoCompra : (int)DominioGenericoEnum.StatusRealizado; }
        }

        public int StatusCodPaiFinalizado
        {
            get { return !this.IsRealizado ? (int)StatusPedidoCompraEnum.Finalizado : (int)StatusRealizadoEnum.LiberadoPraPagamento; }
        }

        public int StatusReprovado
        {
            get { return (int)StatusItemPedidoCompraEnum.Reprovado; }
        }


        [NotMapped]
        private decimal _desconto;
        [NotMapped]
        public decimal Desconto
        {
            get
            {
                if (_desconto == default(decimal))
                {
                    if (this.ValorDesconto != default(decimal))
                        _desconto = this.ValorDesconto;
                    else if (this.PercentualDesconto.HasValue && this.PercentualDesconto.Value != 0)
                        _desconto = this.QtdPedidoCompra * this.ValorUnitario * (this.PercentualDesconto.Value / 100);
                }
                return _desconto;
            }
        }

        [NotMapped]
        public string NomeProjetoCentroCusto
        {
            get
            {
                if (this.PedidoCompra.Projeto != null)
                    return this.PedidoCompra.Projeto.NomeEntidade;
                else if (this.PedidoCompra.CentroCusto != null)
                    return this.PedidoCompra.CentroCusto.NomCentroCusto;
                return string.Empty;
            }
        }
    }
}