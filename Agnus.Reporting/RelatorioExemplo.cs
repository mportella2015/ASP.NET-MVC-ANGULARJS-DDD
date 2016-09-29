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

namespace Agnus.Reporting
{
    public class RelatorioExemplo : RelatorioExcelBase
    {
       

        public override byte[] GenerateReport()
        {
            using (var pck = new ExcelPackage())
            {
                ExcelWorksheet aba1 = this.CreateWorksheet(pck, "Exemplo");

                ExcelWorksheet aba2 = this.CreateWorksheet(pck, "Exemplo 2");
                
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

                return pck.GetAsByteArray();
            }
        }

        public override string GetName()
        {
            return "RelatorioExemplo" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
