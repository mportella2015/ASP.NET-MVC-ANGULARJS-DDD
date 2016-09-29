namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("Empresa")]
    [DataContract]
    public partial class Empresa : KeyAuditableEntity
    {
        public Empresa()
        {
        }

        public string RazaoSocial { get; set; }

        public string CNPJ { get; set; }

        public string IE { get; set; }

        public string IM { get; set; }

        public int Status { get; set; }

        [Required]
        [StringLength(150)]        
        public string TxtEndereco { get; set; }

        [StringLength(50)]        
        public string TxtComplemento { get; set; }

        [Required]
        [StringLength(8)]        
        public string NumEndereco { get; set; }

        [Required]
        [StringLength(50)]        
        public string Bairro { get; set; }

        [Required]
        [StringLength(50)]        
        public string Cidade { get; set; }

        [StringLength(2)]        
        public string UF { get; set; }

        [Required]
        [StringLength(9)]        
        public string CEP { get; set; }
        
        [ForeignKey("Pais")]        
        public long? IdPais { get; set; }
        public virtual ConteudoTabelaDominio Pais { get; set; }

        [ForeignKey("Pessoa")]
        public long IdPessoa { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        public long? Conspiraware { get; set; }
    }
}
