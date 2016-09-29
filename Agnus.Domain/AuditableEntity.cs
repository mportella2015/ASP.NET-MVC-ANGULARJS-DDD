using Agnus.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain
{
    public abstract class AuditableEntity : Entity, IAuditableEntity
    {
        [Required]
        [StringLength(60)]
        public string LoginCadastro { get; set; }

        public DateTime DataCadastro { get; set; }

        [StringLength(60)]
        public string LoginUltimaAtualizacao { get; set; }

        public DateTime? DataUltimaAtualizacao { get; set; }

        [StringLength(60)]
        public string LoginExclusao { get; set; }

        public DateTime? DataExclusao { get; set; }
    }
}
