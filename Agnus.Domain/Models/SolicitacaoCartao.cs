namespace Agnus.Domain.Models
{
    using Agnus.Domain.Models.Enum;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("SolicitacaoCartao")]
    public partial class SolicitacaoCartao : KeyAuditableEntity, IWorkFlowEntity
    {
        public SolicitacaoCartao()
        {
            this.WorkFlowEntity = new HashSet<WorkFlowEntity>();
        }

        public virtual ICollection<WorkFlowEntity> WorkFlowEntity { get; set; }

        public long CodSolicitacao { get; set; }

        public DateTime DataStatusSolicitacao { get; set; }
        public string TxtMensagem { get; set; }

        [ForeignKey("Pessoa")]
        public long IdPessoa { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        [ForeignKey("Nucleo")]
        public long IdNucleo { get; set; }
        public virtual Nucleo Nucleo { get; set; }

        [ForeignKey("Projeto")]
        public long? IdProjeto { get; set; }
        public virtual Projeto Projeto { get; set; }

        [ForeignKey("CentroCusto")]
        public long? IdCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }

        [ForeignKey("ModalidadeCartao")]
        public long? IdModalidadeCartao { get; set; }
        public virtual ConteudoTabelaDominio ModalidadeCartao { get; set; }

        [ForeignKey("StatusSolicitacaoAdiantamento")]
        public long? IdStatusSolicitacaoAdiantamento { get; set; }
        public virtual ConteudoTabelaDominio StatusSolicitacaoAdiantamento { get; set; }

        public string SolicitanteCartao { get; set; }


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
            get { return 0; }
        }

        public int CodObjetoUso
        {
            get { return (int)ObjetoComprometimentoUsoEnum.SolicitacaoControleCartao; }
        }

        public void SetStatusPai(ConteudoTabelaDominio StatusPai)
        {
            this.StatusSolicitacaoAdiantamento = StatusPai;
        }

        public void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus)
        {
            this.StatusSolicitacaoAdiantamento = StatusItem;
            if (updateDataStatus)
                this.DataCadastro = DateTime.Now;
        }

        public int GetCodStatusItemBy(Enum.ParecerAprovacaoEnum parecer)
        {
            switch (parecer)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    return (int)StatusSolicitacaoItemControleCartoesEnum.Aprovado;
                case ParecerAprovacaoEnum.Reprovado:
                    return (int)StatusSolicitacaoItemControleCartoesEnum.Reprovado;
                case ParecerAprovacaoEnum.EmAprovacao:
                    return (int)StatusSolicitacaoItemControleCartoesEnum.EmAprovacao;
                default:
                    break;
            }
            throw new Exception("Não foi possível encontrar um status para este item!");
        }

        public int StatusAprovado
        {
            get { return (int)StatusSolicitacaoItemControleCartoesEnum.Aprovado; }
        }

        public int StatusEmAprovacao
        {
            get { return (int)StatusSolicitacaoItemControleCartoesEnum.EmAprovacao; }
        }

        public int StatusReprovado
        {
            get { return (int)StatusSolicitacaoItemControleCartoesEnum.Reprovado; }
        }

        public int StatusDominioPai
        {
            get { return (int)DominioGenericoEnum.SolicitacaoControleCartoes; }
        }

        public int StatusCodPaiFinalizado
        {
            get { return (int)StatusSolicitacaoControleCartoesEnum.Finalizado; }
        }
    }
}

