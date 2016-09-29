namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
using System.Runtime.Serialization;

    [Table("FornecedorAvaliacao")]
    [DataContract]
    public partial class FornecedorAvaliacao : KeyAuditableEntity, ILogFornecedor
    {
        public DateTime DataAvaliacao { get; set; }

        [ForeignKey("UsuarioSistema")]
        [DataMember]
        public long IdUsuarioSistema { get; set; }

        [DataMember]
        public virtual UsuarioSistema UsuarioSistema { get; set; }

        [ForeignKey("ResultadoAvaliacao")]
        [DataMember]
        public long IdResultadoAvaliacao { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio ResultadoAvaliacao { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Avaliação"; }
        }
    }
}
