using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models
{
    [Table("OrcamentoBibliotecaVersao")]
    public partial class OrcamentoBibliotecaVersao : KeyAuditableEntity
    {
        public OrcamentoBibliotecaVersao()
        {

        }
        
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal ValorDefault { get; set; }        

        [ForeignKey("ItemOrcamento")]
        public long IdItemOrcamento { get; set; }
        public virtual ItemOrcamento ItemOrcamento { get; set; }

        [ForeignKey("BibliotecaItem")]
        public long IdBibliotecaItem { get; set; }
        public virtual BibliotecaItem BibliotecaItem { get; set; }
           
    }
}
