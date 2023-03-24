using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FitMeApp.Services.Contracts.Interfaces;


namespace FitMeApp.Services
{
    public class ExcelReportOpenXml: IExcelReport
    {
        public void WriteToExcel(DataTable table, FileInfo file, string tableName)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = AddWorkbookPart(spreadsheetDocument);
                    Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                    int sheetId = 1;
                }
            }
        }

        private WorkbookPart AddWorkbookPart(SpreadsheetDocument spreadsheetDocument)
        {
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            return workbookPart;
        }

        private void AddNewPartStyle(WorkbookPart workbookPart)
        {
            WorkbookStylesPart stypePart = workbookPart.AddNewPart<WorkbookStylesPart>();
            //stypePart.Stylesheet = GenerateStyle
        }

    }
}
