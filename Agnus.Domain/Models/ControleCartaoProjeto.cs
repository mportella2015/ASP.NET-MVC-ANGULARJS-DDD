namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ControleCartaoProjeto")]
    public partial class ControleCartaoProjeto : KeyAuditableEntity
    {
        [ForeignKey("ControleCartao")]
        public long? IdControleCartao { get; set; }
        public ControleCartao ControleCartao { get; set; }

        [ForeignKey("Projeto")]
        public long IdProjeto { get; set; }
        public Projeto Projeto { get; set; }

    }
}

