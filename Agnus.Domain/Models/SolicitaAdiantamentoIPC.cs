namespace Agnus.Domain.Models
{
    using Agnus.Domain.Models.Enum;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("SolicitaAdiantamentoIPC")]
    public partial class SolicitaAdiantamentoIPC : KeyAuditableEntity
    {

        [ForeignKey("SolicitacaoAdiantamento")]
        public long IdSolicitacaoAdiantamento { get; set; }
        public virtual SolicitacaoAdiantamento SolicitacaoAdiantamento { get; set; }

        [ForeignKey("ItemOrcamento")]
        [DataMember]
        public long? IdItemOrcamento { get; set; }
        public virtual ItemOrcamento ItemOrcamento { get; set; }

        [ForeignKey("ItemPlanoContas")]
        public long? IdItemPlanoContas { get; set; }
        public virtual ItemPlanoContas ItemPlanoContas { get; set; }

        public decimal ValorAdiantamentoIPC { get; set; }

        public string TxtObservacao { get; set; }

        public decimal? ValorSaldo { get; set; }
    }
}

