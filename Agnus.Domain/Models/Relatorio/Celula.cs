using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models.Relatorio
{
    public class Celula
    {
        public string Valor { get; set; }

        public int IndexV { get; set; }

        public int IndexH { get; set; }

        public Celula CelulaPai { get; set; }

        public List<Celula> Celulas { get; set; }

        public bool Borda { get; set; }

        public bool Merge { get; set; }

        public AbaExcel Aba { get; set; }

        public int Range { get; set; }

        public bool Header { get; set; }

        public string Id { get; set; }

        public int StartIndexV { get; set; }

        public int StartIndexH { get; set; }

        public TipoCelula TipoCelula { get; set; }

        public int TamanhoLetra { get; set; }

        public bool AutoAjuste { get; set; }

        public bool Invisivel { get; set; }

        public int UltimoIndexHFilhas
        {
            get
            {
                return this.Celulas.Any() ? this.Celulas.OrderBy(x => x.IndexH).LastOrDefault().IndexH : this.IndexH;
            }
        }

        public int UltimoIndexVFilhas
        {
            get
            {
                return this.Celulas.Any() ? this.Celulas.OrderBy(x => x.IndexV).LastOrDefault().IndexV : this.IndexV;
            }
        }

        public Celula()
        {

        }

        public Celula(AbaExcel aba, string id, string valor, int indexV, int indexH, bool borda = false, int range = 1, bool header = false, TipoCelula tipoCelula = Relatorio.TipoCelula.Texto, int tamanhoLetra = 0, bool autoAjuste = false, bool invisivel = false)
        {
            this.Valor = valor;
            this.IndexV = indexV;
            this.IndexH = indexH;
            this.Borda = borda;
            this.Range = range;
            this.Celulas = new List<Celula>();
            this.Header = header;
            this.Aba = aba;
            aba.Celulas.Add(this);
            this.Id = id;
            this.TipoCelula = tipoCelula;
            this.TamanhoLetra = tamanhoLetra;
            this.AutoAjuste = autoAjuste;
            this.Invisivel = invisivel;
        }

        public Celula AdicionarCelulaFilha(string id, string valor, PosicaoCelula posicao, TipoCelula tipoCelula = Relatorio.TipoCelula.Texto, int incrementoPosicao = 0, bool celulaIsoladaH = false, bool borda = false, bool header = false, bool autoAjuste = false)
        {
            int indexH = this.IndexH;
            int indexV = this.IndexV;

            if (posicao == PosicaoCelula.Horizontal)
            {
                indexV = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexV).LastOrDefault().IndexV + 1 : this.IndexV + 1;
                if (incrementoPosicao != 0)
                    indexV += incrementoPosicao;
            }

            if (posicao == PosicaoCelula.Vertical)
            {
                indexH = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexH).LastOrDefault().IndexH + 1 : this.IndexH + 1;
                if (incrementoPosicao != 0)
                    indexH += incrementoPosicao;
            }

            if (posicao == PosicaoCelula.Diagonal)
            {
                indexH = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexH).LastOrDefault().IndexH + 1 : this.IndexH + 1;
                indexV = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexV).LastOrDefault().IndexV + 1 : this.IndexV + 1;
            }

            if (posicao == PosicaoCelula.VerticalHorizontal)
            {
                indexH = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexH).LastOrDefault().IndexH + 1 : this.IndexH + 1;
                indexV = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexV).LastOrDefault().IndexV : this.IndexV + 1;
            }

            if (posicao == PosicaoCelula.HorizontalVertical)
            {
                indexH = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexH).LastOrDefault().IndexH : this.IndexH + 1;
                indexV = this.Celulas.Any() ? this.Celulas.Where(x => !x.Invisivel).OrderBy(x => x.IndexV).LastOrDefault().IndexV + 1 : this.IndexV;
            }

            if (celulaIsoladaH)
            {
                var isolada = false;

                while (!isolada)
                {
                    if (this.Aba.Celulas.Any(x => x.IndexH == indexH))
                        indexH++;
                    else
                        isolada = true;
                }
            }

            if (this.StartIndexV != 0)
                indexV = this.StartIndexV;

            var celula = new Celula(this.Aba, id, valor, indexV, indexH, borda);
            celula.TipoCelula = tipoCelula;
            celula.CelulaPai = this;
            celula.Header = header;
            celula.AutoAjuste = autoAjuste;
            this.Celulas.Add(celula);

            return celula;
        }

        public Celula AdicionarCelulaFilha(string id, string valor, int indexV, int indexH, TipoCelula tipoCelula = TipoCelula.Texto, bool celulaIsoladaH = false, bool borda = false, bool header = false, bool invisivel = false)
        {
            var celula = new Celula(this.Aba, id, valor, indexV, indexH, borda, invisivel: invisivel);
            celula.CelulaPai = this;
            celula.TipoCelula = tipoCelula;
            this.Celulas.Add(celula);

            return celula;
        }

        public void ExecutarMerge()
        {
            this.Merge = true;
            if (this.Range == 1)
                Range = this.Celulas.Any() ? this.Celulas.OrderBy(x => x.IndexV).LastOrDefault().IndexV - this.IndexV : 1;

        }

        public void AdicionarCelulasRangeVertical(int indexHFinal, string id, string valor, int indexV, int indexH, bool borda = false, bool header = false, int range = 1, int tamanhoLetra = 0, bool autoAjuste = false, bool inivisivel = false, TipoCelula tipoCelula = Relatorio.TipoCelula.Texto)
        {
            for (var i = indexH; i <= indexHFinal; i++)
            {
                if (this.Aba.Celulas.Any(x => x.IndexH == i && x.IndexV == indexV))
                    continue;

                var cel = new Celula(this.Aba, id, valor, indexV, i, borda, header: header, range: range, tamanhoLetra: tamanhoLetra, autoAjuste: autoAjuste, invisivel: inivisivel,tipoCelula:tipoCelula);
            }
        }
    }

    public enum PosicaoCelula
    {
        Horizontal,
        Vertical,
        Diagonal,
        VerticalHorizontal,
        HorizontalVertical
    }


    public enum TipoCelula
    {
        Decimal,
        Texto
    }
}
