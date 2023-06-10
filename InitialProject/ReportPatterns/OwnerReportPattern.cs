using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
        private int numberOfLines = 0;
        public OwnerReportPattern(Owner owner)
        {
            this.owner = owner;
            reportService = new OwnerReviewReportService();
            accommodations = reportService.GetAllAccommodationByOwner(owner);
        }
        public override void GenerateConclusion()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            StringBuilder stringBuilder1 = new StringBuilder("Owner " + owner.ToString() + " has " + accommodations.Count
                + " accommodations registrated in My Travel system. Average grade for owner by all categories " + reportService.GetAverageRate(owner) + " and");
            StringBuilder stringBuilder2 = new StringBuilder("total number of reviews made by guests is "+ reportService.GetNumberOfReviews(owner) + ".");
            Graphics.DrawString(stringBuilder1.ToString(), Font, PdfBrushes.Black, new PointF(0, 250 + numberOfLines*10));
            Graphics.DrawString(stringBuilder2.ToString(), Font, PdfBrushes.Black, new Point(0, 260 + numberOfLines*10));
        }

        public override void GenerateContent()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("This report is generated on request made by ").Append(owner.ToString()).Append(" in this system registated as OWNER.")
                .Append(" Report shows average grades");
            StringBuilder stringBuilder2 = new StringBuilder("by categories for each accommodation registated by this Owner.");



            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 140));
            Graphics.DrawString(stringBuilder2.ToString(), Font, PdfBrushes.Black, new PointF(0, 150));
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
                numberOfLines++;
            }
        }
        public override void GenerateTable1Content() { }
        public override void GenerateTable2Content() { }
        public override void GenerateTitle()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            Graphics.DrawString("Average Owner grade report", Font, PdfBrushes.Black, new PointF(130, 100));
        }

        public override void SavePdf()
        {
            Document.Save("../../../Resources/Reports/OwnerReport.pdf");
            Document.Close(true);
        }
    }
}
