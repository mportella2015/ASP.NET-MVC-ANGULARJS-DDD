namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorContato")]
    [DataContract]
    public partial class FornecedorContato : KeyAuditableEntity, ILogFornecedor
    {
        [Required]
        [StringLength(100)]
        [DataMember]
        public string NomeContato { get; set; }

        [DataMember]
        public bool? IndResponsavelCadastro { get; set; }

        [StringLength(50)]
        [DataMember]
        public string Setor { get; set; }

        [StringLength(50)]
        [DataMember]
        public string Cargo { get; set; }

        [StringLength(21)]
        [DataMember]
        public string TxtTelefone { get; set; }

        [DataMember]
        public int? Ramal { get; set; }

        [StringLength(21)]
        public string TxtCelularCorporativo { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Contato"; }
        }
    }
}
