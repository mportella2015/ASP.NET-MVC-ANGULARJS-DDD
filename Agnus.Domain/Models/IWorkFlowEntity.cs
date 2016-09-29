using Agnus.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models
{
    public interface IWorkFlowEntity
    {
        long IdEntity { get; }

        ItemOrcamento ItemOrcamentoEntity { get; }

        decimal ValorUsoEntity { get; }

        int CodObjetoUso { get; }

        void SetStatusPai(ConteudoTabelaDominio StatusPai);

        void SetStatusItem(ConteudoTabelaDominio StatusItem, bool updateDataStatus);

        int GetCodStatusItemBy(ParecerAprovacaoEnum idParecer);

        int StatusAprovado { get; }

        int StatusEmAprovacao { get; }

        int StatusReprovado { get; }

        int StatusDominioPai { get; }

        int StatusCodPaiFinalizado { get; }
    }
}
