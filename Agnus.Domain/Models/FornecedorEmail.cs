using System.Runtime.Serialization;

namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FornecedorEmail")]
    [DataContract]
    public partial class FornecedorEmail : KeyAuditableEntity, ILogFornecedor
    {
        [Required]
        [StringLength(150)]
        [DataMember]
        public string TxtEmail { get; set; }

        [DataMember]
        [ForeignKey("TipoEmail")]
        public long IdTipoEmail { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio TipoEmail { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Email"; }
        }
    }
}
