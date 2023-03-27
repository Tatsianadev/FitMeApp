﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.Data.SqlClient.DataClassification;
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
    public class ExcelReportOpenXml : IExcelReport
    {
        public void WriteToExcel(DataTable table, FileInfo file, string tableName)
        {
            SpreadsheetDocument spreadsheetDocument =
                SpreadsheetDocument.Create(file.FullName, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookPart = AddWorkbookPart(spreadsheetDocument);
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
            uint currentSheetId = 1;

            AddNewPartStyle(workbookPart);
            int rowIndexCount = 1;

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();
            Columns columns = SetDefaultColumnWidth();
            worksheetPart.Worksheet.Append(columns);

            SheetData sheetData = new SheetData();
            worksheetPart.Worksheet.AppendChild(sheetData);

            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = currentSheetId,
                Name = string.IsNullOrWhiteSpace(tableName) ? "Sheet" + currentSheetId : tableName
            };

            if (table.Rows.Count == 0)
            {
                CreateDefaultWithMessage(rowIndexCount, sheetData);
            }
            else
            {
                int numberOfColumns = table.Columns.Count;
                string[] excelColumnNames = new string[numberOfColumns];

                //Create Header
                Row sheetRowHeader = CreateHeader(rowIndexCount, table, numberOfColumns, excelColumnNames);
                sheetData.Append(sheetRowHeader);
                rowIndexCount++;

                //Create Body
                rowIndexCount = CreateBody(rowIndexCount, table, sheetData, excelColumnNames);
            }

            sheets.Append(sheet);
            workbookPart.Workbook.Save();
            spreadsheetDocument.Close();
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


        private Columns SetDefaultColumnWidth()
        {
            Columns columns = new Columns();
            //width of 1st Column
            columns.Append(new Column() { Min = 1, Max = 1, Width = 10, CustomWidth = true });
            //set column width from 2th to 3 columns
            columns.Append(new Column() { Min = 2, Max = 3, Width = 20, CustomWidth = true });
            //width of 4th Column
            columns.Append(new Column() { Min = 4, Max = 4, Width = 30, CustomWidth = true });
            //width of 5th Column
            columns.Append(new Column() { Min = 5, Max =5, Width = 15, CustomWidth = true });
            //set column width from 6th to 400 columns
            columns.Append(new Column() { Min = 6, Max = 400, Width = 10, CustomWidth = true });

            return columns;
        }


        private void CreateDefaultWithMessage(int rowIndexCount, SheetData sheetData)
        {
            Row sheetRow = new Row() { RowIndex = Convert.ToUInt32(rowIndexCount)};
            Cell cellHeader = new Cell() { CellReference = "A1", CellValue = new CellValue("No records to display"), DataType = CellValues.String };
            cellHeader.StyleIndex = 1;

            sheetRow.Append(cellHeader);
            sheetData.Append(sheetRow);
        }


        private Row CreateHeader(int rowIndexCount, DataTable table, int numberOfColumns, string[] excelColumnNames)
        {
            Row sheetRowHeader = new Row() { RowIndex = Convert.ToUInt32(rowIndexCount) };
            for (int i = 0; i < numberOfColumns; i++)
            {
                excelColumnNames[i] = GetExcelColumnName(i);

                Cell cellHeader = new Cell() { CellReference = excelColumnNames[i] + rowIndexCount, CellValue = new CellValue(table.Columns[i].ColumnName), DataType = CellValues.String };
                cellHeader.StyleIndex = 2;
                sheetRowHeader.Append(cellHeader);
            }

            return sheetRowHeader;
        }


        private int CreateBody(int rowIndexCount, DataTable table, SheetData sheetData, string[] excelColumnNames)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Row sheetRow = new Row() { RowIndex = Convert.ToUInt32(rowIndexCount) };

                for (int j = 0; j < table.Columns.Count; j++)
                {
                    sheetRow.Append(GetCellWithDataType(excelColumnNames[j] + rowIndexCount, table.Rows[i][j], table.Columns[j].DataType));
                }

                sheetData.Append(sheetRow);
                ++rowIndexCount;
            }

            return rowIndexCount;
        }



        private Cell GetCellWithDataType(string cellRef, object value, Type type)
        {
            if (type == typeof(DateTime))
            {
                Cell cell = new Cell()
                {
                    DataType = new EnumValue<CellValues>(CellValues.Number),
                    StyleIndex = 3
                };

                if (value != DBNull.Value)
                {
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    DateTime valueDate = (DateTime)value;
                    string valueString = valueDate.ToOADate().ToString(cultureInfo);
                    CellValue cellValue = new CellValue(valueString);
                    cell.Append(cellValue);
                }

                return cell;
            }

            if (type == typeof(long) || type == typeof(int) || type == typeof(short))
            {
                Cell cell = new Cell() { CellReference = cellRef, CellValue = new CellValue(value.ToString()), DataType = CellValues.Number };
                cell.StyleIndex = 4;
                return cell;
            }

            if (type == typeof(decimal))
            {
                Cell cell = new Cell() { CellReference = cellRef, CellValue = new CellValue(value.ToString()), DataType = CellValues.Number };
                cell.StyleIndex = 5;
                return cell;
            }
            else
            {
                Cell cell = new Cell() { CellReference = cellRef, CellValue = new CellValue(value.ToString()), DataType = CellValues.String };
                cell.StyleIndex = 1;
                return cell;
            }

        }


        private string GetExcelColumnName(int columnIndex)
        {
            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format(CultureInfo.CurrentCulture, "{0}{1}", firstChar, secondChar);
        }


        private Stylesheet GenerateStylesSheet()
        {
            Fonts fonts = GenerateFonts();
            Fills fills = GenerateFills();
            Borders borders = GenerateBorders();
            CellFormats cellFormats = GenerateCellFormats();
            Column column = GenerateColumnProperty();
            Stylesheet stylesheet = new Stylesheet(fonts, fills, borders, cellFormats, column);

            return stylesheet;
        }


        private Fonts GenerateFonts()
        {
            Fonts fonts = new Fonts(
                //default - Index 0
                new Font(new FontSize() { Val = 10 }, new FontName() { Val = "Arial Unicode" }),
                //header - Index 1
                new Font(new FontSize() { Val = 14 }, new Bold())
                );
            return fonts;
        }


        private Fills GenerateFills()
        {
            Fills fills = new Fills(
                //Index 0
                new Fill(new PatternFill() { PatternType = PatternValues.None }),
                //Index 1
                new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }),
                //Index 2
                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } }) { PatternType = PatternValues.Solid })
                );
            return fills;
        }


        private Borders GenerateBorders()
        {
            Borders borders = new Borders(
                //default - Index 0
                new Border(),
                //black border - Index 1
                new Border(
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin })
                );
            return borders;
        }


        private CellFormats GenerateCellFormats()
        {
            CellFormats cellFormats = new CellFormats(
                //default - Cell StyleIndex 0
                new CellFormat(new Alignment() { WrapText = true, Vertical = VerticalAlignmentValues.Top }),
                //default2 - Cell StyleIndex 1
                new CellFormat(new Alignment() { WrapText = true, Vertical = VerticalAlignmentValues.Top }) { FontId = 0, FillId = 1, BorderId = 1, ApplyBorder = true },
                //header - Cell StyleIndex 2
                new CellFormat(new Alignment() { WrapText = true, Vertical = VerticalAlignmentValues.Top, Horizontal = HorizontalAlignmentValues.Center}) { FontId = 1, FillId = 2, BorderId = 1, ApplyBorder = true },
                //DateTime DataType - Cell StyleIndex 3
                new CellFormat(new Alignment() { Vertical = VerticalAlignmentValues.Top }) { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, NumberFormatId = 15, ApplyNumberFormat = true },
                //int, long, short DataType - Cell StyleIndex 4
                new CellFormat(new Alignment() { WrapText = true, Vertical = VerticalAlignmentValues.Top }) { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, NumberFormatId = 1 },
                //decimal DataType - Cell StyleIndex 2
                new CellFormat(new Alignment() { WrapText = true, Vertical = VerticalAlignmentValues.Top }) { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, NumberFormatId = 2 }
                );
            return cellFormats;
        }


        private Column GenerateColumnProperty()
        {
            return new Column()
            {
                Width = 100,
                CustomWidth = true
            };
        }



    }
}