namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorMaterial")]
    [DataContract]
    public partial class FornecedorMaterial : KeyAuditableEntity, ILogFornecedor
    {
        [Column(TypeName = "decimal")]
        [DataMember]
        public decimal ValorUnitario { get; set; }

        [StringLength(250)]
        [DataMember]
        public string TxtObservacao { get; set; }

        [ForeignKey("PrazoPagamento")]
        [DataMember]
        public long? IdPrazoPagamento { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio PrazoPagamento { get; set; }

        [ForeignKey("UnidadeMedida")]
        [DataMember]
        public long? IdUnidadeMedida { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio UnidadeMedida { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }

        [ForeignKey("TipoMaterial")]
        [DataMember]
        public long IdTipoMaterial { get; set; }

        [DataMember]
        public virtual TipoMaterial TipoMaterial { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Material"; }
        }
    }
}
