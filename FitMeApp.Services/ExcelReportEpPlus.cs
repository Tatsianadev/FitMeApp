using FitMeApp.Services.Contracts.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using FitMeApp.Services.Contracts.Models.Chart;
using System.Threading.Tasks;

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


        public async Task<List<TimeVisitorsModel>> ReadFromExcel(FileInfo file)
        {
            List<TimeVisitorsModel> output = new List<TimeVisitorsModel>();
            if (file.Exists && file.Extension == ".xlsx")
            {
                using (var excelPack = new ExcelPackage(file))
                {
                    await excelPack.LoadAsync(file);
                    var wb = excelPack.Workbook; //throwing out
                    int count = excelPack.Workbook.Worksheets.Count;


                    if ( true /*excelPack.Workbook.Worksheets.Count > 0*/)
                    {
                        var workSheet = excelPack.Workbook.Worksheets[0];
                        int row = 4; //row with data to start reading
                        int col = 1;

                        while (string.IsNullOrWhiteSpace(workSheet.Cells[row,col].Value?.ToString()) == false)
                        {
                            TimeVisitorsModel timeVisitors = new TimeVisitorsModel();
                            int timeInMinutes;
                            bool getTimeSuccess = int.TryParse(workSheet.Cells[row, col].Value.ToString(), out timeInMinutes);
                            if (getTimeSuccess)
                            {
                                timeVisitors.TimeInMinutes = timeInMinutes;
                            }

                            int numberOfVisitors;
                            bool getNumberOfFisitorsSuccess = int.TryParse(workSheet.Cells[row, col+1].Value.ToString(), out numberOfVisitors);
                            if (getNumberOfFisitorsSuccess)
                            {
                                timeVisitors.NumberOfVisitors = numberOfVisitors;
                            }
                            //timeVisitors.TimeInMinutes = int.Parse(workSheet.Cells[row, col].Value.ToString());

                            output.Add(timeVisitors);
                            row++;
                        }
                    }
                }
            }

            return output;

        }
    }
}
