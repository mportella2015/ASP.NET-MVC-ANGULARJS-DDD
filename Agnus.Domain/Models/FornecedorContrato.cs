namespace Agnus.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("FornecedorContrato")]
    [DataContract]
    public partial class FornecedorContrato : KeyAuditableEntity, ILogFornecedor
    {
        public FornecedorContrato()
        {
            FornecedorContratoServicoes = new HashSet<FornecedorContratoServico>();
        }

        [DataMember]
        public DateTime DataInicioContrato { get; set; }

        [DataMember]
        public DateTime DataFimContrato { get; set; }

        [DataMember]
        public DateTime? DataCancelamento { get; set; }

        [ForeignKey("IndiceReajuste")]
        public long? IdIndiceReajuste { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio IndiceReajuste { get; set; }

        [ForeignKey("StatusContrato")]
        public long? IdStatusContrato { get; set; }

        [DataMember]
        public virtual ConteudoTabelaDominio StatusContrato { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        public virtual ICollection<FornecedorContratoServico> FornecedorContratoServicoes { get; set; }

        [NotMapped]
        public string NomeEntidade
        {
            get { return "Contrato"; }
        }
    }
}
