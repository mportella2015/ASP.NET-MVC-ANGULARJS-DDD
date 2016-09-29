using Agnus.Domain.Models;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Service.Implementations
{
    public class TemplateOrcamentoService : EntityService<Template>, ITemplateOrcamentoService
    {
        public TemplateOrcamentoService(IUnitOfWork unitOfWork, IGenericRepository<Template> repository)
            : base(unitOfWork, repository)
        {
        }

        public IEnumerable<Template> GetTemplatesByNucleo(Nucleo nucleo)
        {
            if(nucleo == null)
                throw new CustomServiceException("Núcleo Inválido");
            return this.GetAllByFilter(x => x.IdNucleo == nucleo.Id && x.IndAtivo);            
        }
    }
}
