using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models.Enum
{
    public enum OrcamentoNivelPermissaoEnum
    {
        REAL_VISUALIZAR = 1,
        REAL_EDITAR = 2,
        REAL_POR_DATA_CRIAR_DISTRIBUICAO = 3,
        REAL_POR_DATA_EDITAR_DISTRIBUICAO = 4,
        REAL_POR_DATA_VISUALIZAR = 5,
        PRODUTOR_LIBERACAO_ORCAMENTO = 6,
        PRODUTOR_ALTERAR_ORCAMENTO = 7,
        PRODUTOR_VIZUALIZACAO = 8
    }
}
