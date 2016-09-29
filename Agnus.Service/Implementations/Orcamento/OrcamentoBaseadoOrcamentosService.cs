using Conspiracao.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conspiracao.Service.Implementations;
using Conspiracao.Framework;
using Conspiracao.Service.Interfaces;
using Conspiracao.Domain.Models.Enum;

namespace Conspiracao.Service.Implementations
{
    public partial class OrcamentoService
    {
        private void CriarOrcamentoBaseadoOrcamentos(ProjetoOrcamento entity)
        {
            var grupoAprovacao = this.CriarGrupoAprovacao();            
        }        
    }
}
