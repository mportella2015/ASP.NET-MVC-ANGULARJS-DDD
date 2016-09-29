using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models.Relatorio
{
    public class AbaExcel
    {
        public string Nome { get; set; }

        public int IndexV { get; set; }

        public int IndexH { get; set; }

        public List<Celula> Celulas { get; set; }

        public int IndexVDisponivel
        {
            get
            {
                var ultimaCelulaIndex = this.Celulas.OrderBy(x => x.IndexV).LastOrDefault();
                return ultimaCelulaIndex != null ? ultimaCelulaIndex.IndexV + 1 : this.IndexV + 1;
            }
        }

        public int IndexHDisponivel
        {
            get
            {
                var ultimaCelulaIndex = this.Celulas.OrderBy(x => x.IndexH).LastOrDefault();
                return ultimaCelulaIndex != null ? ultimaCelulaIndex.IndexH + 1 : this.IndexH + 1;
            }
        }


        public AbaExcel()
        {

            this.Celulas = new List<Celula>();
        }

        public AbaExcel(string nome, int indexV, int indexH)
        {
            this.Nome = nome;
            this.IndexH = indexH;
            this.IndexV = indexV;
            this.Celulas = new List<Celula>();
        }

        public Celula AdicionarCelula(string id, string valor, int indexV, int indexH, bool borda = false, bool header = false, int range = 1, int tamanhoLetra = 0, bool autoAjuste = false, bool inivisivel = false)
        {
            return new Celula(this, id, valor, indexV, indexH, borda, header: header, range: range, tamanhoLetra: tamanhoLetra, autoAjuste: autoAjuste, invisivel: inivisivel);
        }       
    }
}
