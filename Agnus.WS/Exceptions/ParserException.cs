using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.WS.Exceptions
{
    public class ParserException : Exception
    {
        public ParserException(string mensagem)
            :base(mensagem)
        {            
        }

        public ParserException()
        {
        }
    }
}
