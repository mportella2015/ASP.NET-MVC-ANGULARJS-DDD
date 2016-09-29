//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Agnus.Domain.Models
{
    using Agnus.Domain;
    using Agnus.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CentroCusto")]
    public partial class CentroCusto : KeyAuditableEntity
    {
        public CentroCusto()
        {
            this.PedidoCompra = new HashSet<PedidoCompra>();
            this.SolicitacaoReembolso = new HashSet<SolicitacaoReembolso>();
        }

        public string CodCentroCusto { get; set; }
        public string NomCentroCusto { get; set; }
        public bool IndAtivo { get; set; }

        public decimal ValorFundoFixo { get; set; }
        public decimal? ValorSaldoFundoFixo { get; set; }
        public DateTime DataLimiteFundoFixo { get; set; }
        public int CodStatusCC { get; set; }        

        [ForeignKey("Nucleo")]
        public long IdNucleo { get; set; }
        public virtual Nucleo Nucleo { get; set; }

        public long? CodPessoaResponsavelCC { get; set; }

        public virtual ICollection<PedidoCompra> PedidoCompra { get; set; }

        public virtual ICollection<SolicitacaoReembolso> SolicitacaoReembolso { get; set; }
    }
}