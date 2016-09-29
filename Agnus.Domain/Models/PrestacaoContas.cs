using Agnus.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models
{
    [Table("PrestacaoContas")]
    public partial class PrestacaoContas : KeyAuditableEntity
    {
        public PrestacaoContas()
        {
            this.PrestacaoContasDetalhe = new HashSet<PrestacaoContasDetalhe>();
        }

        public virtual ICollection<PrestacaoContasDetalhe> PrestacaoContasDetalhe { get; set; }

        public string Numero { get; set; }

        public DateTime DataPrestacaoContas { get; set; }

        public decimal ValorPrestacao { get; set; }

        [ForeignKey("PessoaResponsavelVerba")]
        public long IdPessoaResponsavelVerba { get; set; }
        public virtual Pessoa PessoaResponsavelVerba { get; set; }

        [ForeignKey("Projeto")]
        public long? IdProjeto { get; set; }
        public virtual Projeto Projeto { get; set; }

        [ForeignKey("CentroCusto")]
        public long? IdCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }

        [ForeignKey("Nucleo")]
        public long IdNucleo { get; set; }
        public virtual Nucleo Nucleo { get; set; }

        [ForeignKey("StatusPrestacaoContas")]
        public virtual long IdStatusPrestacaoContas { get; set; }
        public virtual ConteudoTabelaDominio StatusPrestacaoContas { get; set; }

        public virtual DateTime? DataStatusPrestacaoContas { get; set; }

        public decimal CalcularValorTotalPor(StatusPrestacaoContasDetalhesEnum statusItem)
        {
            return this.PrestacaoContasDetalhe.Where(x => x.StatusPrestacaoContasDetalhes != null && x.StatusPrestacaoContasDetalhes.Codigo == (int)statusItem).Sum(x => x.ValorDocumento);
        }

        public decimal CalcularValorTotalPor()
        {
            return this.PrestacaoContasDetalhe.Where(x => x.StatusPrestacaoContasDetalhes != null
                                                       && x.StatusPrestacaoContasDetalhes.Codigo != (int)StatusPrestacaoContasDetalhesEnum.Cancelada
                                                       && x.StatusPrestacaoContasDetalhes.Codigo != (int)StatusPrestacaoContasDetalhesEnum.Reprovada).Sum(x => x.ValorDocumento);
        }  
    }
}
