using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using Agnus.Service;
using Agnus.Domain.Models;
using Agnus.Service.Implementations;
using Agnus.Service.Interfaces;
using Agnus.Domain.Models.Relatorio;
using Agnus.Reporting.ModelsReporting;

namespace Agnus.Reporting
{
    public class RelatorioAcompanhamentoOrcamentoProdutor : RelatorioExcelBase
    {
        private IEntityService<ProjetoOrcamento> _service;
        private IEntityService<OrcamentoAtual> _serviceAtual;
        private IOrcamentoService _serviceOrgamento;
        public long _idProduto { get; set; }
        public bool _detalhaComprometido { get; set; }

        public RelatorioAcompanhamentoOrcamentoProdutor(IEntityService<ProjetoOrcamento> service, IEntityService<OrcamentoAtual> serviceOrcamentoAtual, long idProduto, bool detalhaComprometido, IOrcamentoService serviceOrcamento)
        {
            _service = service;
            _idProduto = idProduto;
            _detalhaComprometido = detalhaComprometido;
            _serviceOrgamento = serviceOrcamento;
            _serviceAtual = serviceOrcamentoAtual;
        }

        public override byte[] GenerateReport()
        {
            using (var pck = new ExcelPackage())
            {
                var projetoOrcamento = _service.GetAllByFilter(x => x.IdProjeto == _idProduto && x.DataAprovacao.HasValue);// && x.DataAprovacao.HasValue).FirstOrDefault();

                var orcamentosAtuais = _serviceAtual.GetAllByFilter(x => projetoOrcamento.Any(y => y.Id == x.IdProjetoOrcamento)).Select(x => new OrcamentoVO()
                {
                    Id = x.Id,
                    Data = string.Concat(x.NumMes, "/", x.NumAno),
                    ValorLiberado = x.ValorItemProdutor,
                    ValorComprometido = x.ValorUsoProdutor,
                    Saldo = x.ValorItemProdutor - x.ValorUsoProdutor,
                    NomeItemOrcamento = x.NomeItem,
                    Fase = x.NomeFase,
                    Etapa = x.NomeEtapa,
                    Grupo = x.NomeGrupo,
                    Token = string.Concat(x.NomeFase, ">", x.NomeEtapa, ">", x.NomeGrupo, ">", x.NomeItem),
                    FaseEtapaGrupo = string.Concat(x.NomeFase, ">", x.NomeEtapa, ">", x.NomeGrupo)
                });

                var abaResumo = GerarAbaResumo(pck, orcamentosAtuais);

                if (_detalhaComprometido)
                {
                    var orcamentos = _serviceAtual.GetAllByFilter(x => projetoOrcamento.Any(y => y.Id == x.IdProjetoOrcamento));
                    var detalhes = orcamentos.ToList().Select(x => new OrcamentoVO()
              {
                  Data = string.Concat(x.NumMes, "/", x.NumAno),
                  Objetos = x.OrcamentoProdutorUso.Any() ? x.OrcamentoProdutorUso.Select(y => new ObjetoOrcamento(y.ValorUso, y.ObjetoComprometimentoOrcamento.Texto)).ToList() : new List<ObjetoOrcamento>(),
                  NomeItemOrcamento = x.NomeItem,
                  Fase = x.NomeFase,
                  Etapa = x.NomeEtapa,
                  Grupo = x.NomeGrupo,
                  Token = string.Concat(x.NomeFase, ">", x.NomeEtapa, ">", x.NomeGrupo, ">", x.NomeItem),
                  FaseEtapaGrupo = string.Concat(x.NomeFase, ">", x.NomeEtapa, ">", x.NomeGrupo),
                  ValorUso = x.ValorUsoReal
              });

                    var abaDetalhe = GerarAbaDetalhe(pck, detalhes);
                }

                #region Comentarios
                //ExcelWorksheet abaResumo = this.CreateWorksheet(pck, "Resumo");

                //ws.Cells[1, 1, 1, 10].Merge = true; //Merge columns start and end range
                //ws.Cells[1, 1, 1, 10].Style.Font.Bold = true; //Font should be bold
                //ws.Cells[1, 1, 1, 10].Style.Font.Size = 13;
                //Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D1F5C6");
                //ws.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                //ws.Cells[1, 1, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center


                //int contadorLinhas = 7;
                //contadorLinhas = ws.WriteTitle(this.ReportTitle, contadorLinhas, numeroColunas);
                //if (reportData.Filter != null)
                //    contadorLinhas = ws.WriteFiltro(reportData, contadorLinhas, numeroColunas);
                //var range = WriteData(ws, contadorLinhas);
                ////ws.AutoFitColumns(numeroColunas);
                //range.Style.WrapText = true;
                //range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //CreateTable(ws, range);
                #endregion

                return pck.GetAsByteArray();
            }
        }

