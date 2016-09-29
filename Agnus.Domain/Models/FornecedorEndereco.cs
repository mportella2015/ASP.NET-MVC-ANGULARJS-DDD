namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorEndereco")]
    [DataContract]
    public partial class FornecedorEndereco : KeyAuditableEntity, ILogFornecedor
    {
        [Required]
        [StringLength(150)]
        [DataMember]
        public string TxtEndereco { get; set; }

        [StringLength(50)]
        [DataMember]
        public string TxtComplemento { get; set; }

        [Required]
        [StringLength(8)]
        [DataMember]
        public string NumEndereco { get; set; }

        [Required]
        [StringLength(50)]
        [DataMember]
        public string NomeBairro { get; set; }

        [Required]
        [StringLength(50)]
        [DataMember]
        public string NomeCidade { get; set; }

        [StringLength(2)]
        [DataMember]
        public string CodUF { get; set; }

        [Required]
        [StringLength(9)]
        [DataMember]
        public string CodCEP { get; set; }

        [DataMember]
        public long IdTipoEndereco { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio TipoEndereco { get; set; }

        [ForeignKey("Pais")]
        [DataMember]
        public long? IdPais { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio Pais { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Endereço"; }
        }
    }
}
