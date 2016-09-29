using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Agnus.Domain.Models.Relatorio;

namespace Agnus.Reporting
{
    public abstract class RelatorioExcelBase
    {
        public abstract string GetName();

        public abstract byte[] GenerateReport();

        protected ExcelWorksheet CreateWorksheet(ExcelPackage pck, string sheetName)
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

            //Image imagem = Image.FromFile(HttpRuntime.AppDomainAppPath + @"Content\Images\logo_dpc.png");
            //ws.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);
            //var pic = ws.Drawings.AddPicture("imagem", imagem);
            //pic.SetPosition(0, 0, 0, 0);
            return ws;
        }

        public void CreateTable(ExcelWorksheet ws, ExcelRangeBase range)
        {
            WriteHeaderStyle(ws, ws.Cells[range.Start.Row, range.Start.Column, range.Start.Row + 1, range.End.Column]);
            int li = range.Start.Row + 2, ci = range.Start.Column, lf = range.End.Row, cf = range.End.Column;
            //if (isReportsTotal)
            //    lf--;
            for (int i = li; i <= lf; i++)
                if (i % 2 != 0)
                    ws.Cells[i, ci, i, cf].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        }

        public ExcelRangeBase WriteData(ExcelWorksheet ws, int contadorLinhas)
        {
            var coluna = 1;
            int linha = 0;
            //reportData.ColumnGroupData.ForEach(
            //        delegate(ExcelReportColumnGroup cg)
            //        {
            //            int colunaInicial = coluna;
            //            linha = contadorLinhas;
            //            bool isNullGroupName = String.IsNullOrEmpty(cg.GroupName);
            //            ws.Cells[linha++, colunaInicial].Value = isNullGroupName ? String.Empty : cg.GroupName;
            //            cg.Columns.ForEach(
            //                delegate(ExcelReportColumn cs)
            //                {
            //                    linha = contadorLinhas + 1;
            //                    ws.Cells[linha++, coluna].Value = cs.Name;
            //                    cs.Cells.ForEach(
            //                        delegate(ExcelReportCell cell)
            //                        {
            //                            ws.Cells[linha, coluna].Value = cell.Value;
            //                            InsertFormat(ws.Cells[linha, coluna], cs.Format);
            //                            linha++;
            //                        });
            //                    if (reportData.IsReportsTotal && cs.IsColumnTotal)
            //                        ws.WriteSumFormula(contadorLinhas + 1, linha, coluna, cs.Format);
            //                    if (cs.ColumnSize != default(double))
            //                        ws.Column(ws.Cells[linha, coluna].Start.Column).Width = cs.ColumnSize;
            //                    else
            //                        ws.Column(ws.Cells[linha, coluna].Start.Column).AutoFit(25, 60);
            //                    coluna++;
            //                });
            //            if (isNullGroupName)
            //            {
            //                ws.Cells[contadorLinhas, colunaInicial].Value = ws.Cells[contadorLinhas + 1, colunaInicial].Value;
            //                ws.Cells[contadorLinhas, colunaInicial, contadorLinhas + 1, colunaInicial].Merge = true;
            //            }
            //            else
            //                ws.Cells[contadorLinhas, colunaInicial, contadorLinhas, coluna - 1].Merge = true;
            //        });
            //if (reportData.IsReportsTotal)
            //{
            //    var linhaTotal = linha - 1;
            //    var linhaTotalRange = ws.Cells[linhaTotal, 1, linhaTotal, coluna - 1];
            //    ws.WriteHeaderStyle(linhaTotalRange);
            //    //ws.FormatTotalText(linhaTotal);
            //}
            return ws.Cells[contadorLinhas, 1, linha, coluna];
        }

