namespace Agnus.Domain.Models
{
    using Agnus.Domain.Models.Enum;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ControleCartao")]
    public partial class ControleCartao : KeyAuditableEntity
    {
        public string NumCartao { get; set; }
        public DateTime DataAtivacao { get; set; }
        public DateTime DataValidade { get; set; }
        public DateTime DataStatus { get; set; }

        [ForeignKey("StatusCartao")]
        public long? IdStatusCartao { get; set; }
        public virtual ConteudoTabelaDominio StatusCartao { get; set; }

        [ForeignKey("ModalidadeCartao")]
        public long? IdModalidadeCartao { get; set; }
        public virtual ConteudoTabelaDominio ModalidadeCartao { get; set; }

        [ForeignKey("Pessoa")]
        public long IdPessoa { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        public long? CodSolicitacaoCartao { get; set; }

        public string SolicitanteCartao { get; set; }
    }
}

