
namespace Agnus.Domain.Models
{
    using Agnus.Domain;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using Agnus.Domain.Models.Enum;

    [Table("TipoQuarto")]
    public partial class TipoQuarto : KeyAuditableEntity
    {
        public TipoQuarto(){}

        [DataMember]
        public int Id { get; set; }
        
        [StringLength(100)]
        public string Descricao { get; set; }
        [StringLength(1)]
        public string PossuiCamaCasal { get; set; }
        [StringLength(1)]
        public string PossuiAR { get; set; }
        [StringLength(1)]
        public string PossuiFrigoBar { get; set; }
        [StringLength(1)]
        public string PossuiInternet { get; set; }
        [StringLength(1)]
        public string PossuiHidroMassagem { get; set; }
                
        public int QuantidadePermitidaPessoas { get; set; }

        [StringLength(500)]
        public string Observacao { get; set; }

    }
}

