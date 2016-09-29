using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models
{
    [Table("ProjetoOrcamentoDuracao")]
    public class OrcamentoDuracao : KeyAuditableEntity
    {
        [Column(name:"NumIdProjetoOrcamentoDuracao")]
        public override long Id { get; set; }

        public long NumIdProjetoOrcamento { get; set; }
        public virtual ProjetoOrcamento Orcamento { get; set; }

        [StringLength(20)]
        public string TxtDuracaoProjeto { get; set; }
    }
}
