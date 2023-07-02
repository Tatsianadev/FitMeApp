using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Syncfusion.Drawing;
using System.IO;
using System.Threading.Tasks;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using Color = Aspose.Pdf.Color;


namespace FitMeApp.Services
{
    public class PdfReportSyncfusion : IPdfReport
    {
        public void CreateDietPlanPdf(DietPdfReportModel dietPlan)
        {
            string filePath = Environment.CurrentDirectory + @"\wwwroot\PDF\Diet\DietPlan.pdf";
            Document document = new Document();
            Page page = document.Pages.Add();

            //Add image
            var imagePath = @"wwwroot\Content\images\healthyFood2.jpg";
            page.AddImage(imagePath, new Aspose.Pdf.Rectangle(200,630,400,830));

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
                ColumnWidths = "50 100 250",
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

            

            var dataRow = table.Rows.Add();
            dataRow.Cells.Add("1");
            dataRow.Cells.Add("User name");
            dataRow.Cells.Add(dietPlan.UserFirstName);

            var dataRow2 = table.Rows.Add();
            dataRow2.Cells.Add("2");
            dataRow2.Cells.Add("Last name");
            dataRow2.Cells.Add(dietPlan.UserLastName);

            page.Paragraphs.Add(table);
            document.Save(filePath);
        }

        



        //public Stream CreateDietPlanPdf2(DietPdfReportModel dietPlan)
        //{
           

        //    PdfDocument report = new PdfDocument();
        //    PdfPage page = report.Pages.Add();
        //    PdfGraphics graphics = page.Graphics;

        //    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
        //    graphics.DrawString("Test PDF", font, PdfBrushes.Black, new PointF(0,0));

        //    //Load the image
        //    //FileStream imageStream = new FileStream("wwwroot\\Content\\images\\healthyFood2.jpg",FileMode.Open, FileAccess.Read);
        //    //PdfBitmap image = new PdfBitmap(imageStream);
        //    //graphics.DrawImage(image, 0, 0);

        //    //Save document as a stream
        //    MemoryStream stream = new MemoryStream();
        //    report.Save(stream);

        //    //If the position is not set to '0' then the PDF will be empty.
        //    stream.Position = 0;
        //    report.Close(true);

        //    //string contentType = "application/pdf";
        //    //string fileName = "DietPlan_" + dietPlan.UserFirstName + ".pdf";

        //    //return File(stream.ToArray(), contentType, fileName);

        //    //FileStreamResult fileStreamResult = new FileStreamResult(stream, contentType);
        //    //fileStreamResult.FileDownloadName = fileName;

        //    return stream;


        //}
    }
}
