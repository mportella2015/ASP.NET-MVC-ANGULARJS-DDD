using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain
{
    public interface ILogin
    {
        string DisplayName { get; }
        string Username { get; }
        object Identifier { get; }
        bool FullAccess { get; }        
    }
}
