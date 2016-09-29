using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using Agnus.Service;
using Agnus.Domain.Models;
using Agnus.Service.Interfaces;
using Agnus.Domain.Models.Relatorio;
using Agnus.Reporting.ModelsReporting;

namespace Agnus.Reporting
{
    public class RelatorioAcompanhamentoOrcamentoRealProdutorUso : RelatorioExcelBase
    {
        private IEntityService<ProjetoOrcamento> _service;
        private IEntityService<OrcamentoAtual> _serviceAtual;
        private IOrcamentoService _serviceOrgamento;
        public long _idProduto { get; set; }
        public bool _nucleo { get; set; }

        public RelatorioAcompanhamentoOrcamentoRealProdutorUso(IEntityService<ProjetoOrcamento> service, IEntityService<OrcamentoAtual> serviceOrcamentoAtual, long idProduto, bool nucleo, IOrcamentoService serviceOrcamento)
        {
            _service = service;
            _idProduto = idProduto;
            _nucleo = nucleo;
            _serviceOrgamento = serviceOrcamento;
            _serviceAtual = serviceOrcamentoAtual;
        }

        public override byte[] GenerateReport()
        {
            using (var pck = new ExcelPackage())
            {
                var projetoOrcamento = _service.GetAllByFilter(x => x.IdProjeto == _idProduto && x.DataAprovacao.HasValue);// && x.DataAprovacao.HasValue).FirstOrDefault();

                var orcamentosAtuais = _serviceAtual.GetAllByFilter(x => projetoOrcamento.Any(y => y.Id == x.IdProjetoOrcamento)).Select(x => new OrcamentoVO(x));

                if (_nucleo)
                    GerarAbaNucleo(pck, orcamentosAtuais);
                else
                    GerarAbaPadrao(pck, orcamentosAtuais);

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

        private ExcelWorksheet GerarAbaPadrao(ExcelPackage pck, IEnumerable<OrcamentoVO> dataSource)
        {
            var aba = new AbaExcel("Resumo", 1, 5);
            var ipcs = aba.AdicionarCelula("", "IPC", 1, 6, true, true);
            var projetos = aba.AdicionarCelula("", "Projeto", 2, 6, true, true, autoAjuste: true);
            var tipo = aba.AdicionarCelula("", "Tipo", 3, 5, true, true);
            var tipoR = tipo.AdicionarCelulaFilha(string.Empty, "R", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoCR = tipo.AdicionarCelulaFilha(string.Empty, "CR", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoSR = tipo.AdicionarCelulaFilha(string.Empty, "SR", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoP = tipo.AdicionarCelulaFilha(string.Empty, "P", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoCP = tipo.AdicionarCelulaFilha(string.Empty, "CP", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoSP = tipo.AdicionarCelulaFilha(string.Empty, "SP", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            tipo.ExecutarMerge();
            var total = aba.AdicionarCelula("", "Total", tipo.UltimoIndexVFilhas + 1, 6, true, true);

            var countData = aba.IndexVDisponivel;

            var dataSourceGroup = dataSource.OrderBy(x => x.Data).GroupBy(g => g.FaseEtapaGrupo)
                         .Select(g => g)
                         .ToList();

            foreach (var orc in dataSourceGroup)
            {
                var itensOrc = orc.Select(x => x);

                var nomeIpc = orc.FirstOrDefault().IPC;
                var nomeProjeto = orc.FirstOrDefault().Projeto;


                var ipc = ipcs.Celulas.FirstOrDefault(x => x.Valor == nomeIpc);
                if (ipc == null)
                {
                    ipc = ipcs.AdicionarCelulaFilha(nomeIpc, nomeIpc, PosicaoCelula.Vertical, celulaIsoladaH: true);
                    ipcs.Celulas.Add(ipc);
                }

                var projeto = projetos.Celulas.FirstOrDefault(x => x.Valor == ipc.Valor + nomeProjeto);
                if (projeto == null)
                {
                    var ultimoProjeto = ipc.Celulas.LastOrDefault(x => x.Id == ipc.Valor + nomeProjeto);
                    if (ultimoProjeto != null && ultimoProjeto.Celulas.Any())
                        projeto = ipc.AdicionarCelulaFilha(ipc.Valor + nomeProjeto, nomeProjeto, ultimoProjeto.IndexV, ultimoProjeto.UltimoIndexHFilhas + 1, celulaIsoladaH: false);
                    else
                        projeto = ipc.AdicionarCelulaFilha(ipc.Valor + nomeProjeto, nomeProjeto, PosicaoCelula.Horizontal, celulaIsoladaH: false);

                    projetos.Celulas.Add(projeto);

                    GerarTotal(tipoR, "ValorFaseR", "Projeto", nomeProjeto, "ValorR", dataSource, tipoR.IndexV, projeto.IndexH);
                    GerarTotal(tipoCR, "ValorFaseCR", "Projeto", nomeProjeto, "ValorUsoRealCR", dataSource, tipoCR.IndexV, projeto.IndexH);
                    GerarTotal(tipoSR, "ValorFaseSR", "Projeto", nomeProjeto, "ValorSR", dataSource, tipoSR.IndexV, projeto.IndexH);
                    GerarTotal(tipoP, "ValorFaseP", "Projeto", nomeProjeto, "ValorItemProdutorP", dataSource, tipoP.IndexV, projeto.IndexH);
                    GerarTotal(tipoCP, "ValorFaseCP", "Projeto", nomeProjeto, "ValorUsoProdutorCP", dataSource, tipoCP.IndexV, projeto.IndexH);
                    GerarTotal(tipoSP, "ValorFaseSP", "Projeto", nomeProjeto, "ValorSP", dataSource, tipoSP.IndexV, projeto.IndexH);

                    GerarTotal(total, "TOTALPROJETO", "Projeto", projeto.Valor, "Total", dataSource.Where(x=> x.IPC == ipc.Valor), total.IndexV, projeto.IndexH);
                }


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

                        var data = aba.Celulas.FirstOrDefault(x => x.Valor == itemData.Data);



                        if (data == null)
                        {
                            var ultimaData = aba.Celulas.LastOrDefault(x => x.Id == "DATA");
                            var indexVData = ultimaData != null ? ultimaData.IndexV + 1 : aba.IndexVDisponivel;

                            data = aba.AdicionarCelula("DATA", itemData.Data, indexVData, projetos.IndexH, true, true, autoAjuste: true);
                        }

                        if (!aba.Celulas.Any(x => x.IndexH == projeto.IndexH && x.IndexV == data.IndexV))
                        {
                            data.AdicionarCelulaFilha("TOTALDATA", dataSource.Where(x => x.Data == data.Valor && x.Projeto == projeto.Valor).Select(x => x.Total).Aggregate<decimal>((a, b) => a + b).ToString(), data.IndexV, projeto.IndexH, tipoCelula: TipoCelula.Decimal);
                        }
                    }
                }
            }

            var header = aba.AdicionarCelula("", "Acompanhamento do Orçamento Real x Produtor x Uso", 5, 2, false, true, tamanhoLetra: 20, range: 10);
            header.ExecutarMerge();

            var datas = aba.Celulas.Where(x => x.Id == "DATA");

            if (dataSource.Any())
            {
                var totalGeral = aba.AdicionarCelula("TOTALGERAL", "Total", projetos.IndexV, aba.IndexHDisponivel + 1);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseR").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseCR").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseSR").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseP").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseCP").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseSP").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "TOTALPROJETO").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);

                foreach (var data in datas.ToList())
                {
                    data.AdicionarCelulasRangeVertical(total.UltimoIndexHFilhas, "", "0", data.IndexV, data.IndexH, tipoCelula: TipoCelula.Decimal);
                    totalGeral.AdicionarCelulaFilha("TOTAL", aba.Celulas.Where(x => x.Id == "TOTALDATA" && x.IndexV == data.IndexV).Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                }

            }


            ExcelWorksheet abaResumo = this.CreateWorksheet(pck, aba.Nome);
            aba.Celulas.Where(x => !x.Invisivel).ToList().ForEach(x => WriteCell(abaResumo, x.IndexV, x.IndexH, x.Valor, x.TipoCelula, x.Merge, borda: x.Borda, range: x.Range, tamanhoLetra: x.TamanhoLetra, header: x.Header, autoAjuste: x.AutoAjuste));
            this.SetBorderTable(abaResumo, aba.IndexH, aba.IndexHDisponivel - 1, aba.IndexV, aba.IndexVDisponivel - 1);

            return abaResumo;
        }

        private ExcelWorksheet GerarAbaNucleo(ExcelPackage pck, IEnumerable<OrcamentoVO> dataSource)
        {
            var aba = new AbaExcel("Resumo", 1, 5);
            var fases = aba.AdicionarCelula("", "Fase", 1, 5, true, true);
            var etapas = aba.AdicionarCelula("", "Etapa", 2, 5, true, true);
            var grupos = aba.AdicionarCelula("", "Grupo", 3, 5, true, true);
            var itens = aba.AdicionarCelula("", "Item", 4, 5, true, true);
            var projetos = aba.AdicionarCelula("", "Projeto", 4, 5, true, true, autoAjuste: true);
            var tipo = aba.AdicionarCelula("", "Tipo", 5, 4, true, true);
            var tipoR = tipo.AdicionarCelulaFilha(string.Empty, "R", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoCR = tipo.AdicionarCelulaFilha(string.Empty, "CR", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoSR = tipo.AdicionarCelulaFilha(string.Empty, "SR", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoP = tipo.AdicionarCelulaFilha(string.Empty, "P", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoCP = tipo.AdicionarCelulaFilha(string.Empty, "CP", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            var tipoSP = tipo.AdicionarCelulaFilha(string.Empty, "SP", PosicaoCelula.HorizontalVertical, borda: true, header: true, autoAjuste: true);
            tipo.ExecutarMerge();
            var total = aba.AdicionarCelula("", "Total", tipo.UltimoIndexVFilhas + 1, 5, true, true);


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
                var faseEtapaGrupo = orc.FirstOrDefault().FaseEtapaGrupo;
                var faseEtapa = orc.FirstOrDefault().FaseEtapa;

                var fase = fases.Celulas.FirstOrDefault(x => x.Valor == nomeFase);
                if (fase == null)
                {
                    fase = fases.AdicionarCelulaFilha(nomeFase, nomeFase, PosicaoCelula.Vertical, celulaIsoladaH: true);
                    fases.Celulas.Add(fase);

                    GerarTotal(tipoR, "ValorFaseR", "Fase", fase.Valor, "ValorR", dataSource, tipoR.IndexV, fase.IndexH);
                    GerarTotal(tipoCR, "ValorFaseCR", "Fase", fase.Valor, "ValorUsoRealCR", dataSource, tipoCR.IndexV, fase.IndexH);
                    GerarTotal(tipoSR, "ValorFaseSR", "Fase", fase.Valor, "ValorSR", dataSource, tipoSR.IndexV, fase.IndexH);
                    GerarTotal(tipoP, "ValorFaseP", "Fase", fase.Valor, "ValorItemProdutorP", dataSource, tipoP.IndexV, fase.IndexH);
                    GerarTotal(tipoCP, "ValorFaseCP", "Fase", fase.Valor, "ValorUsoProdutorCP", dataSource, tipoCP.IndexV, fase.IndexH);
                    GerarTotal(tipoSP, "ValorFaseSP", "Fase", fase.Valor, "ValorSP", dataSource, tipoSP.IndexV, fase.IndexH);

                    GerarTotal(total, "TOTALFASE", "Fase", fase.Valor, "Total", dataSource, total.IndexV, fase.IndexH);
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

                    GerarTotal(tipoR, "R", "FaseEtapa", faseEtapa, "ValorR", dataSource, tipoR.IndexV, etapa.IndexH);
                    GerarTotal(tipoCR, "CR", "FaseEtapa", faseEtapa, "ValorUsoRealCR", dataSource, tipoCR.IndexV, etapa.IndexH);
                    GerarTotal(tipoSR, "SR", "FaseEtapa", faseEtapa, "ValorSR", dataSource, tipoSR.IndexV, etapa.IndexH);
                    GerarTotal(tipoP, "P", "FaseEtapa", faseEtapa, "ValorItemProdutorP", dataSource, tipoP.IndexV, etapa.IndexH);
                    GerarTotal(tipoCP, "CP", "FaseEtapa", faseEtapa, "ValorUsoProdutorCP", dataSource, tipoCP.IndexV, etapa.IndexH);
                    GerarTotal(tipoSP, "SP", "FaseEtapa", faseEtapa, "ValorSP", dataSource, tipoSP.IndexV, etapa.IndexH);

                    GerarTotal(total, "TOTAL", "FaseEtapa", faseEtapa, "Total", dataSource, total.IndexV, etapa.IndexH);
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

                    GerarTotal(tipoR, "R", "FaseEtapaGrupo", faseEtapaGrupo, "ValorR", dataSource, tipoR.IndexV, grupo.IndexH);
                    GerarTotal(tipoCR, "CR", "FaseEtapaGrupo", faseEtapaGrupo, "ValorUsoRealCR", dataSource, tipoCR.IndexV, grupo.IndexH);
                    GerarTotal(tipoSR, "SR", "FaseEtapaGrupo", faseEtapaGrupo, "ValorSR", dataSource, tipoSR.IndexV, grupo.IndexH);
                    GerarTotal(tipoP, "P", "FaseEtapaGrupo", faseEtapaGrupo, "ValorItemProdutorP", dataSource, tipoP.IndexV, grupo.IndexH);
                    GerarTotal(tipoCP, "CP", "FaseEtapaGrupo", faseEtapaGrupo, "ValorUsoProdutorCP", dataSource, tipoCP.IndexV, grupo.IndexH);
                    GerarTotal(tipoSP, "SP", "FaseEtapaGrupo", faseEtapaGrupo, "ValorSP", dataSource, tipoSP.IndexV, grupo.IndexH);

                    GerarTotal(total, "TOTAL", "FaseEtapaGrupo", faseEtapaGrupo, "Total", dataSource, total.IndexV, grupo.IndexH);
                }

                fase.StartIndexV = 5;
                etapa.StartIndexV = 5;

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



                        if (data == null)
                        {
                            var ultimaData = aba.Celulas.LastOrDefault(x => x.Id == "DATA");
                            var indexVData = ultimaData != null ? ultimaData.IndexV + 1 : aba.IndexVDisponivel;

                            data = aba.AdicionarCelula("DATA", itemData.Data, indexVData, fases.IndexH, true, true, autoAjuste: true);

                        }


                        if (itemGrupo == null)
                        {
                            itemGrupo = grupo.AdicionarCelulaFilha("ITEM", itemData.NomeItemOrcamento, PosicaoCelula.VerticalHorizontal);

                            GerarTotal(itemGrupo, "Token", itemData.Token, dataSource);

                            var tt = dataSource.Where(x => x.Token == itemData.Token).Select(x => x.Total).Aggregate<decimal>((a, b) => a + b).ToString();
                            total.AdicionarCelulaFilha("TOTAL", tt, total.IndexV, itemGrupo.IndexH, tipoCelula: TipoCelula.Decimal);
                        }

                        if (!aba.Celulas.Any(x => x.IndexH == fase.IndexH && x.IndexV == data.IndexV))
                        {
                            data.AdicionarCelulaFilha("TOTALDATA", dataSource.Where(x => x.Data == data.Valor && x.Fase == itemData.Fase).Select(x => x.Total).Aggregate<decimal>((a, b) => a + b).ToString(), data.IndexV, fase.IndexH, tipoCelula: TipoCelula.Decimal);
                        }

                        if (!aba.Celulas.Any(x => x.IndexH == etapa.IndexH && x.IndexV == data.IndexV))
                        {
                            data.AdicionarCelulaFilha("TOTALDATA", dataSource.Where(x => x.Data == data.Valor && x.FaseEtapa == itemData.FaseEtapa).Select(x => x.Total).Aggregate<decimal>((a, b) => a + b).ToString(), data.IndexV, etapa.IndexH, tipoCelula: TipoCelula.Decimal);
                        }

                        if (!aba.Celulas.Any(x => x.IndexH == grupo.IndexH && x.IndexV == data.IndexV))
                        {
                            data.AdicionarCelulaFilha("TOTALDATA", dataSource.Where(x => x.Data == data.Valor && x.FaseEtapaGrupo == itemData.FaseEtapaGrupo).Select(x => x.Total).Aggregate<decimal>((a, b) => a + b).ToString(), data.IndexV, grupo.IndexH, tipoCelula: TipoCelula.Decimal);
                        }

                        if (!aba.Celulas.Any(x => x.IndexH == itemGrupo.IndexH && x.IndexV == data.IndexV))
                        {
                            itemGrupo.AdicionarCelulaFilha("TOTALDATA", dataSource.Where(x => x.Data == data.Valor && x.Token == itemData.Token).Select(x => x.Total).Aggregate<decimal>((a, b) => a + b).ToString(), data.IndexV, itemGrupo.IndexH, tipoCelula: TipoCelula.Decimal);
                        }



                    }
                }
            }

            var header = aba.AdicionarCelula("", "Acompanhamento do Orçamento Real x Produtor x Uso", 5, 2, false, true, tamanhoLetra: 20, range: 10);
            header.ExecutarMerge();

            var datas = aba.Celulas.Where(x => x.Id == "DATA");

            if (dataSource.Any())
            {
                var totalGeral = aba.AdicionarCelula("TOTALGERAL", "Total", projetos.IndexV, aba.IndexHDisponivel + 1);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseR").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseCR").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseSR").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseP").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseCP").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "ValorFaseSP").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                totalGeral.AdicionarCelulaFilha("TOTALGERAL", aba.Celulas.Where(x => x.Id == "TOTALFASE").Select(x => Convert.ToDecimal(x.Valor)).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);

                foreach (var data in datas.ToList())
                {
                    data.AdicionarCelulasRangeVertical(total.UltimoIndexHFilhas, "", "0", data.IndexV, data.IndexH, tipoCelula: TipoCelula.Decimal);
                    totalGeral.AdicionarCelulaFilha("TOTAL", dataSource.Where(x => x.Data == data.Valor).Select(x => x.Total).Aggregate<decimal>((a, b) => a + b).ToString(), PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
                }

            }

            ExcelWorksheet abaResumo = this.CreateWorksheet(pck, aba.Nome);
            aba.Celulas.Where(x => !x.Invisivel).ToList().ForEach(x => WriteCell(abaResumo, x.IndexV, x.IndexH, x.Valor, x.TipoCelula, x.Merge, borda: x.Borda, range: x.Range, tamanhoLetra: x.TamanhoLetra, header: x.Header, autoAjuste: x.AutoAjuste));
            this.SetBorderTable(abaResumo, aba.IndexH, aba.IndexHDisponivel - 1, aba.IndexV, aba.IndexVDisponivel - 1);

            return abaResumo;
        }

        private void GerarTotal(Celula celula, string parametro, string id, IEnumerable<OrcamentoVO> dataSource)
        {
            var vr = dataSource.Where(x => x.GetType().GetProperty(parametro).GetValue(x).ToString() == id).Select(x => x.ValorR).Aggregate<decimal>((a, b) => a + b).ToString();
            celula.AdicionarCelulaFilha("R", vr, PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
            var vcr = dataSource.Where(x => x.GetType().GetProperty(parametro).GetValue(x).ToString() == id).Select(x => x.ValorUsoRealCR).Aggregate<decimal>((a, b) => a + b).ToString();
            celula.AdicionarCelulaFilha("CR", vcr, PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
            var vsr = dataSource.Where(x => x.GetType().GetProperty(parametro).GetValue(x).ToString() == id).Select(x => x.ValorSR).Aggregate<decimal>((a, b) => a + b).ToString();
            celula.AdicionarCelulaFilha("SR", vsr, PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
            var vp = dataSource.Where(x => x.GetType().GetProperty(parametro).GetValue(x).ToString() == id).Select(x => x.ValorItemProdutorP).Aggregate<decimal>((a, b) => a + b).ToString();
            celula.AdicionarCelulaFilha("P", vp, PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
            var vcp = dataSource.Where(x => x.GetType().GetProperty(parametro).GetValue(x).ToString() == id).Select(x => x.ValorUsoProdutorCP).Aggregate<decimal>((a, b) => a + b).ToString();
            celula.AdicionarCelulaFilha("CP", vcp, PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
            var vsp = dataSource.Where(x => x.GetType().GetProperty(parametro).GetValue(x).ToString() == id).Select(x => x.ValorSP).Aggregate<decimal>((a, b) => a + b).ToString();
            celula.AdicionarCelulaFilha("SP", vsp, PosicaoCelula.Horizontal, tipoCelula: TipoCelula.Decimal);
        }

        private void GerarTotal(Celula celula, string id, string parametroFiltro, string valorBusca, string parametroValor, IEnumerable<OrcamentoVO> dataSource, int indexV, int indexH)
        {
            var vr = dataSource.Where(x => x.GetType().GetProperty(parametroFiltro).GetValue(x).ToString() == valorBusca).Select(x => Convert.ToDecimal(x.GetType().GetProperty(parametroValor).GetValue(x))).Aggregate<decimal>((a, b) => a + b).ToString();
            celula.AdicionarCelulaFilha(id, vr, indexV, indexH, tipoCelula: TipoCelula.Decimal);
        }

        public override string GetName()
        {
            return "RelatorioExemplo" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
