﻿using FitMeApp.Services.Contracts.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FitMeApp.Services.Contracts.Models.Chart;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services
{
    public sealed class ExcelReportEpPlus : IExcelReport
    {
        public async Task WriteToExcelAsync (DataTable table, FileInfo file, string tableName)
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

                await excelPack.SaveAsync();
            }
        }


        public async Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(FileInfo file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<AttendanceChartModel> attendanceChart = new List<AttendanceChartModel>();

            if (file.Exists && file.Extension == ".xlsx")
            {
                using (var excelPack = new ExcelPackage(file))
                {
                    await excelPack.LoadAsync(file);

                    if (excelPack.Workbook.Worksheets.Count > 0)
                    {
                        var workSheet = excelPack.Workbook.Worksheets[0];
                        for (int col = 1; col <= (2 * Enum.GetValues(typeof(DayOfWeek)).Length); col += 2)
                        {
                            List<VisitorsPerHourModel> timeVisitorsLine = new List<VisitorsPerHourModel>();
                            AttendanceChartModel currentDayAttendance = new AttendanceChartModel();
                            int row = 4; //row with data to start reading

                            var dayName = workSheet.Cells[2, col].Value.ToString();
                            if (Enum.IsDefined(typeof(DayOfWeek), dayName))
                            {
                                currentDayAttendance.DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayName, true);
                            }

                            while (string.IsNullOrWhiteSpace(workSheet.Cells[row, col].Value?.ToString()) == false)
                            {
                                VisitorsPerHourModel timeVisitors = new VisitorsPerHourModel();
                                int hour;
                                bool getTimeSuccess = int.TryParse(workSheet.Cells[row, col].Value.ToString(), out hour);
                                if (getTimeSuccess)
                                {
                                    timeVisitors.Hour = hour;
                                }

                                int numberOfVisitors;
                                bool getNumberOfVisitorsSuccess = int.TryParse(workSheet.Cells[row, col + 1].Value.ToString(), out numberOfVisitors);
                                if (getNumberOfVisitorsSuccess)
                                {
                                    timeVisitors.NumberOfVisitors = numberOfVisitors;
                                }

                                timeVisitorsLine.Add(timeVisitors);
                                row++;
                            }

                            currentDayAttendance.NumberOfVisitorsPerHour = timeVisitorsLine;
                            attendanceChart.Add(currentDayAttendance);
                        }
                    }
                }
            }

            return attendanceChart;

        }



        public async Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var nutrientsModel = new NutrientsModel();
            List<string>[] modelFieldsContainer = new List<string>[3];

            if (file.Exists && file.Extension == ".xlsx")
            {
                using (var excelPack = new ExcelPackage(file))
                {
                    await excelPack.LoadAsync(file);
                    int sheetsCount = excelPack.Workbook.Worksheets.Count;
                    if (sheetsCount > 0)
                    {
                        for (int i = 0; i < sheetsCount; i++)
                        {
                            var workSheet = excelPack.Workbook.Worksheets[i];
                            int row = 0;
                            
                            while (string.IsNullOrWhiteSpace(workSheet.Cells[row, 2].Value?.ToString()) == false)
                            {
                                modelFieldsContainer[i].Add(workSheet.Cells[row, 2].Value.ToString());
                                row++;
                            }

                        }

                        nutrientsModel.Proteins = modelFieldsContainer[0];
                        nutrientsModel.Fats = modelFieldsContainer[1];
                        nutrientsModel.Carbohydrates = modelFieldsContainer[2];
                    }
                }

            }

            return nutrientsModel;
        }
    }
}
