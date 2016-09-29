using Agnus.Domain.Models;
using Agnus.Service.DTO.Orcamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agnus.Framework.Extensions;
using Agnus.Framework.Helper;

namespace Agnus.Service.Implementations
{
    public class ListaAdjacenciaManager
    {
        long countId = 0;
        long id = 0;
        public ListaAdjacenciaDTO CriarListaAdjacencia<T>(List<T> itens)
        {
            var listaNos = this.CriarNos(itens);
            var lista = this.CriarListaAdjacencia(listaNos);
            this.AtualizarIds(lista);
            this.BindFormulas(lista);
            return lista;
        }

        private void BindFormulas(ListaAdjacenciaDTO lista)
        {
            var fxManager = new FxManager();
            fxManager.GerarFormulaClient(lista);
        }

        private void AtualizarIds(ListaAdjacenciaDTO lista)
        {
            //for (int i = lista.Listas.Count - 1; i >= 0; i--)
            //    lista.Listas[i].No.Estrutura.Id = this.GerarIdItemLista(lista.Listas[i]);
            for (int i = 0; i < lista.Listas.Count; i++)
                lista.Listas[i].No.Estrutura.Id = this.GerarIdItemLista(lista.Listas[i]);
        }

        internal long GerarIdItemLista(EstruturaListaAdjacencia est)
        {
            if (est.NosAdjacentes.Count > 0)
                return --this.id;//this.GerarIdItemLista(est.NosAdjacentes);
            else
                return est.No.Estrutura.IdEntidade;
        }

        internal long GerarIdItemLista(List<NoArvoreOrcamentoDTO> nos)
        {
            return 100 + nos.Sum(z => z.Estrutura.Id);
        }

        private ListaAdjacenciaDTO CriarListaAdjacencia(List<NoArvoreOrcamentoDTO> listaNos)
        {
            var listaAdjacencia = new ListaAdjacenciaDTO();
            var estrutura = new EstruturaListaAdjacencia();
            int itemLista = 0;

            estrutura.No = this.CriarNoRaiz();
            listaAdjacencia.Listas.Add(estrutura);
            while (true)
            {
                estrutura.NosAdjacentes = this.PegarNosAdjacentes(listaNos, estrutura.No.Token);
                this.AdicionarNovosNosALista(listaAdjacencia, listaAdjacencia.Listas[itemLista].NosAdjacentes);
                itemLista++;
                if (itemLista == listaAdjacencia.Listas.Count)
                    break;
                estrutura = listaAdjacencia.Listas[itemLista];
            }
            return listaAdjacencia;
        }

        private void AdicionarNovosNosALista(ListaAdjacenciaDTO listaAdjacencia, List<NoArvoreOrcamentoDTO> list)
        {
            foreach (var item in list.Where(x => !x.IncluidoNaLista))
            {
                item.IncluidoNaLista = true;
                listaAdjacencia.Listas.Add(new EstruturaListaAdjacencia { No = item });
            }
        }

        private List<NoArvoreOrcamentoDTO> PegarNosAdjacentes(List<NoArvoreOrcamentoDTO> listaNos, string token)
        {
            return listaNos.Where(x => x.TokenPai == token).ToList();
        }

