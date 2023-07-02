using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using System.IO;
using System.Threading.Tasks;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using Color = Aspose.Pdf.Color;


namespace FitMeApp.Services
{
    public class PdfReportAspose : IPdfReport
    {
        public void CreateDietPlanPdf(DietPdfReportModel dietPlan)
        {
            string filePath = Environment.CurrentDirectory + @"\wwwroot\PDF\Diet\DietPlan_" + dietPlan.UserFirstName + "_" + dietPlan.UserLastName + ".pdf";
            Document document = new Document();
            Page page = document.Pages.Add();

            //Add image
            var imagePath = @"wwwroot\Content\images\healthyFood2.jpg";
            page.AddImage(imagePath, new Aspose.Pdf.Rectangle(200, 630, 400, 830));

            //Add header
            var header = new TextFragment("Diet plan");
            header.TextState.Font = FontRepository.FindFont("Arial");
            header.TextState.FontSize = 24;
            header.HorizontalAlignment = HorizontalAlignment.Center;
            header.Position = new Position(80, 610);
            page.Paragraphs.Add(header);

            //Add table
            var table = new Table
            {
                ColumnWidths = "50 130 250",
                Border = new BorderInfo(BorderSide.Box, 1f, Color.DarkSlateGray),
                DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Color.Black),
                DefaultCellPadding = new MarginInfo(4.5, 4.5, 4.5, 4.5),
                Margin =
                {
                    Bottom = 10,
                    Top = 10
                },
                DefaultCellTextState =
                {
                    Font = FontRepository.FindFont("Helvetica")
                }
            };

            var content = CreateDietTableContent(dietPlan);
            table = FillTable(table, content);

            page.Paragraphs.Add(table);
            document.Save(filePath);
        }


        private List<List<string>> CreateDietTableContent(DietPdfReportModel dietPlan)
        {
            var activity = (Common.PhysicalActivityEnum) dietPlan.PhysicalActivity;
            var dietGoal = (Common.DietGoalsEnum) dietPlan.DietGoalId;

            var rowContent = new List<List<string>>()
            {
                new List<string>() {"1", "Name", dietPlan.UserFirstName},
                new List<string>() {"2", "Surname", dietPlan.UserLastName},
                new List<string>() {"3", "Age", dietPlan.Age.ToString()},
                new List<string>() {"4", "Gender", dietPlan.Gender},
                new List<string>() {"5", "Height, sm", dietPlan.Height.ToString()},
                new List<string>() {"6", "Weight, sm", dietPlan.Weight.ToString()},
                new List<string>() {"7", "Activity level", activity.ToString()},
                new List<string>() {"8", "Current intake, kcal", dietPlan.CurrentCalorieIntake.ToString()},
                new List<string>() {"9", "Diet goal", dietGoal.ToString()},
                new List<string>() {"10", "Date", dietPlan.AnthropometricInfoDate.ToString("yy-MM-dd")},
                new List<string>() {"", "", ""},
                new List<string>() {"11", "Required intake, kcal", dietPlan.RequiredCalorieIntake.ToString()},
                new List<string>() {"12", "Proteins, gr", dietPlan.Proteins.ToString()},
                new List<string>() {"13", "Fats, gr", dietPlan.Fats.ToString()},
                new List<string>() {"14", "Carbohydrates, gr", dietPlan.Carbohydrates.ToString()},
                new List<string>() {"15", "Budget, $", dietPlan.Budget.ToString()},
                new List<string>() {"16", "Diet created date", dietPlan.DietPlanCreatedDate.ToString("yy-MM-dd")}
            };

            return rowContent;
        }



        private Table FillTable(Table table, List<List<string>> content)
        {
            foreach (var rowContent in content)
            {
                var dataRow = table.Rows.Add();
                foreach (var cellContent in rowContent)
                {
                    dataRow.Cells.Add(cellContent);
                }
            }
            return table;
        }
    }
}
