using FitMeApp.Services.Contracts.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace FitMeApp.Services
{
    public class ExcelReportEpPlus: IExcelReport
    {
        public void WriteToExcel(DataTable table, FileInfo file, string tableName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excelPack = new ExcelPackage(file))
            {
                var sheet = excelPack.Workbook.Worksheets.Add(tableName);
                sheet.Cells["A2"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light11);
                sheet.Cells.AutoFitColumns();

                //Formats the header
                sheet.Cells["A1"].Value = $"{tableName} _ " + DateTime.Now.ToString("dd-MM-yyyy");
                sheet.Cells["A1:F1"].Merge = true;
                sheet.Rows[1].Style.Font.Size = 20;
                sheet.Rows[1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Rows[2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                excelPack.Save();
            }
        }
    }
}
