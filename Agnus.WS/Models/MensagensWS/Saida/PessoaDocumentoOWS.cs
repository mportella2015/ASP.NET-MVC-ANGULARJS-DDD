using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agnus.WS.Models.MensagensWS.Saida
{
    public class PessoaDocumentoOWS
    {        
        public bool FL_RG { get; set; }    
        public bool FL_CPF { get; set; }
        public bool FL_COMPROVANTERESIDENCIA { get; set; }    
        public bool FL_COMPROVANTERECOLHEINSS { get; set; }    
        public bool FL_CONTRATOSOCIAL { get; set; }    
        public bool p_fl_CNPJ { get; set; }    
        public bool p_fl_InscricaoMunicipal { get; set; }    
        public bool p_fl_InscricaoEstadual { get; set; }    
        public bool p_fl_BalancoAnual { get; set; }    
        public bool p_fl_DeclaracaoFaturamento { get; set; }
    }
}
