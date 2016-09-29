namespace Agnus.Domain.Models
{
    using Agnus.Domain.Models.Enum;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SolicitacaoAdiantamento")]
    public partial class SolicitacaoAdiantamento : KeyAuditableEntity, IWorkFlowEntity
    {

        public SolicitacaoAdiantamento()
        {
            this.WorkFlowEntity = new HashSet<WorkFlowEntity>();
            this.SolicitaAdiantamentoIPC = new HashSet<SolicitaAdiantamentoIPC>();
            this.SAPrestacoes = new HashSet<SA_PrestacaoContas>();
        }

        public virtual ICollection<WorkFlowEntity> WorkFlowEntity { get; set; }

        public virtual ICollection<SolicitaAdiantamentoIPC> SolicitaAdiantamentoIPC { get; set; }

        public virtual ICollection<SA_PrestacaoContas> SAPrestacoes { get; set; }

        public long CodSolicitacao { get; set; }

        public string PassosWorkflow { get; set; }

        public int? Cod_AdiantamentoConspiraware { get; set; }

        [ForeignKey("Projeto")]
        public long? IdProjeto { get; set; }
        public virtual Projeto Projeto { get; set; }

        [ForeignKey("CentroCusto")]
        public long? IdCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }

        [ForeignKey("Empresa")]
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }

        [ForeignKey("Nucleo")]
        public long IdNucleo { get; set; }
        public virtual Nucleo Nucleo { get; set; }

        [ForeignKey("ControleCartao")]
        public long? IdControleCartao { get; set; }
        public virtual ControleCartao ControleCartao { get; set; }

        public string LoginSolicitante { get; set; }

        [ForeignKey("FormaLiberacaoAdiantamento")]
        public long IdFormaLiberacaoAdiantamento { get; set; }
        public virtual ConteudoTabelaDominio FormaLiberacaoAdiantamento { get; set; }

        public string TxtJustificativa { get; set; }

        public DateTime DataStatusAdiantamento { get; set; }

        [ForeignKey("StatusAdiantamento")]
        public long IdStatusAdiantamento { get; set; }
        public virtual ConteudoTabelaDominio StatusAdiantamento { get; set; }

        public decimal ValorTotalAdiantamento { get; set; }

        public decimal ValorSaldo { get; set; }

        public DateTime DataVencimento { get; set; }

        public DateTime? DataPagamento { get; set; }

        public string TxtPassoWorkflow { get; set; }

        public string NumCartao { get; set; }

        public bool Ind1aSACartaoProjeto { get; set; }

        [ForeignKey("PessoaResponsavelVerba")]
        public long IdPessoaResponsavelVerba { get; set; }
        public virtual Pessoa PessoaResponsavelVerba { get; set; }

        [ForeignKey("PessoaFavorecido")]
        public long IdPessoaFavorecido { get; set; }
        public virtual Pessoa PessoaFavorecido { get; set; }
        
        public DateTime? DataSolicitacao { get; set; } 

        public long IdEntity
        {
            get { return this.Id;}
        }

        public ItemOrcamento ItemOrcamentoEntity
        {
            get { return null; }
        }

        public decimal ValorUsoEntity
        {
            get { return this.ValorTotalAdiantamento; }
        }

        public int CodObjetoUso
        {
            get { return (int)ObjetoComprometimentoUsoEnum.SolicitacaoAdiantamentoIPC; }
        }

        public void SetStatusPai(ConteudoTabelaDominio StatusPai)
        {
            this.StatusAdiantamento = StatusPai;
        }

        public void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus)
        {
            this.StatusAdiantamento = StatusItem;
            if (updateDataStatus)
                this.DataStatusAdiantamento = DateTime.Now;
        }

        public int GetCodStatusItemBy(ParecerAprovacaoEnum parecer)
        {
            switch (parecer)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    return (int)StatusSolicitacaoAdiantamentoEnum.Aprovada;
                case ParecerAprovacaoEnum.Reprovado:
                    return (int)StatusSolicitacaoAdiantamentoEnum.Reprovada;
                case ParecerAprovacaoEnum.Revisar:
                    return (int)StatusSolicitacaoAdiantamentoEnum.EmRevisao;
                case ParecerAprovacaoEnum.EmAprovacao:
                    return (int)StatusSolicitacaoAdiantamentoEnum.EmAprovacao;
                default:
                    break;
            }
            throw new Exception("Não foi possível encontrar um status para este item!");
        }

        public int StatusAprovado
        {
            get { return (int)StatusSolicitacaoAdiantamentoEnum.Aprovada; }
        }

        public int StatusEmAprovacao
        {
            get { return (int)StatusSolicitacaoAdiantamentoEnum.EmAprovacao; }
        }

        public int StatusReprovado
        {
            get { return (int)StatusSolicitacaoAdiantamentoEnum.Reprovada; }
        }

        public int StatusDominioPai
        {
            get { return (int)DominioGenericoEnum.StatusSolicitacaoAdiantamento; }
        }

        public int StatusCodPaiFinalizado
        {
            //get { return (int)StatusSolicitacaoAdiantamentoEnum.Aprovada; }

            get {
                switch (this.StatusAdiantamento.Texto)
                {
                    case "Reprovada":
                        return (int)StatusSolicitacaoAdiantamentoEnum.Reprovada;
                    case "Aprovado":
                        return (int)StatusSolicitacaoAdiantamentoEnum.Aprovada;
                    default:
                        return (int)StatusSolicitacaoAdiantamentoEnum.Aprovada;
                }
            }
        }
    }
}

