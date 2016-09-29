using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain
{
    public interface IAuditableEntity
    {
        string LoginCadastro { get; set; }

        DateTime DataCadastro { get; set; }

        string LoginUltimaAtualizacao { get; set; }

        DateTime? DataUltimaAtualizacao { get; set; }

        string LoginExclusao { get; set; }

        DateTime? DataExclusao { get; set; }
    }
}
