using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Quality.SIGP2.Framework.Authentication
{
    public enum StatusTokenError
    {
        [Description("Token inválido")]
        InvalidToken = 1,
        [Description("Acesso não permitido")]
        AccessDenied = 2,
        [Description("Desconhecido")]
        Unknown = 3,

    }
}