        private void WriteHeaderStyle(ExcelWorksheet ws, ExcelRangeBase range)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(4, 36, 77));
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.Font.Bold = true;
            range.Style.Font.Color.SetColor(Color.White);
        }

        public ExcelRange WriteHeaderCell(ExcelWorksheet ws, int columnIndex, object content, int rowIndex)
        {
            var cell = ws.Cells[rowIndex, columnIndex];
            WriteHeaderCell(ws, cell, columnIndex, content);

            return cell;
        }

        public ExcelRange WriteHeaderCell(ExcelWorksheet ws, ExcelRange cell, int columnIndex, object content, bool autoAjuste = false)
        {
            cell.Value = content;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 14;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            if (autoAjuste)
            {
                cell.Style.WrapText = false;
                cell.Worksheet.Column(columnIndex).AutoFit();
            }
            else
                cell.Style.WrapText = true;

            return cell;
        }

        public void WriteCell(ExcelWorksheet ws, int columnIndex, int rowIndex, object content, TipoCelula tipoCelula, bool merge = false, bool borda = false, int range = 1, bool header = false, int tamanhoLetra = 0, bool autoAjuste = false)
        {
            var cell = ws.Cells[rowIndex, columnIndex];
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.WrapText = true;

            if (tipoCelula == TipoCelula.Decimal)
            {
                cell.Value = Convert.ToDecimal(content);
                cell.Style.Numberformat.Format = "#,##0.00";
            }
            else
                cell.Value = content;

            if (merge)
            {
                cell = ws.Cells[rowIndex, columnIndex, rowIndex, columnIndex + range];

                if (!cell.Any(x => x.Merge))
                    cell.Merge = true;
            }

            if (header)
            {
                WriteHeaderCell(ws, cell, columnIndex, content, autoAjuste);
            }

            if (borda)
            {
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                cell.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Medium;
            }

            if (tamanhoLetra != 0)
                cell.Style.Font.Size = tamanhoLetra;

            if (autoAjuste)
                cell.Worksheet.Column(columnIndex).AutoFit();
        }

        public void SetBorderTable(ExcelWorksheet ws, int rowIndex, int rowTo, int columnIndex, int collTo)
        {
            var table = ws.Cells[rowIndex, columnIndex, rowTo, collTo];

            table.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            table.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            table.Style.Border.Left.Style = ExcelBorderStyle.Medium;
            table.Style.Border.Right.Style = ExcelBorderStyle.Medium;
        }

        //public void MontaResumoFaturamento(ExcelWorksheet wsComercial)
        //{
        //    wsComercial.Cells["A6:D6"].Style.Border.Top.Style = ExcelBorderStyle.Double;
        //    wsComercial.Cells[6, 1].Value = "Resumo Faturamento";
        //    wsComercial.Cells["A6:D6"].Merge = true;
        //    wsComercial.Cells["A6:D6"].Style.Font.Size = 14;
        //    wsComercial.Cells["A6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //    wsComercial.Cells[7, 1].Value = "Contrato";
        //    wsComercial.Cells[8, 1].Value = "Faturado";
        //    wsComercial.Cells[9, 1].Value = "Participação Meta";
        //    wsComercial.Cells[10, 1].Value = "Participação Incentivo";
        //    wsComercial.Cells[11, 1].Value = "Pagamento BV";
        //    wsComercial.Cells[12, 1].Value = "Adiantamento BV";

        //    wsComercial.Cells[7, 4].Value = "Variação %";
        //}

        //public void ResumoFaturamento(ExcelWorksheet wsComercial, ContratoVenda contrato, int salto)
        //{
        //    wsComercial.Cells[7, salto].Value = contrato.CodContrato;
        //    wsComercial.Cells[8, salto].Value = contrato.TotalValorFatura;
        //    wsComercial.Cells[9, salto].Value = contrato.TotalValorMeta;
        //    wsComercial.Cells[10, salto].Value = contrato.TotalValorIncentivo;
        //    wsComercial.Cells[11, salto].Value = contrato.ContratoVendaMetas.Sum(x => x.ContratoVendaMetaPgtoes.Sum(z => z.ValorPagamentoBV));
        //    wsComercial.Cells[12, salto].Value = contrato.ContratoVendaMetas.Sum(x => x.ContratoVendaMetaPgtoes.Sum(z => z.ValorAdiantamentoBV));

        //    for (int i = 8; i <= 12; i++)
        //    {
        //        wsComercial.Cells[i, 4].Formula = "=IF(B" + i + "+C" + i + "=0,0,IF(B" + i + ">C" + i + ",((B" + i + "-C" + i + ")*100)/B" + i + ",((C" + i + "-B" + i + ")*100)/C" + i + "))";
        //    }
        //}

        //public void FaturamentoMensal(ExcelWorksheet wsComercial, ContratoVenda contrato, IEnumerable<FaturamentoMensalDTO> faturamentoMensal, int salto)
        //{
        //    wsComercial.Cells["A14:D14"].Style.Border.Top.Style = ExcelBorderStyle.Double;
        //    wsComercial.Cells[14, 1].Value = "Faturamento Mensal";
        //    wsComercial.Cells["A14:D14"].Merge = true;
        //    wsComercial.Cells["A14:D14"].Style.Font.Size = 14;
        //    wsComercial.Cells["A14:D14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //    wsComercial.Cells[15, 1].Value = "Contrato";

        //    wsComercial.Cells[15, salto].Value = contrato.CodContrato;

        //    wsComercial.Cells[15, 4].Value = "Variação %";

        //    foreach (var item in faturamentoMensal)
        //    {
        //        iNumColunaFaturamentoMensal++;
        //        wsComercial.Cells[iNumColunaFaturamentoMensal, 1].Value = item.MesDesc;
        //        wsComercial.Cells[iNumColunaFaturamentoMensal, salto].Value = item.ValorFaturado;
        //    }

        //    wsComercial.Cells[iNumColunaFaturamentoMensal + 1, 1].Value = "Total:";

        //    if (iNumColunaFaturamentoMensal != 15) //VERIFICAÇÃO SE EXISTE ALGUM REGISTRO DE FATURAMENTO MENSAL
        //    {
        //        wsComercial.Cells[iNumColunaFaturamentoMensal + 1, 2].Formula = "=SUM(B16:B" + iNumColunaFaturamentoMensal + ")";
        //        wsComercial.Cells[iNumColunaFaturamentoMensal + 1, 3].Formula = "=SUM(C16:C" + iNumColunaFaturamentoMensal + ")";

        //        for (int i = 16; i <= iNumColunaFaturamentoMensal; i++)
        //        {
        //            wsComercial.Cells[i, 4].Formula = "=IF(B" + i + "+C" + i + "=0,0,IF(B" + i + ">C" + i + ",((B" + i + "-C" + i + ")*100)/B" + i + ",((C" + i + "-B" + i + ")*100)/C" + i + "))";
        //        }

        //        int result = iNumColunaFaturamentoMensal + 1;
        //        wsComercial.Cells[result, 4].Formula = "=IF(B" + result + "+C" + result + "=0,0,IF(B" + result + ">C" + result + ",((B" + result + "-C" + result + ")*100)/B" + result + ",((C" + result + "-B" + result + ")*100)/C" + result + "))";
        //    }
        //}

        //public void MontaFaturamentoPorCliente(ExcelWorksheet wsFaturamentoCliente)
        //{
        //    wsFaturamentoCliente.Cells["A6:D6"].Style.Border.Top.Style = ExcelBorderStyle.Double;
        //    wsFaturamentoCliente.Cells[6, 1].Value = "Faturamento por Cliente";
        //    wsFaturamentoCliente.Cells["A6:D6"].Merge = true;
        //    wsFaturamentoCliente.Cells["A6:D6"].Style.Font.Size = 14;
        //    wsFaturamentoCliente.Cells["A6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //    wsFaturamentoCliente.Cells[7, 1].Value = "Contrato";

        //    wsFaturamentoCliente.Cells["B7:C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    wsFaturamentoCliente.Cells["B7:C7"].Merge = true;
        //    wsFaturamentoCliente.Cells["D7:E7"].Merge = true;
        //    wsFaturamentoCliente.Cells["D7:E7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //    wsFaturamentoCliente.Cells[7, 6].Value = "Variação %";
        //    wsFaturamentoCliente.Cells["F7:G7"].Merge = true;
        //    wsFaturamentoCliente.Cells["F7:G7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //    wsFaturamentoCliente.Cells[8, 1].Value = "Cliente";
        //    wsFaturamentoCliente.Cells[8, 2].Value = "Faturado";
        //    wsFaturamentoCliente.Cells[8, 3].Value = "% partic.";
        //    wsFaturamentoCliente.Cells[8, 4].Value = "Faturado";
        //    wsFaturamentoCliente.Cells[8, 5].Value = "% partic.";
        //    wsFaturamentoCliente.Cells[8, 6].Value = "Faturado";
        //    wsFaturamentoCliente.Cells[8, 7].Value = "% partic.";
        //}

        //public void FaturamentoPorCliente(ExcelWorksheet wsFaturamentoCliente, ContratoVenda contrato, IEnumerable<FaturamentoPorClienteDTO> faturamentoPorClienteAtual, IEnumerable<FaturamentoPorClienteDTO> faturamentoPorClienteAnterior, int saltoContrato, int saltoValorFaturado, int saltoPercParticipacao)
        //{
        //    wsFaturamentoCliente.Cells[7, saltoContrato].Value = contrato.CodContrato;

        //    List<FaturamentoPorClienteDTO> listaFaturamentoSobra = new List<FaturamentoPorClienteDTO>();
        //    FaturamentoPorClienteDTO fatCliente = new FaturamentoPorClienteDTO();

        //    if (faturamentoPorClienteAnterior != null)
        //    {
        //        foreach (var item in faturamentoPorClienteAnterior)
        //        {
        //            if (faturamentoPorClienteAtual.Where(d => d.ClienteId == item.ClienteId).Count() == 0)
        //                listaFaturamentoSobra.Add(item);
        //        }

        //    }

        //    foreach (var itemAtual in faturamentoPorClienteAtual)
        //    {
        //        iNumColunaFaturamentoPorCliente++;
        //        wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, 1].Value = itemAtual.Cliente;
        //        wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, saltoValorFaturado].Value = itemAtual.ValorFaturado;
        //        wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, saltoPercParticipacao].Value = itemAtual.PercParticipacao;

        //        if (faturamentoPorClienteAnterior != null)
        //        {
        //            fatCliente = faturamentoPorClienteAnterior.Where(d => d.ClienteId == itemAtual.ClienteId).FirstOrDefault();

        //            wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, 4].Value = fatCliente.ValorFaturado;
        //            wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, 5].Value = fatCliente.PercParticipacao;

        //        }
        //    }

        //    foreach (var itemSobra in listaFaturamentoSobra)
        //    {
        //        iNumColunaFaturamentoPorCliente++;
        //        wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, 1].Value = itemSobra.Cliente;
        //        wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, 4].Value = itemSobra.ValorFaturado;
        //        wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente, 5].Value = itemSobra.PercParticipacao;
        //    }

        //    wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente + 1, 1].Value = "Total:";
        //    wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente + 1, 2].Formula = "=SUM(B9:B" + iNumColunaFaturamentoPorCliente + ")";
        //    wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente + 1, 3].Formula = "=SUM(C9:C" + iNumColunaFaturamentoPorCliente + ")";
        //    wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente + 1, 4].Formula = "=SUM(D9:D" + iNumColunaFaturamentoPorCliente + ")";
        //    wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente + 1, 5].Formula = "=SUM(E9:E" + iNumColunaFaturamentoPorCliente + ")";
        //    wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente + 1, 6].Formula = "=SUM(F9:F" + iNumColunaFaturamentoPorCliente + ")";
        //    wsFaturamentoCliente.Cells[iNumColunaFaturamentoPorCliente + 1, 7].Formula = "=SUM(G9:G" + iNumColunaFaturamentoPorCliente + ")";

        //    for (int i = 9; i <= iNumColunaFaturamentoPorCliente; i++)
        //    {
        //        wsFaturamentoCliente.Cells[i, 6].Formula = "=IF(B" + i + "+D" + i + "=0,0,IF(B" + i + ">D" + i + ",((B" + i + "-D" + i + ")*100)/B" + i + ",((D" + i + "-B" + i + ")*100)/D" + i + "))";
        //        wsFaturamentoCliente.Cells[i, 7].Formula = "=IF(C" + i + "+E" + i + "=0,0,IF(C" + i + ">E" + i + ",((C" + i + "-E" + i + ")*100)/C" + i + ",((E" + i + "-C" + i + ")*100)/E" + i + "))";
        //    }

        //}

        //public void CondicoesComercializacao(ExcelWorksheet wsCondicaoComercializacao, ContratoVenda contratoAtual, ContratoVenda contratoAnterior)
        //{
        //    wsCondicaoComercializacao.Cells[7, 1].Value = "Condições de Comercialização";

        //    foreach (var itemContratoVendaMetas in contratoAtual.ContratoVendaMetas)
        //    {
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 1, 1].Value = "Meta";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 2, 1].Value = "Meta Pai";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 3, 1].Value = "Valor da Meta";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 4, 1].Value = "Todos os Clientes?";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 5, 1].Value = "Todos os Produtos?";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 6, 1].Value = "Todas as Agencias?";

        //        wsCondicaoComercializacao.Cells[7, 2].Value = contratoAtual.CodContrato;
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 1, 2].Value = itemContratoVendaMetas.CodContratoVendaMeta;
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 2, 2].Value = (itemContratoVendaMetas.ContratoVendaMetaPai != null ? itemContratoVendaMetas.ContratoVendaMetaPai.CodContratoVendaMeta : "");
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 3, 2].Value = itemContratoVendaMetas.ValMetaBasica;
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 4, 2].Value = (itemContratoVendaMetas.IndTodosClientes == true ? "Sim" : "Não");
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 5, 2].Value = (itemContratoVendaMetas.IndTodosProdutos == true ? "Sim" : "Não");
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMeta + 6, 2].Value = (itemContratoVendaMetas.IndTodasAgencias == true ? "Sim" : "Não");

        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI, 1].Value = "Plano de Incentivo";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI, 2].Value = "Faixa De";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI, 3].Value = "Faixa Até";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI, 4].Value = "%";
        //        wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI, 5].Value = "Valor";

        //        foreach (var itemPI in itemContratoVendaMetas.ContratoVendaMetaPIs)
        //        {
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI + 1, 2].Value = itemPI.ValFaturamentoDe;
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI + 1, 3].Value = itemPI.ValFaturamentoAte;
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI + 1, 4].Value = itemPI.PercIncentivo;
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAtualMetaPI + 1, 5].Value = itemPI.ValIncentivo;

        //            iNumColunaCondicoesComercializacaoContratoAtualMetaPI++;
        //        }

        //        iNumColunaCondicoesComercializacaoContratoAtualMeta = iNumColunaCondicoesComercializacaoContratoAtualMetaPI + 1;
        //        iNumColunaCondicoesComercializacaoContratoAtualMetaPI = iNumColunaCondicoesComercializacaoContratoAtualMetaPI + 9;
        //    }

        //    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#DCEAE3");
        //    wsCondicaoComercializacao.Cells["A1:E" + iNumColunaCondicoesComercializacaoContratoAtualMetaPI].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    wsCondicaoComercializacao.Cells["A1:E" + iNumColunaCondicoesComercializacaoContratoAtualMetaPI].Style.Fill.BackgroundColor.SetColor(colFromHex);

        //    if (contratoAnterior != null)
        //    {
        //        wsCondicaoComercializacao.Column(7).Width = 30;
        //        wsCondicaoComercializacao.Cells[1, 7].Style.WrapText = true;

        //        foreach (var itemContratoVendaMetas in contratoAnterior.ContratoVendaMetas)
        //        {
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 1, 7].Value = "Meta";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 2, 7].Value = "Meta Pai";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 3, 7].Value = "Valor da Meta";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 4, 7].Value = "Todos os Clientes?";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 5, 7].Value = "Todos os Produtos?";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 6, 7].Value = "Todas as Agencias?";

        //            wsCondicaoComercializacao.Cells[7, 7].Value = contratoAnterior.CodContrato;
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 1, 8].Value = itemContratoVendaMetas.CodContratoVendaMeta;
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 2, 8].Value = (itemContratoVendaMetas.ContratoVendaMetaPai != null ? itemContratoVendaMetas.ContratoVendaMetaPai.CodContratoVendaMeta : "");
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 3, 8].Value = itemContratoVendaMetas.ValMetaBasica;
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 4, 8].Value = (itemContratoVendaMetas.IndTodosClientes == true ? "Sim" : "Não");
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 5, 8].Value = (itemContratoVendaMetas.IndTodosProdutos == true ? "Sim" : "Não");
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMeta + 6, 8].Value = (itemContratoVendaMetas.IndTodasAgencias == true ? "Sim" : "Não");

        //            //wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoMetaPI, 1].Value = "Plano de Incentivo";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI, 7].Value = "Faixa De";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI, 8].Value = "Faixa Até";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI, 9].Value = "%";
        //            wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI, 10].Value = "Valor";


        //            foreach (var itemPI in itemContratoVendaMetas.ContratoVendaMetaPIs)
        //            {
        //                wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI + 1, 7].Value = itemPI.ValFaturamentoDe;
        //                wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI + 1, 8].Value = itemPI.ValFaturamentoAte;
        //                wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI + 1, 9].Value = itemPI.PercIncentivo;
        //                wsCondicaoComercializacao.Cells[iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI + 1, 10].Value = itemPI.ValIncentivo;

        //                iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI++;
        //            }

        //            iNumColunaCondicoesComercializacaoContratoAnteriorMeta = iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI + 1;
        //            iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI = iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI + 9;
        //        }

        //        if (contratoAnterior.ContratoVendaMetas.Count != 0)
        //        {
        //            Color colFromHex2 = System.Drawing.ColorTranslator.FromHtml("#E2CED7");
        //            wsCondicaoComercializacao.Cells["G1:J" + iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            wsCondicaoComercializacao.Cells["G1:J" + iNumColunaCondicoesComercializacaoContratoAnteriorMetaPI].Style.Fill.BackgroundColor.SetColor(colFromHex2);
        //        }
        //    }
        //}
    }
}

