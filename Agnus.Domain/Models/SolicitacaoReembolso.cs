using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agnus.Domain.Models
{
    [Table("SolicitacaoReembolso")]
    public class SolicitacaoReembolso : KeyAuditableEntity
    {
        public SolicitacaoReembolso()
        {
        }

        //public int NumSolicitacao { get; set; }
        public DateTime DataStatusReembolso { get; set; }

        public DateTime? DataPagamentoBoleta { get; set; }

        public decimal ValorTotalReembolso { get; set; }

        [ForeignKey("Nucleo")]
        public long? IdNucleo { get; set; }
        public virtual Nucleo Nucleo { get; set; }

        [ForeignKey("LoginSolicitante")]
        public long IdLoginSolicitante { get; set; }
        public virtual UsuarioSistema LoginSolicitante { get; set; }

        [ForeignKey("StatusReembolso")]
        public long? IdStatusReembolso { get; set; }
        public virtual ConteudoTabelaDominio StatusReembolso { get; set; }

        [ForeignKey("Empresa")]
        public long? IdEmpresa { get; set; }
        public virtual Pessoa Empresa { get; set; }

      }
}
