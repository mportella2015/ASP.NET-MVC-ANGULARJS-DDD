namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoServicoTipoDocumento")]
    public partial class TipoServicoTipoDocumento : KeyAuditableEntity
    {
        [ForeignKey("TipoDocumento")]
        public long IdTipoDocumento { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }

        [ForeignKey("TipoServico")]
        public long IdTipoServico { get; set; }
        public virtual TipoServico TipoServico { get; set; }
    }
}
