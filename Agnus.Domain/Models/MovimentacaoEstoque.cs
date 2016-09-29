namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MovimentacaoEstoque")]
    public partial class MovimentacaoEstoque : KeyAuditableEntity
    {
        
        public int QtdMovimentacaoEstoque { get; set; }
        public DateTime DataMovimentacaoEstoque { get; set; }

        [ForeignKey("MovimentacaoEstoqueCartao")]
        public long? IdMovimentacaoEstoqueCartao { get; set; }
        public virtual ConteudoTabelaDominio MovimentacaoEstoqueCartao { get; set; }

        [ForeignKey("EstoqueCartao")]
        public long IdEstoqueCartao { get; set; }
        public virtual EstoqueCartao EstoqueCartao { get; set; }
    }
}

