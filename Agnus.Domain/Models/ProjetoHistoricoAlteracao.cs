using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agnus.Domain.Models
{
    [Table("ProjetoHistoricoAlteracao")]
    public partial class ProjetoHistoricoAlteracao : KeyAuditableEntity
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

        [ForeignKey("Projeto")]
        public long IdProjeto { get; set; }

        public virtual Projeto Projeto { get; set; }

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
