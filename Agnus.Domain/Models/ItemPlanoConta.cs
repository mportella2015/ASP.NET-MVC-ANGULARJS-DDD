namespace Conspiracao.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemPlanoConta : KeyAuditableEntity
    {
        public ItemPlanoConta()
        {
            TipoMaterials = new HashSet<TipoMaterial>();
            TipoServicoes = new HashSet<TipoServico>();
        }

        [Required]
        [StringLength(150)]
        public string TxtItemPlanoConta { get; set; }

        public virtual ICollection<TipoMaterial> TipoMaterials { get; set; }

        public virtual ICollection<TipoServico> TipoServicoes { get; set; }

        public bool IndAtivo { get; set; }
    }
}
