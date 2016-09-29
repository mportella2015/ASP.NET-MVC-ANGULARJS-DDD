using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Framework.Helper;
using Agnus.Service.DTO;
using Agnus.Service.DTO.Orcamento;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Agnus.Service.Implementations
{
    public partial class OrcamentoTotalService : EntityService<OrcamentoTotal>, IOrcamentoTotalService
    {

        public OrcamentoTotalService(
            IUnitOfWork unitOfWork,
            IGenericRepository<OrcamentoTotal> repository)
            : base(unitOfWork, repository)
        {

        }

        public bool ValidarOrcamentos(List<OrcamentoImport> orcamentosCSV, long idProjeto)
        {
            var orcamentos = this.GetAllByFilter(x => x.IdProjetoOrcamento == idProjeto);// && orcamentosCSV.Any(y => y.Fase == x.NomeFase && y.Etapa == x.NomeEtapa && y.Grupo == x.NomeGrupo && y.Conta == x.NomeItem));

            var query = from orcamento in orcamentos
                        from orcamentoCSV in orcamentosCSV
                        where orcamento.NomeFase == orcamentoCSV.Fase && orcamento.NomeEtapa == orcamentoCSV.Etapa && orcamento.NomeGrupo == orcamentoCSV.Grupo && orcamento.NomeItem == orcamentoCSV.Conta
                        select orcamento;

            if (orcamentosCSV.Count() != query.Count())
                return false;

            foreach (var orcamento in query.ToList())
            {
                var orcCSV = orcamentosCSV.FirstOrDefault(x => x.Fase == orcamento.NomeFase && x.Etapa == orcamento.NomeEtapa && x.Grupo == orcamento.NomeGrupo && x.Conta == orcamento.NomeItem);
                orcamento.NomeDetalhe = orcCSV.Detalhe;
                orcamento.ValorTotal = Convert.ToDecimal(orcCSV.Total);
                this.Save(orcamento);
            }

            return true;
        }
    }
}