        private IEnumerable<NoArvoreOrcamentoDTO> CriarNosPorItem(dynamic item)
        {
            var listaNos = new List<NoArvoreOrcamentoDTO>();
            var manutencao = this.ConstruirNomeManutencao(item);

            var noFase = new NoArvoreOrcamentoDTO()
            {
                Estrutura = new EstruturaDados() { Nome = item.NomeFase },
                TokenPai = "ORCAMENTO",
                Nivel = 2
            };
            listaNos.Add(noFase);
            var noEtapa = new NoArvoreOrcamentoDTO()
            {
                Estrutura = new EstruturaDados() { Nome = item.NomeEtapa },
                TokenPai = noFase.Token,
                Nivel = 3
            };
            listaNos.Add(noEtapa);

            var noGrupo = new NoArvoreOrcamentoDTO()
            {
                Estrutura = new EstruturaDados() { Nome = item.NomeGrupo },
                TokenPai = noEtapa.Token,
                Nivel = 4
            };
            listaNos.Add(noGrupo);

            NoArvoreOrcamentoDTO noConta;

            if (((object)item).GetType().GetProperty("NomeDetalhe") != null && !string.IsNullOrEmpty(item.NomeDetalhe))
            {
                noConta = new NoArvoreOrcamentoDTO()
                {
                    Estrutura = new EstruturaDados() { Nome = item.NomeItem },
                    TokenPai = noGrupo.Token,
                    EhConta = true,
                    Nivel = 5,
                    IdVirtualSubstituicao = item.IdVirtualSubstituicao
                };
                listaNos.Add(noConta);

                var noDetalhe = new NoArvoreOrcamentoDTO()
                {
                    Estrutura = new EstruturaDados() { IdEntidade = item.Id, Nome = item.NomeDetalhe + manutencao, Valor = item.ValorTotal, Formula = new FormulaDTO { Exp = item.TxtFormulaCalculo }, Observacao = item.TxtObservacao },
                    TokenPai = noConta.Token,
                    Nivel = 6,
                    DirecionarFormulaPai = item.DirecionarFormulaPai,
                    IsNew = item.IsNew,
                    IdVirtualSubstituicao = item.IdVirtualSubstituicao
                };
                listaNos.Add(noDetalhe);
            }
            else
            {
                noConta = new NoArvoreOrcamentoDTO()
                {
                    Estrutura = new EstruturaDados()
                    {
                        IdEntidade = ((object)item).GetType().GetProperty("ItemOrcamento") != null ? item.ItemOrcamento.Id : item.Id,
                        Nome = item.NomeItem + manutencao,
                        Valor = item.ValorTotal,
                        Formula = ((object)item).GetType().GetProperty("NomeDetalhe") != null ? new FormulaDTO { Exp = item.TxtFormulaCalculo } : null,
                        Observacao = ((object)item).GetType().GetProperty("TxtObservacao") != null ? item.TxtObservacao : null
                    },
                    TokenPai = noGrupo.Token,
                    EhConta = true,
                    Nivel = 5
                };
                listaNos.Add(noConta);
            }
            return listaNos;

        }

        private string ConstruirNomeManutencao(dynamic item)
        {
            if (item is OrcamentoTotal)
            {
                var grupoAprovacao = ((OrcamentoTotal)item).GrupoAprovacao;
                if (grupoAprovacao != null && grupoAprovacao.TipoOrcamento.Codigo == (int)Agnus.Domain.Models.Enum.TipoOrcamentoEnum.Manutencao)
                    return string.Format(" manutenção#{0}", grupoAprovacao.DataStatusOrcamento.ToString());
            }
            return string.Empty;
        }

        private List<NoArvoreOrcamentoDTO> CriarNos<T>(List<T> itens)
        {
            try
            {
                var listaNos = new List<NoArvoreOrcamentoDTO>();
                foreach (var item in itens)
                    listaNos.AddRange(this.CriarNosPorItem(item));
                return listaNos.DistinctBy(x => x.Token).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Objeto não é apropriado para gerar a arvore. Você deve utilizar os tipos: OrcamentoTotal e OrcamentoAtual");
            }
        }

        private NoArvoreOrcamentoDTO CriarNoRaiz()
        {
            return new NoArvoreOrcamentoDTO
            {
                TokenPai = string.Empty,
                Estrutura = new EstruturaDados
                {
                    Nome = "ORCAMENTO",
                    Id = ++this.countId
                },
                Nivel = 1
            };
        }


        internal object BindPorPeriodo(ListaAdjacenciaDTO listaAdjacencia, ProjetoOrcamento orcamento, List<string> nomesPeriodo)
        {
            this.BindPeriodo(listaAdjacencia, orcamento, nomesPeriodo, this.IncluirValoresOrcamentoPeriodo);
            return null;
        }

