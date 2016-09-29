using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Service.Implementations
{
    public partial class WorkflowAprovacaoService : EntityService<WorkflowAprovacao>, IWorkflowAprovacaoService
    {
        public decimal ValorAlcadaDefault { get { return Convert.ToDecimal("999999999,99"); } }

        public WorkflowAprovacaoService(
            IUnitOfWork unitOfWork,
            IGenericRepository<WorkflowAprovacao> repository)
            : base(unitOfWork, repository)
        {            
        }        

        public override void EditEntity(WorkflowAprovacao entity)
        {            
            if (entity.IndAtivo)
            {
                entity.LoginExclusao = string.Empty;
                entity.DataExclusao = null;                
            }
            if (!entity.ValorAlcada.HasValue)
                entity.ValorAlcada = this.ValorAlcadaDefault;

            base.EditEntity(entity);
        }

    }
}
