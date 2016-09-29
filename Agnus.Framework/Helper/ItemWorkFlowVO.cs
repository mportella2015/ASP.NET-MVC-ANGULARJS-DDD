using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Framework.Helper
{
    public class ItemWorkFlowVO
    {
        public long? IdWf { get; set; }
        public int Nivel { get; set; }
        public int UltimoNivel { get; set; } 
        public long IdObjAprovacao { get; set; }
        public int ValorAprovacao { get; set; } 
        public bool IsAlcadaDireta { get; set; }
        public int UltimaAlcada { get; set; }
        public long? IdProjeto { get; set; }
        public long? IdNucleo { get; set; }
        public long? IdCentroCusto { get; set; }
        public long? IdParecerAprovacao { get; set; }
        public DateTime? DataParecer { get; set; }
        public decimal? QtdTempoAprovacao { get; set; }
        public string TxtJustificativa { get; set; }
        public bool IsProjetoFundo { get; set; }
        public List<int> PassosWorkflowList { get; set; }
        public bool HasStatusRevisao { get; set; }
        public long? IdAprovador;
    }
}
