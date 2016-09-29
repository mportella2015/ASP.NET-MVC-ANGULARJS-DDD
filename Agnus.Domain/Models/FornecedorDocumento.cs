namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorDocumento")]
    [DataContract]
    public partial class FornecedorDocumento : KeyAuditableEntity, ILogFornecedor
    {
        [Required]
        public byte[] Documento { get; set; }

        [DataMember]
        public DateTime? DataVencimento { get; set; }

        [StringLength(300)]
        [DataMember]
        public string NomeArquivo { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [ForeignKey("TipoDocumento")]
        [DataMember]
        public long IdTipoDocumento { get; set; }

        [DataMember]
        public virtual TipoDocumento TipoDocumento { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Documento"; }
        }
    }
}
