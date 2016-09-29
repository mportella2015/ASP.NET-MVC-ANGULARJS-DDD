using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Agnus.Framework;

namespace Agnus.Domain.Models
{

    [Table("ProjetoOrcamentoEtapa")]
    public class ProjetoOrcamentoEtapa : Entity
    {
        public string NomeEtapa { get; set; }
        public long IdFase { get; set; }
        public ProjetoOrcamentoFase Fase { get; set; }
    }
}
