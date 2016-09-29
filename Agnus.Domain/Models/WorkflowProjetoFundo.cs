using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conspiracao.Domain.Models
{
    [Table("ProjetoFundo")]
    public class ProjetoFundo : KeyAuditableEntity
    {
        [ForeignKey("Projeto")]
        public long IdProjeto { get; set; }
        public Projeto Projeto { get; set; }

        public decimal? ValFundo { get; set; }

        public DateTime? DataLimiteFundo { get; set; }

        [DataMember]
        public bool IndAtivo { get; set; }

        [NotMapped]
        public decimal? ValFundoAtual
        {
            get { return this.Projeto.ValFundo; }
        }

        [NotMapped]
        public DateTime? DataLimiteFundoAtual
        {
            get { return this.Projeto.DataLimiteFundo; }
        }
    }
}
