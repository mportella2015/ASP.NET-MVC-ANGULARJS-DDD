using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agnus.Domain.Models;

namespace Agnus.Domain
{
    public interface ILogFornecedor
    {
        long IdFornecedor { get; }
        string NomeEntidade { get; }
    }
}
