using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Service
{
    public class CustomServiceException : Exception
    {
        public CustomServiceException(string message) : base(message)
        {
            
        }
    }
}
