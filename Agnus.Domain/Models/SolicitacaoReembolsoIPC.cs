using Agnus.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agnus.Domain.Models
{
    [Table("SolicitacaoReembolsoIPC")]
    public class SolicitacaoReembolsoIPC : KeyAuditableEntity, IWorkFlowEntity
    {
        public SolicitacaoReembolsoIPC()
        {
            this.WorkFlowEntity = new HashSet<WorkFlowEntity>();
        }
        [StringLength(10)]
        [Required]
        public string NumDocumento { get; set; }

        [StringLength(1)]
        public string NumSerieDocumento { get; set; }

        public DateTime DataEmissaoDocumento { get; set; }

        [StringLength(18)]
        [Required]
        public string CPFCNPJ { get; set; }

        [Required]
        public string NomeFornecedor { get; set; }

        public decimal ValorDocumento { get; set; }

        public DateTime DataStatusItemReembolso { get; set; }

        [StringLength(100)]
        public string TxtObservacao { get; set; }

        [ForeignKey("SolicitacaoReembolso")]
        public long IdSolicitacaoReembolso { get; set; }
        public virtual SolicitacaoReembolso SolicitacaoReembolso { get; set; }

        [ForeignKey("ItemOrcamento")]
        public long? IdItemOrcamento { get; set; }
        public virtual ItemOrcamento ItemOrcamento { get; set; }

        [ForeignKey("ItemPlanoContas")]
        public long? IdItemPlanoContas { get; set; }
        public virtual ItemPlanoContas ItemPlanoContas { get; set; }

        [ForeignKey("StatusItemReembolso")]
        public long? IdStatusItemReembolso { get; set; }
        public virtual ConteudoTabelaDominio StatusItemReembolso { get; set; }

        public virtual ICollection<WorkFlowEntity> WorkFlowEntity { get; set; }

        public long IdEntity
        {
            get { return this.Id; }
        }

        public ItemOrcamento ItemOrcamentoEntity
        {
            get { return this.ItemOrcamento; }
        }

        public decimal ValorUsoEntity
        {
            get { return this.ValorDocumento; }
        }

        public int CodObjetoUso
        {
            get { return (int)ObjetoComprometimentoUsoEnum.SolicitacaoReembolsoIPC; }
        }

        public void SetStatusPai(ConteudoTabelaDominio StatusPai)
        {
            this.SolicitacaoReembolso.StatusReembolso = StatusPai;
        }

        public void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus)
        {
            this.StatusItemReembolso = StatusItem;
            if (updateDataStatus)
                this.DataStatusItemReembolso = DateTime.Now;
        }

        public int GetCodStatusItemBy(ParecerAprovacaoEnum parecer)
        {
            switch (parecer)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    return (int)StatusItemReembolsoEnum.Aprovada;
                case ParecerAprovacaoEnum.Reprovado:
                    return (int)StatusItemReembolsoEnum.Reprovada;
                case ParecerAprovacaoEnum.Revisar:
                    return (int)StatusItemReembolsoEnum.EmRevisao;
                case ParecerAprovacaoEnum.EmAprovacao:
                    return (int)StatusItemReembolsoEnum.EmAprovacao;
                default:
                    break;
            }
            throw new Exception("Não foi possível encontrar um status para este item!");
        }

        public int StatusAprovado
        {
            get { return (int)StatusItemReembolsoEnum.Aprovada; }
        }

        public int StatusEmAprovacao
        {
            get { return (int)StatusItemReembolsoEnum.EmAprovacao; }
        }

        public int StatusReprovado
        {
            get { return (int)StatusItemReembolsoEnum.Reprovada; }
        }

        public int StatusDominioPai
        {
            get { return (int)DominioGenericoEnum.StatusPrestacaodeContasAdiantamentoSolicitacaoReembolso; }
        }

        public int StatusCodPaiFinalizado
        {
            get
            {

                switch (this.SolicitacaoReembolso.StatusReembolso.Texto)
                {
                    case "Em Revisão":
                        return (int)StatusReembolsoEnum.EmRevisao;
                    default:
                        return (int)StatusReembolsoEnum.Finalizada;
                }
            }
        }
        
    }
}
