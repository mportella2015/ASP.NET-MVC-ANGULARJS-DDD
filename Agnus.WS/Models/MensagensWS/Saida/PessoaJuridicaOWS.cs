using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;


namespace Agnus.WS.Models.MensagensWS.Saida
{

    public class PessoaJuridicaOWS : PessoaOWS, IMensagemOWS
    {
        public int cd_PaisCobranca { get; set; }
        public string tx_CNPJ { get; set; }
        public string tx_InscricaoMunicipal { get; set; }
        public string tx_InscricaoEstadual { get; set; }
        public string tx_EnderecoCobranca { get; set; }
        public string tx_BairroCobranca { get; set; }
        public string tx_CidadeCobranca { get; set; }
        public string tx_EstadoCobranca { get; set; }
        public string tx_CepCobranca { get; set; }
        public string tx_NumeroCobranca { get; set; }
        public string tx_ComplementoCobranca { get; set; }
        public int? fl_Simples { get; set; }
        public int? fl_IssRj { get; set; }
        public int? fl_IssSp { get; set; }
        public decimal? vl_AliquotaISS { get; set; }
        public int? fl_MEI { get; set; }
        public int? cd_PessoaInterveniente { get; set; }
        public int? cd_EstadoCobranca { get; set; }
        public int? cd_MunicipioCobranca { get; set; }
        public List<SocioOWS> LST_SOCIO { get; set; }
        public List<ContatoOWS> LST_CONTATO { get; set; }
    }
}