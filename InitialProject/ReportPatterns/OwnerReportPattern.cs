using System.Collections.Generic;
using System.Drawing;
using InitialProject.APPLICATION.UseCases;
using InitialProject.Model;
using Syncfusion.Pdf.Graphics;

namespace InitialProject.ReportPatterns
{
    public class OwnerReportPattern : ReportGenerator
    {
        private Owner owner;
        private List<Accommodation> accommodations;
        private OwnerReviewReportService reportService;
        private Graphics graphics;
        public OwnerReportPattern(Owner owner)
        {
            this.owner = owner;
            reportService = new OwnerReviewReportService();
            accommodations = reportService.GetAllAccommodationByOwner(owner);
        }
        public override void GenerateConclusion()
        {
           
        }

        public override void GenerateContent()
        {
        }

        public override void GenerateTableContent()
        {
            Table.Columns.Add("Accommodation");
            Table.Columns.Add("Location");
            Table.Columns.Add("Cleanliness (average)");
            Table.Columns.Add("Owner Correctness (average)");

            Table.Rows.Add(new string[] { "Accommodation", "Location", "Cleanliness (average)", "Owner Correctness (average)"});

            foreach (var accommodation in accommodations)
            {
                Table.Rows.Add(new string[] {accommodation.Name, accommodation.Location.ToString(), 
                                             reportService.GetAverageCleanlinessByAccommodation(accommodation).ToString(),
                                             reportService.GetAverageCorrectnessByAccommodation(accommodation).ToString()});
            }
        }
        public override void GenerateTable1Content()
        {
            
        }
        public override void GenerateTitle()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            Graphics.DrawString("Average owner grade report", Font, PdfBrushes.Black, new PointF(170, 100));
        }

        public override void SavePdf()
        {
            Document.Save("../../../Resources/Reports/OwnerReport.pdf");
            Document.Close(true);
        }
    }
}
