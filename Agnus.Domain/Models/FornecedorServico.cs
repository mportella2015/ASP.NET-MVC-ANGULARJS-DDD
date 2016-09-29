namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorServico")]
    [DataContract]
    public partial class FornecedorServico : KeyAuditableEntity, ILogFornecedor
    {
        [Column(TypeName = "numeric")]
        [DataMember]
        public decimal? ValorUnitario { get; set; }

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

        [ForeignKey("TipoServico")]
        [DataMember]
        public long? IdTipoServico { get; set; }

        [DataMember]
        public virtual TipoServico TipoServico { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Serviço"; }
        }
    }
}
