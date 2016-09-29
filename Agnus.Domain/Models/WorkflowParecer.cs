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
    using Agnus.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WorkflowParecer")]
    public partial class WorkflowParecer : KeyAuditableEntity
    {
        public bool IndExigeJustificativa { get; set; }

        [ForeignKey("ParecerAprovacao")]
        public long IdParecerAprovacao { get; set; }
        public virtual ConteudoTabelaDominio ParecerAprovacao { get; set; }

        [ForeignKey("WorkflowAprovacao")]
        public long? IdWorkflowAprovacao { get; set; }
        public virtual WorkflowAprovacao WorkflowAprovacao { get; set; }
    }
}