namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoDocumento")]
    public partial class TipoDocumento : KeyAuditableEntity
    {
        public TipoDocumento()
        {
            FornecedorDocumentoes = new HashSet<FornecedorDocumento>();
            TipoServicoTipoDocumentoes = new HashSet<TipoServicoTipoDocumento>();
        }

        public int Codigo { get; set; }

        public int? PeriodoValidade { get; set; }

        [Required]
        [StringLength(50)]
        public string TxtTipoDocumento { get; set; }

        [StringLength(50)]
        public string TxtTipoDocumentoIngles { get; set; }

        public bool IndObrigatorioPFEstrangeiro { get; set; }

        public bool IndObrigatorioPF { get; set; }

        public bool IndObrigatorioPJEstarngeiro { get; set; }

        public bool IndObrigatorioPJ { get; set; }

        public virtual ICollection<FornecedorDocumento> FornecedorDocumentoes { get; set; }

        public virtual ICollection<TipoServicoTipoDocumento> TipoServicoTipoDocumentoes { get; set; }

        public bool IndAtivo { get; set; }
    }
}
