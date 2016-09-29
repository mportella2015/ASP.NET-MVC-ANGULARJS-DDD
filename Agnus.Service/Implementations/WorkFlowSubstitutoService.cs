using Agnus.Domain.Models;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    class WorkFlowSubstitutoService : EntityService<WorkFlow>, IWorkFlowSubstitutoService
    {
        public WorkFlowSubstitutoService(IUnitOfWork unitOfWork, IGenericRepository<WorkFlow> repository)
            : base(unitOfWork, repository)
        {
        }

        private IEntityService<WorkflowSubstituto> _workflowSubstitutoService = DependencyResolver.Current.GetService<IEntityService<WorkflowSubstituto>>();

        public void AlterarPessoaTitular()
        {
            var listaWorflowSubtituir = GetAllQuery(y => y.DataParecer == null
                && y.ValorSLA > 0).ToList().Where(x => (int)(Convert.ToDateTime(DateTime.Now)-Convert.ToDateTime(x.DataNotificacao)).TotalDays > x.ValorSLA).ToList();

            if (listaWorflowSubtituir.Any()) 
            {
                foreach (var item in listaWorflowSubtituir)
                {
                    var substituto = _workflowSubstitutoService.GetAllByFilter(h => h.PessoaTitular.WorkflowAprovador.Where(x=>x.WorkFlowEntity.Id == item.WorkFlowEntity.Id).FirstOrDefault().PessoaAprovador.Id == item.PessoaAprovador.Id && h.IdObjetoAprovacao == item.WorkflowAprovacao.ObjetoAprovacao.Id).FirstOrDefault();

                    item.PessoaAprovador= substituto.PessoaSubstituto;
                    item.DataNotificacao = DateTime.Now;

                    AttachEntity(item);
                    Save(item);
                }
            }

        }
    }
}
