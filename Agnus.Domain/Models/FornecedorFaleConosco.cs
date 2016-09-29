namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FornecedorFaleConosco")]
    public partial class FornecedorFaleConosco : KeyAuditableEntity
    {
        public DateTime DataFornecedorFaleConosco { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string TxtFornecedorFaleConosco { get; set; }

        [ForeignKey("Fornecedor")]
        public long? IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }
    }
}
