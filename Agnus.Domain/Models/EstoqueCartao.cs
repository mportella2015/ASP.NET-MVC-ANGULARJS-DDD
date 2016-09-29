namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("EstoqueCartao")]
    public partial class EstoqueCartao : KeyAuditableEntity
    {
        [ForeignKey("ModalidadeCartao")]
        public long IdModalidadeCartao { get; set; }
        public virtual ConteudoTabelaDominio ModalidadeCartao { get; set; }

        [ForeignKey("TipoMovimentacao")]
        public long IdTipoMovimentacao { get; set; }
        public virtual ConteudoTabelaDominio TipoMovimentacao { get; set; }

        public int QtdCartao { get; set; }

        public bool Ativo { get; set; }

        public int QtdPontoRessuprimento { get; set; }

        public int QtdPedido { get; set; }

        public decimal ValorUnitario { get; set; }

        [ForeignKey("Fornecedor")]        
        public long IdFornecedor { get; set; }

        [DataMember]
        public virtual Fornecedor Fornecedor { get; set; }

        [ForeignKey("ItemPlanoContas")]
        public long IdItemPlanoContas { get; set; }

        [DataMember]
        public virtual ItemPlanoContas ItemPlanoContas { get; set; }
    }
}

