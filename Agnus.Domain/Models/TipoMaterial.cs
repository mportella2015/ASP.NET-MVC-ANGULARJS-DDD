namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoMaterial")]
    public partial class TipoMaterial : KeyAuditableEntity
    {
        public TipoMaterial()
        {
            FornecedorMaterials = new HashSet<FornecedorMaterial>();
        }

        [Required]
        [StringLength(50)]
        public string TxtTipoMaterial { get; set; }

        [StringLength(50)]
        public string TxtTipoMaterialIngles { get; set; }

        public virtual ICollection<FornecedorMaterial> FornecedorMaterials { get; set; }

        public bool IndAtivo { get; set; }
    }
}
