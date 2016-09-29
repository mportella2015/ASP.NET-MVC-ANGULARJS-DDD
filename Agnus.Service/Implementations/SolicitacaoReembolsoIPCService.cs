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
    public class SolicitacaoReembolsoIPCService : EntityService<SolicitacaoReembolsoIPC>, ISolicitacaoReembolsoIPCService
    {
        public SolicitacaoReembolsoIPCService(IUnitOfWork unitOfWork, IGenericRepository<SolicitacaoReembolsoIPC> repository)
            : base(unitOfWork, repository)
        {
        }
    }
}
