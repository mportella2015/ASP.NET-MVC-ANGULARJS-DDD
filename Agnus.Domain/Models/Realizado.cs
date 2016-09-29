using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Agnus.Domain.Models.Enum;

namespace Agnus.Domain.Models
{
    [Table("Realizado")]
    public partial class Realizado : KeyAuditableEntity, IWorkFlowEntity
    {
        public Realizado()
        {
            PedidoCompraItens = new HashSet<PedidoCompraItem>();
            this.WorkFlowEntity = new HashSet<WorkFlowEntity>();
        }

        public virtual ICollection<WorkFlowEntity> WorkFlowEntity { get; set; }

        public int NumRealizado { get; set; }
        public bool IndTemContrato { get; set; }

        [StringLength(20)]
        [DataMember]
        public string NumNotaFiscal { get; set; }

        public DateTime? DataPagamentoBoleta { get; set; }
        public DateTime DataEmissaoNotaFiscal { get; set; }
        public DateTime DataVencimentoNotaFiscal { get; set; }
        public DateTime DataRecebimentoNotaFiscal { get; set; }
        public decimal ValorNotaFiscal { get; set; }
        public decimal ValorImposto { get; set; }
        //public int? NumBoleta { get; set; }
        //public DateTime? DataBoleta { get; set; }
        public int NumBanco { get; set; }
        public string CodAgencia { get; set; }
        public string NumContaBancaria { get; set; }
        public string Observacao { get; set; }

        [ForeignKey("Fornecedor")]
        public long IdFornecedor { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }

        [ForeignKey("NaturezaDespesa")]
        public long IdNaturezaDespesa { get; set; }
        public virtual ConteudoTabelaDominio NaturezaDespesa { get; set; }

        [ForeignKey("StatusRealizado")]
        public long IdStatusRealizado { get; set; }
        public virtual ConteudoTabelaDominio StatusRealizado { get; set; }

        [ForeignKey("FormaPagamento")]
        public long IdFormaPagamento { get; set; }
        public virtual ConteudoTabelaDominio FormaPagamento { get; set; }

        public virtual ICollection<PedidoCompraItem> PedidoCompraItens { get; set; }

        [ForeignKey("TipoContaBancaria")]
        public long? IdTipoContaBancaria { get; set; }
        public virtual ConteudoTabelaDominio TipoContaBancaria { get; set; }

        public long IdEntity
        {
            get { return this.Id; }
        }

        public ItemOrcamento ItemOrcamentoEntity
        {
            get { return null; }
        }

        public decimal ValorUsoEntity
        {
            get { return this.ValorNotaFiscal; }
        }

        public int CodObjetoUso
        {
            get { return (int)ObjetoComprometimentoUsoEnum.Realizado; }
        }

        public void SetStatusPai(ConteudoTabelaDominio StatusPai)
        {
            this.StatusRealizado = StatusPai;
        }

        public void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus)
        {
            this.StatusRealizado = StatusItem;
            if (updateDataStatus)
                this.DataCadastro = DateTime.Now;
        }

        public int GetCodStatusItemBy(Enum.ParecerAprovacaoEnum parecer)
        {
            switch (parecer)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    return (int)StatusRealizadoEnum.LiberadoPraPagamento;
                case ParecerAprovacaoEnum.Reprovado:
                    return (int)StatusRealizadoEnum.Reprovado;
                case ParecerAprovacaoEnum.Revisar:
                    return (int)StatusRealizadoEnum.EmRevisao;
                case ParecerAprovacaoEnum.EmAprovacao:
                    return (int)StatusRealizadoEnum.EmAprovacao;
                default:
                    break;
            }
            throw new Exception("Não foi possível encontrar um status para este item!");
        }

        public int StatusAprovado
        {
            get { return (int)StatusRealizadoEnum.LiberadoPraPagamento; }
        }

        public int StatusEmAprovacao
        {
            get { return (int)StatusRealizadoEnum.EmAprovacao; }
        }

        public int StatusReprovado
        {
            get { return (int)StatusRealizadoEnum.Reprovado; }
        }

        public int StatusDominioPai
        {
            get { return (int)DominioGenericoEnum.StatusRealizado; }
        }

        public int StatusCodPaiFinalizado
        {
            get
            {

                switch (this.StatusRealizado.Texto)
                {
                    case "Reprovado":
                        return (int)StatusRealizadoEnum.Reprovado;
                    case "Aprovado":
                        return (int)StatusRealizadoEnum.LiberadoPraPagamento;
                    case "Em Revisão":
                        return (int)StatusRealizadoEnum.EmRevisao;
                    //case "Cancelado":
                    //    return (int)StatusRealizadoEnum.Cancelado;
                    default:
                        return (int)StatusRealizadoEnum.Reprovado;
                }

                //return this.StatusRealizado.Texto == "Reprovado" ? (int)StatusRealizadoEnum.Reprovado : (int)StatusRealizadoEnum.LiberadoPraPagamento; }
                //get { return (int)StatusRealizadoEnum.Reprovado; }
            }
        }
    }
}
