using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Correios.Net;

namespace Agnus.Service.Implementations
{
    public class TipoQuartoService : EntityService<Domain.Models.TipoQuarto>, ITipoQuartoService
    {

        public TipoQuartoService(IUnitOfWork unitOfWork, IGenericRepository<Domain.Models.TipoQuarto> repository)
            : base(unitOfWork, repository)
        {
        }
        
    }
}
