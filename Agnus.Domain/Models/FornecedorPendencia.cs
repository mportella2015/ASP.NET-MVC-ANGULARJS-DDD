namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FornecedorPendencia")]
    public partial class FornecedorPendencia : KeyAuditableEntity
    {
        public DateTime DataPendencia { get; set; }

        public DateTime? DataResolucao { get; set; }

        [StringLength(200)]
        public string TxtPendencia { get; set; }

        public long IdTipoPendencia { get; set; }

        public virtual ConteudoTabelaDominio TipoPendencia { get; set; }

        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        public bool EnviarEmail { get; set; }

        [ForeignKey("TipoDocumento")]
        public long? IdTipoDocumento { get; set; }
        public virtual ConteudoTabelaDominio TipoDocumento { get; set; }

        public FornecedorPendencia()
        {
        }
        
        public void Resolvido()
        {
            DataResolucao = DateTime.Now;
        }
    }
}
