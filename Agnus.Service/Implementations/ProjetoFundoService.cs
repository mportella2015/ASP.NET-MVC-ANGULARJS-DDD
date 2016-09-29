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
    public class ProjetoFundoService : EntityService<ProjetoFundo>, IProjetoFundoService
    {
        private IEntityService<Projeto> _projetoService = DependencyResolver.Current.GetService<IEntityService<Projeto>>();
        private IEntityService<WorkFlowEntity> _workFlowEntityService = DependencyResolver.Current.GetService<IEntityService<WorkFlowEntity>>();

        public ProjetoFundoService(IUnitOfWork unitOfWork, IGenericRepository<ProjetoFundo> repository)
            : base(unitOfWork, repository)
        {
        }



        public bool AplicarAlteracao(long idProjetoFundo)
        {
            var idProjeto = this.GetAllQuery(x => x.Id == idProjetoFundo).FirstOrDefault().IdProjeto;

            var projetoCorrente = _projetoService.GetBy(idProjeto);
            var dadosAlterados = GetAllByFilter(x => x.IdProjeto == idProjeto).FirstOrDefault();

            if (dadosAlterados.ValFundo.HasValue)
                projetoCorrente.ValFundo = dadosAlterados.ValFundo.Value;

            if (dadosAlterados.DataLimiteFundo.HasValue)
                projetoCorrente.DataLimiteFundo = dadosAlterados.DataLimiteFundo.Value;

            try
            {
                _projetoService.Save(projetoCorrente);

                dadosAlterados.IndAtivo = false;
                Save(dadosAlterados);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no serviço [WorkflowProjetoFundoService]: "+ ex);
            }
            
        }
    }
}
