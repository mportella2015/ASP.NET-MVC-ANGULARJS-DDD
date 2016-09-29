using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agnus.WS.Models.MensagensWS.Saida
{
    public class PessoaFisicaOWS : PessoaOWS, IMensagemOWS
    {
        public string tx_NumeroIdentificacao { get; set; }
        public string tx_OrgaoEmissor { get; set; }
        public DateTime? dt_Emissao { get; set; }
        public string tx_Cpf { get; set; }
        public string tx_RegistroTecnico { get; set; }
        public DateTime dt_Nascimento { get; set; }
        public int? tp_Sexo { get; set; }
        public string tx_inscricaoINSS { get; set; }
        public string tx_LivroDRT { get; set; }
        public string tx_FolhaLivroDRT { get; set; }
        public string tx_Naturalidade { get; set; }
    }
}