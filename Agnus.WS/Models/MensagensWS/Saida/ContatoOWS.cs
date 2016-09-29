using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agnus.WS.Models.MensagensWS.Saida
{
    public class ContatoOWS
    {
        public string NM_CONTATO { get; set; }
        public string TX_FUNCAO { get; set; }
        public string TX_TELEFONE { get; set; }
        public string TX_EMAIL { get; set; }
        public int NU_NIVELHIERARQUICO { get; set; }
    }
}