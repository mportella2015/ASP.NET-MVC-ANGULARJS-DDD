using Agnus.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Agnus.Domain.Models
{
    [Table("PrestacaoContasDetalhe")]
    public partial class PrestacaoContasDetalhe : KeyAuditableEntity, IWorkFlowEntity
    {
        public PrestacaoContasDetalhe()
        {
            this.WorkFlowEntity = new HashSet<WorkFlowEntity>();
        }

        public virtual ICollection<WorkFlowEntity> WorkFlowEntity { get; set; }

        [ForeignKey("PrestacaoContas")]
        public long IdPrestacaoContas { get; set; }
        public virtual PrestacaoContas PrestacaoContas { get; set; }

        [ForeignKey("ItemOrcamento")]
        public long? IdItemOrcamento { get; set; }
        public virtual ItemOrcamento ItemOrcamento { get; set; }

        [ForeignKey("ItemPlanoContas")]
        public long? IdItemPlanoContas { get; set; }
        public virtual ItemPlanoContas ItemPlanoContas { get; set; }

        public string NumDocumento { get; set; }

        public string NumSerieDocumento { get; set; }

        public DateTime? DataEmissaoDocumento { get; set; }

        public string CPFCNPJ { get; set; }

        public string NomeFornecedor { get; set; }

        public decimal ValorDocumento { get; set; }

        public string TxtObservacao { get; set; }

        public bool IndDevolucao { get; set; }

        [ForeignKey("TipoDocumento")]
        [DataMember]
        public long? IdTipoDocumento { get; set; }
        public virtual ConteudoTabelaDominio TipoDocumento { get; set; }

        [ForeignKey("StatusPrestacaoContasDetalhes")]
        [DataMember]
        public virtual long? IdStatusPrestacaoContasDetalhes { get; set; }
        public virtual ConteudoTabelaDominio StatusPrestacaoContasDetalhes { get; set; }

        [DataMember]
        public virtual DateTime? DatStatusPrestacaoContasDetalhes { get; set; }

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
            get { return this.ValorDocumento; }
        }

        public int CodObjetoUso
        {
            get { return (int)ObjetoComprometimentoUsoEnum.PretacaoContasIPC; }
        }


        public void SetStatusPai(ConteudoTabelaDominio StatusPai)
        {
            this.PrestacaoContas.StatusPrestacaoContas= StatusPai;
        }

        public void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus)
        {
            this.StatusPrestacaoContasDetalhes = StatusItem;
            if (updateDataStatus)
                this.DatStatusPrestacaoContasDetalhes = DateTime.Now;
        }

        public int GetCodStatusItemBy(Enum.ParecerAprovacaoEnum parecer)
        {
            switch (parecer)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    return (int)StatusItemPrestacaoContasEnum.Aprovada;
                case ParecerAprovacaoEnum.Reprovado:
                    return (int)StatusItemPrestacaoContasEnum.Reprovada;
                case ParecerAprovacaoEnum.EmAprovacao:
                    return (int)StatusItemPrestacaoContasEnum.EmAprovacao;
                case ParecerAprovacaoEnum.Revisar:
                    return (int)StatusItemPrestacaoContasEnum.EmRevisao;
                default:
                    break;
            }
            throw new Exception("Não foi possível encontrar um status para este item!");
        }

        public int StatusAprovado
        {
            get { return (int)StatusItemPrestacaoContasEnum.Aprovada; }
        }

        public int StatusEmAprovacao
        {
            get { return (int)StatusItemPrestacaoContasEnum.EmAprovacao; }
        }

        public int StatusReprovado
        {
            get { return (int)StatusItemPrestacaoContasEnum.Reprovada; }
        }

        public int StatusDominioPai
        {
            get { return (int)DominioGenericoEnum.StatusPrestacaodeContasAdiantamentoSolicitacaoReembolso; }
        }

        public int StatusCodPaiFinalizado
        {
            get
            {

                switch (this.PrestacaoContas.StatusPrestacaoContas.Texto)
                {
                    case "Em Revisão":
                        return (int)StatusPrestacaoContasEnum.EmRevisao;
                    default:
                        return (int)StatusPrestacaoContasEnum.Finalizado;
                }
            }
        }
        //public int StatusCodPaiFinalizado
        //{
        //    get { return (int)StatusProjetoEnum.Ativo; }
        //}
    }
}
