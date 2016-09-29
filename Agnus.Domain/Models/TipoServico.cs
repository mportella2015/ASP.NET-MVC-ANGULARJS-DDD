namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoServico")]
    public partial class TipoServico : KeyAuditableEntity
    {
        public TipoServico()
        {
            FornecedorContratoServicoes = new HashSet<FornecedorContratoServico>();
            FornecedorServicoes = new HashSet<FornecedorServico>();
            TipoServicoTipoDocumentoes = new HashSet<TipoServicoTipoDocumento>();
        }

        [Required]
        [StringLength(100)]
        public string TxtTipoServico { get; set; }

        [StringLength(100)]
        public string TxtTipoServicoIngles { get; set; }

        public virtual ICollection<FornecedorContratoServico> FornecedorContratoServicoes { get; set; }

        public virtual ICollection<FornecedorServico> FornecedorServicoes { get; set; }
        
        public virtual ICollection<TipoServicoTipoDocumento> TipoServicoTipoDocumentoes { get; set; }

        public bool IndAtivo { get; set; }
    }
}
