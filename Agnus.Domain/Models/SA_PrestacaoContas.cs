using Agnus.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models
{
    [Table("SA_PrestacaoContas")]
    public partial class SA_PrestacaoContas : KeyAuditableEntity
    {

        [ForeignKey("SolicitacaoAdiantamento")]
        public long IdSolicitacaoAdiantamento { get; set; }
        public virtual SolicitacaoAdiantamento SolicitacaoAdiantamento { get; set; }

        [ForeignKey("PrestacaoContasDetalhe")]
        public long IdPrestacaoContasDetalhe { get; set; }
        public virtual PrestacaoContasDetalhe PrestacaoContasDetalhe { get; set; }

        public decimal ValorSA { get; set; }
    }
}
