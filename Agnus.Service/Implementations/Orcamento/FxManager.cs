using Agnus.Domain.Models;
using Agnus.Service.DTO.Orcamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Agnus.Service.Implementations
{
    public class FxManager
    {
        private const string MATCH_VARIAVEL = @"{[0-9]+}";
        private const string MATCH_GRUPO = @"{{[0-9]+}(_{[0-9]+})*}";
        private const string MATCH_GRUPO_GRUPO = @"{{({[0-9]+}(_{[0-9]+})*)}(_({[0-9]+}|{({[0-9]+}(_{[0-9]+})*)}))*}";

        private List<int> _Niveis { get { return new List<int>() {4,5 }; } }

        internal string ConverterObjectoFormula(ProjetoOrcamento orcamento, FormulaDTO formulaDTO, ListaAdjacenciaDTO lista)
        {
            string novaExpressao = null;
            if (formulaDTO.ExpLiteral != null && formulaDTO.Vars != null)
                novaExpressao = formulaDTO.Vars.Aggregate(formulaDTO.ExpLiteral,(atualLista, item) => this.InterpretarVariavel(item.Var, item.Fator, atualLista, lista));
            else
                novaExpressao = formulaDTO.ExpLiteral;

            return novaExpressao;
        }

        private string InterpretarVariavel(string variavel, int fator, string novaExpressao, ListaAdjacenciaDTO lista)
        {
            var idItem = int.Parse(this.SepararVariaveisByPattern(variavel, @"[0-9]+").FirstOrDefault()) * fator;
            var itemLista = lista.Listas.FirstOrDefault(x => x.No.Estrutura.Id == idItem);
            var variavelConvertida = this.ConverterVariavel(lista, itemLista, itemLista.No.DirecionarFormulaPai);
            return novaExpressao.Replace(variavel, variavelConvertida);
        }

        private string ConverterVariavel(ListaAdjacenciaDTO lista, EstruturaListaAdjacencia itemLista, bool direcionarFormulaPai = false)
        {
            if (direcionarFormulaPai)
                itemLista = lista.Listas.FirstOrDefault(x => x.NosAdjacentes.Any(z => z.Estrutura.Id == itemLista.No.Estrutura.Id));
            if (itemLista.No.Estrutura.IdEntidade != default(long))
                return "{" + itemLista.No.Estrutura.IdEntidade + "}";
            else
            {
                var lItens = new List<string>();
                itemLista.NosAdjacentes.ForEach(x => lItens.Add(this.ConverterVariavel(lista, x.Estrutura.Id, false)));
                if (lItens.Count == 1)
                    lItens.Add("{0}");
                return "{" + string.Join("_", lItens) + "}";
            }
        }

        private string ConverterVariavel(ListaAdjacenciaDTO lista, long idEst, bool direcionarFormulaPai = false)
        {
            var itemLista = lista.Listas.FirstOrDefault(x => x.No.Estrutura.Id == idEst);
            return this.ConverterVariavel(lista, itemLista, direcionarFormulaPai);
        }

        internal void GerarFormulaClient(ListaAdjacenciaDTO lista)
        {
            lista.Listas.Where(x => x.No.Estrutura.Formula != null && !string.IsNullOrEmpty(x.No.Estrutura.Formula.Exp)).ToList()
                .ForEach(delegate(EstruturaListaAdjacencia est)
                {
                    var vars = this.SepararVariaveis(est.No.Estrutura.Formula.Exp);
                    est.No.Estrutura.Formula.Vars = this.TransformarVariaveis(vars, lista);

                    if (est.No.Estrutura.Formula.Vars.Count > 0)
                    {
                        for (var i = 0; i < est.No.Estrutura.Formula.Vars.Count; i++)
                        {
                            est.No.Estrutura.Formula.Exp = est.No.Estrutura.Formula.Exp.Replace(vars[i],est.No.Estrutura.Formula.Vars.Select(z => z.Var).ToList()[i]);
                            est.No.Estrutura.Formula.ExpLiteral = est.No.Estrutura.Formula.Exp.Replace(vars[i],est.No.Estrutura.Formula.Vars.Select(z => z.Var).ToList()[i]);
                        }
                    }else
                        est.No.Estrutura.Formula.ExpLiteral = est.No.Estrutura.Formula.Exp;
                });
        }

        internal List<Scope> TransformarVariaveis(List<string> vars, ListaAdjacenciaDTO lista)
        {            
            var varsT = new List<Scope>();
            vars.ForEach(delegate(string vId)
            {                
                var itemLista = this.GetItemLista(vId, lista);
                var fator = itemLista.No.Estrutura.IdEntidade != default(long) ? (1) : (-1);
                varsT.Add(new Scope { Var = string.Format("_v{0}__", (itemLista.No.Estrutura.Id * fator)), Fator = fator });
            });
            return varsT;
        }


        private EstruturaListaAdjacencia GetItemLista(string variavel, ListaAdjacenciaDTO lista)
        {
            EstruturaListaAdjacencia item = null;
            if (Regex.Replace(variavel, MATCH_VARIAVEL, string.Empty) == string.Empty)
                item = lista.Listas.FirstOrDefault(x => x.No.Estrutura.Id.ToString() == Regex.Replace(variavel, @"[^0-9]", string.Empty));
            else if (Regex.Replace(variavel, MATCH_GRUPO, string.Empty) == string.Empty)
                item = lista.Listas.FirstOrDefault(x => x.NosAdjacentes.Any(z => variavel.Replace("{", "").Replace("}", "").Split('_').Contains(z.Estrutura.Id.ToString())));
            else if (Regex.Replace(variavel, MATCH_GRUPO_GRUPO, string.Empty) == string.Empty)
            {
                var varsSub = this.GetSubs(variavel);
                var itensSub = varsSub.Select(v => this.GetItemLista(v, lista)).ToList();
                item = lista.Listas.FirstOrDefault(x => x.NosAdjacentes.Any(z => itensSub.Select(w => w.No.Estrutura.Id).Contains(z.Estrutura.Id)));
            }
            return item;
        }

        

        private List<string> GetSubs(string variavel)
        {
            var subs = new List<string>();
            foreach (Match g in Regex.Matches(variavel, MATCH_GRUPO))
                subs.Add(g.Value);
            foreach (var s in subs)
                variavel = variavel.Replace(s, string.Empty);
            foreach (Match g in Regex.Matches(variavel, MATCH_VARIAVEL))
                subs.Add(g.Value);
            return subs;
        }

        internal List<string> SepararVariaveis(string textoFormula)
        {
            var variaveis = new List<string>();
            variaveis.AddRange(this.SepararVariaveisByPattern(textoFormula, MATCH_GRUPO_GRUPO));
            textoFormula = this.RemoverGrupo(textoFormula, MATCH_GRUPO_GRUPO);
            variaveis.AddRange(this.SepararVariaveisByPattern(textoFormula, MATCH_GRUPO));
            textoFormula = this.RemoverGrupo(textoFormula, MATCH_GRUPO);
            variaveis.AddRange(this.SepararVariaveisByPattern(textoFormula, MATCH_VARIAVEL));
            return variaveis;
        }

        private string RemoverGrupo(string textoFormula, string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            return regex.Replace(textoFormula, "");
        }

        private IEnumerable<string> SepararVariaveisByPattern(string textoFormula, string pattern)
        {
            var variaveis = new List<string>();
            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            var matchs = regex.Matches(textoFormula);
            foreach (Match match in matchs)
                variaveis.Add(match.Value);

            return variaveis;
        }
       
        internal string TransformarGrupoSomatorio(string textoFormula)
        {
            var vars = this.SepararVariaveisByPattern(textoFormula, MATCH_GRUPO);
            if (vars.Any())
                foreach (var v in vars)
                {
                    var vIn = this.SepararVariaveisByPattern(v, MATCH_VARIAVEL);
                    var strNovaParte = string.Join(" + ", vIn);
                    textoFormula = textoFormula.Replace(v, strNovaParte);
                }
            return textoFormula;
        }
    }
}
