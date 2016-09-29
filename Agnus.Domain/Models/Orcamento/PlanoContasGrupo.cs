//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Agnus.Domain.Models
{
    using Agnus.Framework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PlanoContasGrupo")]
    public partial class PlanoContasGrupo : Entity
    {
        public PlanoContasGrupo()
        {
            this.ItemPlanoContas = new HashSet<ItemPlanoContas>();
        }

        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Nome { get; set; }

        [ForeignKey("PlanoContas")]
        public long IdPlanoContas { get; set; }
        public PlanoContas PlanoContas { get; set; }

        public virtual ICollection<ItemPlanoContas> ItemPlanoContas { get; set; }
    }
}