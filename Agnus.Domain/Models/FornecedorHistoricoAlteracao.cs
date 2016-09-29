namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FornecedorHistoricoAlteracao")]
    public partial class FornecedorHistoricoAlteracao : KeyAuditableEntity
    {
        public DateTime DataAlteracao { get; set; }

        [Column(TypeName = "text")]
        public string XML { get; set; }

        [Required]
        [StringLength(100)]
        public string LoginUsuarioResponsavel { get; set; }
        
        [Required]
        [StringLength(1)]
        public string CodigoOperacaoCadastral { get; set; }

        public string NomeEntidade { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [NotMapped]
        public string DescricaoOperacaoCadastral
        {
            get
            {
                switch (CodigoOperacaoCadastral)
                {
                    case "I":
                        return "Inclusão";
                    case "U":
                        return "Alteração";
                    case "D":
                        return "Exclusão";
                }
                return string.Empty;
            }
        }
    }
}