        private bool IncluirValoresOrcamentoPeriodo(ProjetoOrcamento orcamento, EstruturaDados estruturaDados)
        {
            var orcamentoTotal = orcamento.OrcamentoTotal.FirstOrDefault(x => x.Id == estruturaDados.IdEntidade);
            foreach (var item in estruturaDados.Meses)
            {
                var data = Util.ConverterStringDataPraDatetime(item);
                var orcamentoPeriodo = orcamentoTotal.OrcamentoPeriodo.FirstOrDefault(x => x.NumMes == data.Month && x.NumAno == data.Year);
                estruturaDados.ValoresMeses.Add(orcamentoPeriodo == null ? 0 : orcamentoPeriodo.ValorItem);
                estruturaDados.PercentualMeses.Add(orcamentoPeriodo != null && orcamentoPeriodo.Percentual.HasValue ? orcamentoPeriodo.Percentual.Value : 0);
            }
            return true;
        }

        internal object BindProdutorPorPeriodo(ListaAdjacenciaDTO listaAdjacencia, ProjetoOrcamento orcamento, List<string> nomesPeriodo)
        {
            this.BindPeriodo(listaAdjacencia, orcamento, nomesPeriodo, this.IncluirValoresProdutorOrcamentoPeriodo);
            return null;
        }

        private bool IncluirValoresProdutorOrcamentoPeriodo(ProjetoOrcamento orcamento, EstruturaDados estruturaDados)
        {
            var itemOrcamento = orcamento.ItemOrcamento.FirstOrDefault(x => x.Id == estruturaDados.IdEntidade);
            var orcamentoAtualTotal = itemOrcamento.OrcamentoAtual.FirstOrDefault(x => x.NumAno == null && x.NumMes == null);
            foreach (var item in estruturaDados.Meses)
            {
                var data = Util.ConverterStringDataPraDatetime(item);
                var orcamentoAtual = itemOrcamento.OrcamentoAtual.FirstOrDefault(x => x.NumMes == data.Month && x.NumAno == data.Year);
                estruturaDados.ValoresProdutor.Add(orcamentoAtual == null ? 0 : orcamentoAtual.ValorItemProdutor);
                estruturaDados.ValoresMeses.Add(orcamentoAtual == null ? 0 : orcamentoAtual.ValorItemReal);
                estruturaDados.ValorProdutor = orcamentoAtualTotal.ValorItemProdutor;
            }
            estruturaDados.PercentualProdutor = itemOrcamento.Percentual;
            return true;
        }

        private void BindPeriodo(ListaAdjacenciaDTO listaAdjacencia, ProjetoOrcamento orcamento, List<string> nomesPeriodo, Func<ProjetoOrcamento, EstruturaDados, bool> func)
        {
            var nosFolha = listaAdjacencia.Listas.Where(z => !z.NosAdjacentes.Any());
            var itensRemove = new List<long>();
            foreach (var no in nosFolha)
            {
                no.No.Estrutura.Meses = nomesPeriodo;
                no.No.Estrutura.Formula = null;
                func(orcamento, no.No.Estrutura);
            }
            listaAdjacencia.Listas[0].No.Estrutura.Meses = nomesPeriodo;
        }

        internal void AdicionarAlteracoes(ProjetoOrcamento orcamento, ListaAdjacenciaDTO grafo, IEnumerable<OrcamentoTotal> alteracoes)
        {
            foreach (var alteracao in alteracoes)
            {
                var similar = alteracao.GetSimilar();
                var vertice = grafo.Listas.FirstOrDefault(x => x.No.Estrutura.Id == similar.Id);
                foreach (var op in alteracao.OrcamentoPeriodo)
                    vertice.No.Estrutura.ValoresProdutor.Add(op.ValorItem);
                if (similar.OrcamentoPeriodo.Count > alteracao.OrcamentoPeriodo.Count)
                    for (int i = alteracao.OrcamentoPeriodo.Count; i < similar.OrcamentoPeriodo.Count; i++)
                        vertice.No.Estrutura.ValoresProdutor.Add(0);
            }
        }
    }
}
