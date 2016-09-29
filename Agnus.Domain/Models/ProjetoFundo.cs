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
    [Table("ProjetoFundo")]
    public partial class ProjetoFundo : KeyAuditableEntity, IWorkFlowEntity
    {
        [ForeignKey("Projeto")]
        public long IdProjeto { get; set; }
        public Projeto Projeto { get; set; }

        public ProjetoFundo()
        {
            this.WorkFlowEntity = new HashSet<WorkFlowEntity>();
        }

        public virtual ICollection<WorkFlowEntity> WorkFlowEntity { get; set; }

        public decimal? ValFundo { get; set; }

        [DataMember]
        public virtual DateTime? DataLimiteFundo { get; set; }

        [DataMember]
        public bool IndAtivo { get; set; }

        [NotMapped]
        public decimal? ValFundoAtual { get; set; }

        [NotMapped]
        [DataMember]
        public virtual string DataLimiteFundoAtual { get; set; }

        [ForeignKey("StatusProjetoFundo")]
        [DataMember]
        public virtual long? IdStatusProjetoFundo { get; set; }
        public virtual ConteudoTabelaDominio StatusProjetoFundo { get; set; }

        [DataMember]
        public virtual DateTime? DatStatusProjetoFundo { get; set; }

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
            get { throw new NotImplementedException(); }
        }

        public int CodObjetoUso
        {
            get { throw new NotImplementedException(); }
        }


        public void SetStatusPai(ConteudoTabelaDominio StatusPai)
        {
            this.StatusProjetoFundo = StatusPai;
        }

        public void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus)
        {
            this.StatusProjetoFundo = StatusItem;
            if (updateDataStatus)
                this.DatStatusProjetoFundo = DateTime.Now;
        }

        public int GetCodStatusItemBy(Enum.ParecerAprovacaoEnum parecer)
        {
            switch (parecer)
            {
                case ParecerAprovacaoEnum.Aprovado:
                    return (int)StatusProjetoFundoEnum.Aprovado;
                case ParecerAprovacaoEnum.Reprovado:
                    return (int)StatusProjetoFundoEnum.Reprovado;
                default:
                    break;
            }
            throw new Exception("Não foi possível encontrar um status para este item!");
        }

        public int StatusAprovado
        {
            get { return (int)StatusProjetoFundoEnum.Aprovado; }
        }

        public int StatusEmAprovacao
        {
            get { return (int)StatusProjetoFundoEnum.EmAprovacao; }
        }

        public int StatusReprovado
        {
            get { return (int)StatusProjetoFundoEnum.Reprovado; }
        }

        public int StatusDominioPai
        {
            get { return (int)DominioGenericoEnum.StatusProjetoFundo; }
        }

        public int StatusCodPaiFinalizado
        {
            get { return (int)StatusProjetoFundoEnum.Finalizado; }
        }
    }
}
