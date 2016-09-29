using Agnus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agnus.Domain.Models
{
    public class OrcamentoImport
    {
        public string Fase { get; set; }
        public string Etapa { get; set; }
        public string Grupo { get; set; }
        public string Conta { get; set; }
        public string Detalhe { get; set; }
        public string Total { get; set; }

        public OrcamentoImport()
        { 
        
        }

        public OrcamentoImport(string fase, string etapa, string grupo, string conta, string detalhe, string total)
        {
            this.Fase = fase;
            this.Etapa = etapa;
            this.Grupo = grupo;
            this.Conta = conta;
            this.Detalhe = detalhe;
            this.Total = total;
        }

    }
}