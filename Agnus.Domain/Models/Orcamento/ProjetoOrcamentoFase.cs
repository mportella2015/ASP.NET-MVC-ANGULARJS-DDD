using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Agnus.Framework;

namespace Agnus.Domain.Models
{

    [Table("ProjetoOrcamentoFase")]
    public class ProjetoOrcamentoFase : Entity
    {
        public string NomeFase { get; set; }

        public long IdOrcamento { get; set; }
        public ProjetoOrcamento Orcamento { get; set; }

        public ICollection<ProjetoOrcamentoEtapa> Etapas { get; set; }

        public ProjetoOrcamentoFase()
        {
            this.Etapas = new HashSet<ProjetoOrcamentoEtapa>();
        }
    }
}
