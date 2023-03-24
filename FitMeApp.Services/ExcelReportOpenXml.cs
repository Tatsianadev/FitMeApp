using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FitMeApp.Services.Contracts.Interfaces;
using BottomBorder = DocumentFormat.OpenXml.Spreadsheet.BottomBorder;
using Fill = DocumentFormat.OpenXml.Spreadsheet.Fill;
using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
using ForegroundColor = DocumentFormat.OpenXml.Spreadsheet.ForegroundColor;
using LeftBorder = DocumentFormat.OpenXml.Spreadsheet.LeftBorder;
using PatternFill = DocumentFormat.OpenXml.Spreadsheet.PatternFill;
using RightBorder = DocumentFormat.OpenXml.Spreadsheet.RightBorder;
using TopBorder = DocumentFormat.OpenXml.Spreadsheet.TopBorder;

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
            WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylePart.Stylesheet = GenerateStylesSheet();
            stylePart.Stylesheet.Save();
        }


        private Stylesheet GenerateStylesSheet()
        {
            Fonts fonts = GenerateFonts();
            Fills fills = GenerateFills();
            Borders borders = GenerateBorders();
            return new Stylesheet();
        }


        private Fonts GenerateFonts()
        {
            Fonts fonts = new Fonts(
                new Font(new FontSize() {Val = 10}, new FontName() {Val = "Arial Unicode"}),
                new Font(new FontSize() {Val = 10}, new Bold()));
            
            return fonts;
        }
        

        private Fills GenerateFills()
        {
            Fills fills = new Fills(
                new Fill(new PatternFill() { PatternType = PatternValues.None }),
                new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }),
                new Fill(new PatternFill(new ForegroundColor{ Rgb = new HexBinaryValue(){Value = "66666666" } }) { PatternType = PatternValues.Solid }));

            return fills;
        }


        private Borders GenerateBorders()
        {
            Borders borders = new Borders(
                new Border(
                    new LeftBorder(new Color(){Auto = true}){Style = BorderStyleValues.Thin}, 
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }));

            return borders;
        }
    }
}
