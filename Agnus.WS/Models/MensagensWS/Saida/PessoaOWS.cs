using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agnus.WS.Models.MensagensWS.Saida
{
    public abstract class PessoaOWS
    {
        public long CD_PESSOAQUALITY { get; set; }
        public long cd_Pessoa { get; set; }
        public int cd_Nacionalidade { get; set; }
        public int cd_Pais { get; set; }
        public string nm_Pessoa { get; set; }
        public string nm_Fantasia { get; set; }
        public string tx_Endereco { get; set; }
        public string tx_Bairro { get; set; }
        public string tx_Cidade { get; set; }
        public string tx_Estado { get; set; }
        public string tx_Cep { get; set; }
        public string tx_Observacao { get; set; }
        public string tx_Numero { get; set; }
        public string tx_Complemento { get; set; }
        public int? cd_Estado { get; set; }
        public int? cd_nucleopessoa { get; set; }
        public int? cd_Municipio { get; set; }
        public string tx_Site { get; set; }
        public List<TelefoneOWS> LST_TELEFONE { get; set; }
        public List<EmailOWS> LST_EMAIL { get; set; }
        public List<PessoaServicoOWS> LST_SERVICO { get; set; }
        //public List<PessoaDocumentoOWS> LST_DOCUMENTO { get; set; }
        public PessoaDocumentoOWS LST_DOCUMENTO { get; set; }
    }
}