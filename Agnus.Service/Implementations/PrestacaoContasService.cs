using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System.Linq;

namespace Agnus.Service.Implementations
{
    public class PrestacaoContasService : EntityService<PrestacaoContas>, IPrestacaoContasService
    {
        public PrestacaoContasService(IUnitOfWork unitOfWork, IGenericRepository<PrestacaoContas> repository)
            : base(unitOfWork, repository)
        {
        }

        public string GetIncentivado(PrestacaoContas entity)
        {
            if (entity.Projeto != null)
            {
                var projRegistro = entity.Projeto.ProjetoRegistro.Any(x => x.DataAprovacao != null);
                var Incentivado = projRegistro ? "Sim" : "Não";
                return Incentivado;
            }

            return string.Empty;
        }

        public long CriarNovoNumPrestacao()
        {
            //return GetAll().Count() + 1;
            var ultimo = GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            if (ultimo != null)
                return (int)ultimo.Id + 1;
            else
                return 1;
        }
    }
}
