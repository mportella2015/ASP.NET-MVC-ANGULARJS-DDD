using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models
{
    [Table("WorkFlow")]
    public class WorkFlow : KeyAuditableEntity
    {
        public DateTime DataNotificacao { get; set; }//
        public int NumNivel { get; set; }//
        public DateTime? DataParecer { get; set; }//
        public decimal? QtdTempoAprovacao { get; set; }//
        public decimal? ValorAlcada { get; set; }//
        public int? NumProximoNivel { get; set; }
        public decimal? ValorSLA { get; set; }
        public string TxtJustificativa { get; set; }              
     

        [ForeignKey("ParecerAprovacao")]
        public long? IdParecerAprovacao { get; set; }
        public virtual ConteudoTabelaDominio ParecerAprovacao { get; set; }

        [ForeignKey("PessoaAprovador")]
        public long IdPessoaAprovador { get; set; }
        public virtual Pessoa PessoaAprovador { get; set; }
                
        public string TipoWorkFlowEntity { get; set; }

        [ForeignKey("WorkFlowEntity")]
        public long IdWorkFlowEntity { get; set; }
        public virtual WorkFlowEntity WorkFlowEntity { get; set; }        

        [ForeignKey("WorkflowAprovacao")]
        public long IdWorkflowAprovacao { get; set; }
        public virtual WorkflowAprovacao WorkflowAprovacao { get; set; }
    }
}
