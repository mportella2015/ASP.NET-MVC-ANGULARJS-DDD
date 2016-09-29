using System.Runtime.Serialization;

namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FornecedorTelefone")]
    [DataContract]
    public partial class FornecedorTelefone : KeyAuditableEntity, ILogFornecedor
    {
        [Required]
        [StringLength(21)]
        [DataMember]
        public string TxtTelefone { get; set; }
        
        [DataMember]
        public long IdTipoUsoTelefone { get; set; }

        public virtual ConteudoTabelaDominio TipoUsoTelefone { get; set; }

        [DataMember]
        public long IdTipoTelefone { get; set; }

        public virtual ConteudoTabelaDominio TipoTelefone { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Telefone"; }
        }
    }
}