        private ExcelWorksheet GerarAbaResumo(ExcelPackage pck, IEnumerable<OrcamentoVO> dataSource)
        {
            var aba = new AbaExcel("Resumo", 1, 5);
            var fases = aba.AdicionarCelula("", "Fase", 1, 5, true, true);
            var etapas = aba.AdicionarCelula("", "Etapa", 2, 5, true, true);
            var grupos = aba.AdicionarCelula("", "Grupo", 3, 5, true, true);
            var itens = aba.AdicionarCelula("", "Itens", 4, 5, true, true);
            //var totalGeral = aba.AdicionarCelula("", "Total", 5, 5, true, true,inivisivel:true);
            var totalCalculado = aba.AdicionarCelula("", "Total Calculado", 5, 4, true, true);
            var liberado = totalCalculado.AdicionarCelulaFilha(string.Empty, "Liberado", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var comprometido = totalCalculado.AdicionarCelulaFilha(string.Empty, "Comprometido", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var saldo = totalCalculado.AdicionarCelulaFilha(string.Empty, "Saldo", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);

            totalCalculado.ExecutarMerge();

            var countData = aba.IndexVDisponivel;

            var dataSourceGroup = dataSource.OrderBy(x => x.Data).GroupBy(g => g.FaseEtapaGrupo)
                         .Select(g => g)
                         .ToList();

            foreach (var orc in dataSourceGroup)
            {
                var itensOrc = orc.Select(x => x);

                var nomeFase = orc.FirstOrDefault().Fase;
                var nomeEtapa = orc.FirstOrDefault().Etapa;
                var nomeGrupo = orc.FirstOrDefault().Grupo;

                var fase = fases.Celulas.FirstOrDefault(x => x.Valor == nomeFase);
                if (fase == null)
                {
                    fase = fases.AdicionarCelulaFilha(nomeFase, nomeFase, PosicaoCelula.Vertical, celulaIsoladaH: true);
                    fases.Celulas.Add(fase);
                }



                var etapa = etapas.Celulas.FirstOrDefault(x => x.Valor == nomeEtapa);
                if (etapa == null)
                {
                    var ultimaEtapa = fase.Celulas.LastOrDefault(x => x.Id == "ETAPA");
                    if (ultimaEtapa != null && ultimaEtapa.Celulas.Any())
                        etapa = fase.AdicionarCelulaFilha("ETAPA", nomeEtapa, ultimaEtapa.IndexV, ultimaEtapa.UltimoIndexHFilhas + 1, celulaIsoladaH: false);
                    else
                        etapa = fase.AdicionarCelulaFilha("ETAPA", nomeEtapa, PosicaoCelula.Diagonal, celulaIsoladaH: false);

                    etapas.Celulas.Add(etapa);


                }



                var grupo = grupos.Celulas.FirstOrDefault(x => x.Valor == nomeGrupo);
                if (grupo == null)
                {
                    var ultimoGrupo = etapa.Celulas.LastOrDefault(x => x.Id == "GRUPO");
                    if (ultimoGrupo != null && ultimoGrupo.Celulas.Any())
                        grupo = etapa.AdicionarCelulaFilha("GRUPO", nomeGrupo, ultimoGrupo.IndexV, ultimoGrupo.UltimoIndexHFilhas + 1, celulaIsoladaH: false);
                    else
                        grupo = etapa.AdicionarCelulaFilha("GRUPO", nomeGrupo, PosicaoCelula.Diagonal, celulaIsoladaH: false);

                    grupos.Celulas.Add(grupo);
                }

                fase.StartIndexV = 5;
                etapa.StartIndexV = 5;




                liberado.AdicionarCelulaFilha("TOTALLIBERADOGRUPO", itensOrc.Where(x => x.Grupo == nomeGrupo && x.Data != "/").Select(x => x.ValorLiberado).Aggregate<decimal>((a, b) => a + b).ToString(), liberado.IndexV, grupo.IndexH, tipoCelula: TipoCelula.Decimal);
                comprometido.AdicionarCelulaFilha("TOTALCOMPROMETIDOGRUPO", itensOrc.Where(x => x.Grupo == nomeGrupo && x.Data != "/").Select(x => x.ValorComprometido).Aggregate<decimal>((a, b) => a + b).ToString(), comprometido.IndexV, grupo.IndexH, tipoCelula: TipoCelula.Decimal);
                saldo.AdicionarCelulaFilha("TOTALSALDOGRUPO", itensOrc.Where(x => x.Grupo == nomeGrupo && x.Data != "/").Select(x => x.Saldo).Aggregate<decimal>((a, b) => a + b).ToString(), saldo.IndexV, grupo.IndexH, tipoCelula: TipoCelula.Decimal);


                var itensPorData = itensOrc.GroupBy(g => g.Data)
                     .Select(g => g)
                     .ToList();

                foreach (var item in itensPorData)
                {

                    var itensOrcData = item.Select(x => x);

                    foreach (var itemData in item)
                    {
                        if (itemData.Data == "/")
                            continue;

                        var itemGrupo = grupo.Celulas.FirstOrDefault(x => x.Valor == itemData.NomeItemOrcamento);
                        var data = aba.Celulas.FirstOrDefault(x => x.Valor == itemData.Data);

                        if (itemGrupo == null)
                        {
                            itemGrupo = grupo.AdicionarCelulaFilha("ITEM", itemData.NomeItemOrcamento, PosicaoCelula.VerticalHorizontal);
                            GerarTotal(itemGrupo, itemData.NomeItemOrcamento, itensOrc.Where(x => x.FaseEtapaGrupo == itemData.FaseEtapaGrupo && x.NomeItemOrcamento == itemData.NomeItemOrcamento && x.Data != "/"));
                        }


                        if (data == null)
                        {
                            var ultimaData = aba.Celulas.LastOrDefault(x => x.Id == "DATA");
                            var indexVData = ultimaData != null ? ultimaData.IndexV + 1 : aba.IndexVDisponivel;

                            data = aba.AdicionarCelula("DATA", itemData.Data, indexVData, totalCalculado.IndexH, true, true);
                            data.AdicionarCelulaFilha("DATA", "Liberado", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
                            data.AdicionarCelulaFilha("DATA", "Comprometido", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
                            data.AdicionarCelulaFilha("DATA", "Saldo", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
                            //GerarTotal(etapa, "NADA", itensOrcData.Where(x => x.Etapa == nomeEtapa && x.Data == itemData.Data && x.Data != "/"));
                            data.ExecutarMerge();
                        }

                        GerarTotalCondicao(fase, itemData.Fase + "-TOTAL", dataSource.Where(x => x.Fase == nomeFase && x.Data != "/"));
                        GerarTotalCondicao(etapa, itemData.Etapa + "-TOTAL", dataSource.Where(x => x.Etapa == nomeEtapa && x.Data != "/"));

                        fase.StartIndexV = 0;
                        etapa.StartIndexV = 0;

                        GerarTotalCondicao(fase, itemData.Fase + "/" + itemData.Data, dataSource.Where(x => x.Fase == nomeFase && x.Data == itemData.Data && x.Data != "/"));
                        GerarTotalCondicao(etapa, itemData.Etapa + "/" + itemData.Data, dataSource.Where(x => x.Etapa == nomeEtapa && x.Data == itemData.Data && x.Data != "/"));


                        var valorLiberado = itemGrupo.AdicionarCelulaFilha("LIBERADO", itemData.ValorLiberado.ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);

                        liberado.AdicionarCelulaFilha("TOTALLIBERADOGRUPO", itensOrc.Where(x => x.Grupo == itemData.Grupo && x.Data == itemData.Data && x.Data != "/").Select(x => x.ValorLiberado).Aggregate<decimal>((a, b) => a + b).ToString(), valorLiberado.IndexV, grupo.IndexH, tipoCelula: TipoCelula.Decimal);
                        var valorComprometido = itemGrupo.AdicionarCelulaFilha("COMPROMETIDO", itemData.ValorComprometido.ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                        comprometido.AdicionarCelulaFilha("TOTALCOMPROMETIDOGRUPO", itensOrc.Where(x => x.Grupo == itemData.Grupo && x.Data == itemData.Data && x.Data != "/").Select(x => x.ValorComprometido).Aggregate<decimal>((a, b) => a + b).ToString(), valorComprometido.IndexV, grupo.IndexH, tipoCelula: TipoCelula.Decimal);
                        var valorSaldo = itemGrupo.AdicionarCelulaFilha("SALDO", itemData.Saldo.ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                        saldo.AdicionarCelulaFilha("TOTALSALDOGRUPO", itensOrc.Where(x => x.Grupo == itemData.Grupo && x.Data == itemData.Data && x.Data != "/").Select(x => x.Saldo).Aggregate<decimal>((a, b) => a + b).ToString(), valorSaldo.IndexV, grupo.IndexH, tipoCelula: TipoCelula.Decimal);




                        data.Celulas.Add(valorLiberado);
                        data.Celulas.Add(valorComprometido);
                        data.Celulas.Add(valorSaldo);
                    }
                }
            }

            var header = aba.AdicionarCelula("", "Acompanhamento do Orçamento Produtor", 5, 2, false, true, tamanhoLetra: 20, range: 8);
            header.ExecutarMerge();

            if (dataSource.Any())
            {
                var total = aba.AdicionarCelula("Total", "Total", 4, aba.IndexHDisponivel + 1, false, true);
                total.AdicionarCelulaFilha("TOTALLIBERADO", aba.Celulas.Where(x => x.Id == "LIBERADO").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                total.AdicionarCelulaFilha("TOTALCOMPROMETIDO", aba.Celulas.Where(x => x.Id == "COMPROMETIDO").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                total.AdicionarCelulaFilha("TOTALSALDO", aba.Celulas.Where(x => x.Id == "SALDO").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
            }
            ExcelWorksheet abaResumo = this.CreateWorksheet(pck, aba.Nome);
            aba.Celulas.Where(x => !x.Invisivel).ToList().ForEach(x => WriteCell(abaResumo, x.IndexV, x.IndexH, x.Valor, x.TipoCelula, x.Merge, borda: x.Borda, range: x.Range, tamanhoLetra: x.TamanhoLetra, header: x.Header, autoAjuste: x.AutoAjuste));
            this.SetBorderTable(abaResumo, aba.IndexH, aba.IndexHDisponivel - 1, aba.IndexV, aba.IndexVDisponivel - 1);

            return abaResumo;
        }

        private ExcelWorksheet GerarAbaDetalhe(ExcelPackage pck, IEnumerable<OrcamentoVO> dataSource)
        {

            var aba = new AbaExcel("Detalhe", 1, 5);
            var fases = aba.AdicionarCelula("", "Fase", 1, 5, true, true);
            var etapas = aba.AdicionarCelula("", "Etapa", 2, 5, true, true);
            var grupos = aba.AdicionarCelula("", "Grupo", 3, 5, true, true);
            var itens = aba.AdicionarCelula("", "Itens", 4, 5, true, true);
            var objetos = aba.AdicionarCelula("", "Objeto", 5, 5, true, true);
            var totalComprometido = aba.AdicionarCelula("", "Total Comprometido", 6, 5, true, true, autoAjuste:true);

            var countData = aba.IndexVDisponivel;

            var dataSourceGroup = dataSource.GroupBy(g => g.FaseEtapaGrupo)
                         .Select(g => g)
                         .ToList();

            foreach (var orc in dataSourceGroup)
            {
                var itensOrc = orc.Select(x => x);

                var nomeFase = orc.FirstOrDefault().Fase;
                var nomeEtapa = orc.FirstOrDefault().Etapa;
                var nomeGrupo = orc.FirstOrDefault().Grupo;
                var nomeItem = orc.FirstOrDefault().NomeItemOrcamento;
                var token = orc.FirstOrDefault().Token;

                var fase = fases.Celulas.FirstOrDefault(x => x.Valor == nomeFase);
                if (fase == null)
                {
                    fase = fases.AdicionarCelulaFilha(nomeFase, nomeFase, PosicaoCelula.Vertical, celulaIsoladaH: true);
                    fases.Celulas.Add(fase);
                }



                var etapa = etapas.Celulas.FirstOrDefault(x => x.Valor == nomeEtapa);
                if (etapa == null)
                {
                    var ultimaEtapa = fase.Celulas.LastOrDefault(x => x.Id == "ETAPA");
                    if (ultimaEtapa != null && ultimaEtapa.Celulas.Any())
                        etapa = fase.AdicionarCelulaFilha("ETAPA", nomeEtapa, ultimaEtapa.IndexV, ultimaEtapa.UltimoIndexHFilhas + 1, celulaIsoladaH: false);
                    else
                        etapa = fase.AdicionarCelulaFilha("ETAPA", nomeEtapa, PosicaoCelula.Diagonal, celulaIsoladaH: false);

                    etapas.Celulas.Add(etapa);


                }



                var grupo = grupos.Celulas.FirstOrDefault(x => x.Valor == nomeGrupo);
                if (grupo == null)
                {
                    var ultimoGrupo = etapa.Celulas.LastOrDefault(x => x.Id == "GRUPO");
                    if (ultimoGrupo != null && ultimoGrupo.Celulas.Any())
                        grupo = etapa.AdicionarCelulaFilha("GRUPO", nomeGrupo, ultimoGrupo.IndexV, ultimoGrupo.UltimoIndexHFilhas + 1, celulaIsoladaH: false);
                    else
                        grupo = etapa.AdicionarCelulaFilha("GRUPO", nomeGrupo, PosicaoCelula.Diagonal, celulaIsoladaH: false);

                    grupos.Celulas.Add(grupo);
                }

                fase.StartIndexV = 6;
                etapa.StartIndexV = 6;

                var itensPorData = itensOrc.GroupBy(g => g.Data)
                     .Select(g => g)
                     .ToList();

                foreach (var it in itensPorData)
                {
                    foreach (var itemData in it)
                    {
                        if (itemData.Data == "/")
                            continue;

                        var data = aba.Celulas.FirstOrDefault(x => x.Valor == itemData.Data);
                        var itemGrupo = grupo.Celulas.FirstOrDefault(x => x.Valor == itemData.NomeItemOrcamento);
                        var ultimoItenGrupo = grupo.Celulas.LastOrDefault(x => x.Id == "ITEM");

                        if (itemGrupo == null)
                        {

                            if (ultimoItenGrupo != null)
                                itemGrupo = grupo.AdicionarCelulaFilha("ITEM", itemData.NomeItemOrcamento, ultimoItenGrupo.IndexV, ultimoItenGrupo.UltimoIndexHFilhas + 1);
                            else
                                itemGrupo = grupo.AdicionarCelulaFilha("ITEM", itemData.NomeItemOrcamento, PosicaoCelula.Diagonal);

                            
                        }


                        if (data == null)
                        {
                            data = aba.AdicionarCelula("DATA", itemData.Data, aba.IndexVDisponivel, 5, true, true,autoAjuste: true);
                            totalComprometido.Celulas.Add(data);
                          
                        }

                       

                        foreach (var obj in itemData.Objetos)
                        {
                            var indexH = itemGrupo.UltimoIndexHFilhas + 1;
      
                            var objCel = itemGrupo.AdicionarCelulaFilha("OBJETO", obj.Nome, objetos.IndexV, indexH);
                            grupo.Celulas.Add(objCel);
                            objetos.Celulas.Add(objCel);
                            objCel.AdicionarCelulaFilha("", obj.Valor.ToString(), data.IndexV, objCel.IndexH,tipoCelula:TipoCelula.Decimal);

                            var totalGrupo = dataSource.Where(x => x.Grupo == nomeGrupo && x.Data == itemData.Data).Select(x => x.ValoresObjetos()).Aggregate<decimal>((a, b) => a + b);
                            grupo.AdicionarCelulaFilha(itemData.Grupo + "/" + itemData.Data + "/" + "TOTALGRUPO", totalGrupo.ToString(), data.IndexV, grupo.IndexH,tipoCelula:TipoCelula.Decimal);

                        }

                        GerarTotalCondicaoDetalhe(fase, "TOTALFASE", dataSource.Where(x => x.Fase == nomeFase));
                        GerarTotalCondicaoDetalhe(etapa, "TOTALEAPA", dataSource.Where(x => x.Etapa == nomeEtapa));
                        itemGrupo.AdicionarCelulaFilha("TOTALITEM", dataSource.Where(x => x.Token == itemData.Token).Select(x=> x.ValoresObjetos()).Aggregate<decimal>((a,b) => a + b).ToString(),totalComprometido.IndexV,itemGrupo.IndexH,tipoCelula:TipoCelula.Decimal);                      
                        GerarTotalCondicaoDetalhe(grupo, "TOTAL", dataSource.Where(x => x.Grupo == nomeGrupo), totalComprometido.IndexV);
                        GerarTotalCondicaoDetalhe(itemGrupo, itemData.Token + "/" + itemData.Data, dataSource.Where(x => x.Token == itemData.Token && x.Data == itemData.Data),data.IndexV);
                        GerarTotalCondicaoDetalhe(fase, itemData.Fase + "/" + itemData.Data + "/FASE", dataSource.Where(x => x.Fase == nomeFase && x.Data == itemData.Data));
                        GerarTotalCondicaoDetalhe(etapa, itemData.Fase + "/" + itemData.Data, dataSource.Where(x => x.Etapa == nomeEtapa && x.Data == itemData.Data));
                    }
                }
            }

            var header = aba.AdicionarCelula("", "Acompanhamento do Orçamento Produtor", 5, 2, false, true, tamanhoLetra: 20, range: 8);
            header.ExecutarMerge();

            
            if (dataSource.Any())
            {
                var total = aba.AdicionarCelula("Total", "Total", 5, aba.IndexHDisponivel + 1, false, true);
                var totalComprometidoGeral = dataSource.Select(x => x.ValoresObjetos()).Aggregate<decimal>((a, b) => a + b);
                total.AdicionarCelulaFilha("TOTALLIBERADO", totalComprometidoGeral.ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);

                var datas = aba.Celulas.Where(x => x.Id == "DATA");

                foreach (var data in datas.ToList())
                {
                    data.AdicionarCelulasRangeVertical(objetos.UltimoIndexHFilhas, "", "0", data.IndexV, data.IndexH,tipoCelula:TipoCelula.Decimal);
                    var query  = dataSource.Where(x => x.Data == data.Valor);

                    var valor = query.Any()? query.Select(x => x.ValoresObjetos()).Aggregate<decimal>((a, b) => a + b).ToString() : "0";
                    total.AdicionarCelulaFilha("TOTALLIBERADO", valor, PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                }

            }



            ExcelWorksheet abaDetalhe = this.CreateWorksheet(pck, aba.Nome);
            aba.Celulas.Where(x => !x.Invisivel).ToList().ForEach(x => WriteCell(abaDetalhe, x.IndexV, x.IndexH, x.Valor, x.TipoCelula, x.Merge, borda: x.Borda, range: x.Range, tamanhoLetra: x.TamanhoLetra, header: x.Header, autoAjuste: x.AutoAjuste));
            this.SetBorderTable(abaDetalhe, aba.IndexH, aba.IndexHDisponivel - 1, aba.IndexV, aba.IndexVDisponivel - 1);
           

            return abaDetalhe;

        }

        private void GerarTotalCondicao(Celula celula, string id, IEnumerable<OrcamentoVO> dataSource)
        {
            if (!celula.Celulas.Any(x => x.Id == id))
            {
                GerarTotal(celula, id, dataSource);
            }
        }

        private void GerarTotalCondicaoDetalhe(Celula celula, string id, IEnumerable<OrcamentoVO> dataSource, int indexV = 0)
        {
            if (!celula.Celulas.Any(x => x.Id == id))
            {
                GerarTotalDetalhe(celula, id, dataSource, indexV);
            }
        }

        private void GerarTotal(Celula celula, string id, IEnumerable<OrcamentoVO> dataSource)
        {
            var valorTTItemLiberado = dataSource.Select(x => x.ValorLiberado).Aggregate<decimal>((a, b) => a + b);
            celula.AdicionarCelulaFilha(id, valorTTItemLiberado.ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
            celula.StartIndexV = 0;
            var valorTTItemComprometido = dataSource.Select(x => x.ValorComprometido).Aggregate<decimal>((a, b) => a + b);
            celula.AdicionarCelulaFilha(id, valorTTItemComprometido.ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);

            var valorTTItemSaldo = dataSource.Select(x => x.Saldo).Aggregate<decimal>((a, b) => a + b);
            celula.AdicionarCelulaFilha(id, valorTTItemSaldo.ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
        }

        private void GerarTotalDetalhe(Celula celula, string id, IEnumerable<OrcamentoVO> dataSource, int indexV = 0)
        {
            if (indexV == 0)
                indexV = celula.UltimoIndexVFilhas > celula.StartIndexV ? celula.UltimoIndexVFilhas + 1 : celula.StartIndexV;

            var totalFase = dataSource.Select(x => x.ValoresObjetos()).Aggregate<decimal>((a, b) => a + b);
            celula.AdicionarCelulaFilha(id, totalFase.ToString(), indexV, celula.IndexH,tipoCelula:TipoCelula.Decimal);
            celula.StartIndexV = 0;
        }

        //private void GerarTotal(AbaExcel aba, Celula celula)
        //{
        //    var liberados = aba.Celulas.Where(x => x.CelulaHPai != null && x.CelulaHPai.Valor == celula.Valor).OrderBy(x => x.IndexH);

        //    foreach (var cel in liberados)
        //    {
        //        var total = celula.CelulasVFilhas.LastOrDefault(x => x.IndexH == cel.IndexH);
        //        if (total != null)
        //            total.Valor = (Convert.ToDecimal(total.Valor) + Convert.ToDecimal(cel.Valor)).ToString();
        //        else
        //            celula.AdicionarCelulaVFilha("", cel.Valor);
        //    }
        //}

        public override string GetName()
        {
            return "RelatorioExemplo" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
