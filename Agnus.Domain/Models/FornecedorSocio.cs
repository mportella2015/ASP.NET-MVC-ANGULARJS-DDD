namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorSocio")]
    [DataContract]
    public partial class FornecedorSocio :KeyAuditableEntity, ILogFornecedor
    {
        [ForeignKey("TipoRelacao")]
        [DataMember]
        public long? IdTipoRelacao { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio TipoRelacao { get; set; }

        [ForeignKey("PessoaJuridica")]
        [DataMember]
        public long? IdPessoaJuridica { get; set; }

        [DataMember]
        public virtual Fornecedor PessoaJuridica { get; set; }

        [ForeignKey("PessoaFisica")]
        [DataMember]
        public long? IdPessoaFisica { get; set; }

        [DataMember]
        public virtual Fornecedor PessoaFisica { get; set; }

        [Required]
        [StringLength(120)]
        [DataMember]
        public string NomeFornecedor { get; set; }

        [StringLength(18)]
        [DataMember]
        public string CPFCNPJ { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NumRG { get; set; }

        [DataMember]
        public DateTime? DataEmissaoRG { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NomeOrgaoRG { get; set; }

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

        [ForeignKey("Pais")]
        [DataMember]
        public long? IdPais { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio Pais { get; set; }

        [ForeignKey("Nacionalidade")]
        [DataMember]
        public long? IdNacionalidade { get; set; }

        public virtual ConteudoTabelaDominio Nacionalidade { get; set; }

        [DataMember]
        public bool? IsSocioRepresentanteLegal { get; set; }


        [NotMapped]
        public string NomeEntidade
        {
            get { return "Sócio"; }
        }

        [NotMapped]
        public long IdFornecedor
        {
            get { return IdPessoaJuridica.HasValue ? IdPessoaJuridica.Value : 0; }
        }

        [NotMapped]
        public string txtTipoRelacao 
        {
            get { return IsSocioRepresentanteLegal.HasValue && IsSocioRepresentanteLegal.Value ? string.Format("{0}/{1}", TipoRelacao == null ? string.Empty : TipoRelacao.Texto, "Representante legal") : TipoRelacao == null ? string.Empty : TipoRelacao.Texto; }
        }
    }
}
