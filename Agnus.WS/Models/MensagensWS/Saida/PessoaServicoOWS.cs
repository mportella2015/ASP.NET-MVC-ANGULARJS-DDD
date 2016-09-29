using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agnus.WS.Models.MensagensWS.Saida
{
    public class PessoaServicoOWS
    {        
        public DateTime DT_INICIOSERVICO { get; set; }
        public DateTime DT_FIMSERVICO { get; set; }
        public long CD_CARGOFUNCAO { get; set; }
        public string TX_DESCRICAOSERVICO { get; set; }
        public decimal VL_REMUNERACAO { get; set; }
        public long CD_PROJETO { get; set; }
        public long CD_FASE { get; set; } 
        public string TX_OBSERVACAO { get; set; }
    }
}
