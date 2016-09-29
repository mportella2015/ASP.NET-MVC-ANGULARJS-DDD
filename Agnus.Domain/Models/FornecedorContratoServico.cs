namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorContratoServico")]
    public partial class FornecedorContratoServico : KeyAuditableEntity
    {
        [Column(TypeName = "numeric")]
        [DataMember]
        public decimal ValorUnitario { get; set; }

        [ForeignKey("PrazoPagamento")]
        [DataMember]
        public long? IdPrazoPagamento { get; set; }
        public virtual ConteudoTabelaDominio PrazoPagamento { get; set; }

        [ForeignKey("UnidadeMedida")]
        [DataMember]
        public long? IdUnidadeMedida { get; set; }
        public virtual ConteudoTabelaDominio UnidadeMedida { get; set; }

        [ForeignKey("FornecedorContrato")]
        [DataMember]
        public long IdFornecedorContrato { get; set; }
        public virtual FornecedorContrato FornecedorContrato { get; set; }

        [ForeignKey("TipoServico")]
        [DataMember]
        public long IdTipoServico { get; set; }
        public virtual TipoServico TipoServico { get; set; }
    }
}
