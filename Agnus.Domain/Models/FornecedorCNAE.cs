namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
using System.Runtime.Serialization;

    [Table("FornecedorCNAE")]
    [DataContract]
    public partial class FornecedorCNAE : KeyAuditableEntity, ILogFornecedor
    {
        public bool IndPrincipal { get; set; }

        [ForeignKey("ConteudoTabelaDominio")]
        public long IdConteudoTabelaDominio { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio ConteudoTabelaDominio { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "CNAE"; }
        }
    }
}
