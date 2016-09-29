using Agnus.Domain.Models;
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
    public class EstoqueCartaoService : EntityService<EstoqueCartao>, IEstoqueCartaoService
    {
        public EstoqueCartaoService(IUnitOfWork unitOfWork, IGenericRepository<EstoqueCartao> repository)
            : base(unitOfWork, repository)
        {
        }

        /// <summary>
        /// Na inclusão do cartão o estoque da modalidade deverá ser decrescido de 1
        /// </summary>
        /// <param name="id">IdModalidadeCartao</param>
        /// <returns></returns>
        public void DecrementarEstoqueCartao(long idModalidadeCartao)
        {
            var estoqueCartao = GetAllByFilter(x => x.IdModalidadeCartao == idModalidadeCartao).FirstOrDefault();

            if (estoqueCartao != null)
            {
                int qtdCartao = estoqueCartao.QtdCartao;

                if (qtdCartao != 0)
                {
                    estoqueCartao.QtdCartao = qtdCartao - 1;

                    Save(estoqueCartao);
                    _unitOfWork.Commit();
                }
            }
        }
    }
}
