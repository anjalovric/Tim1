using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using InitialProject.Model;
using InitialProject.Service;
using static Xceed.Wpf.Toolkit.Calculator;
using InitialProject.APPLICATION.UseCases;
using SixLabors.Fonts;
using System.Windows.Controls;

namespace InitialProject.ReportPatterns
{
    public class Guest1ReportPattern : ReportGenerator
    {
        private Guest1 guest;
        private Graphics graphics;
        private int numberOfLines = 0;
        private GuestAverageReviewService guestAverageReviewService;
        private double averageCleanliness;
        private double averageFollowingRules;
        public Guest1ReportPattern(Guest1 guest)
        {
            this.guest = guest;
            guestAverageReviewService = new GuestAverageReviewService();
        }
        public override void GenerateConclusion()
        {
            
            int reviewsNumber = guestAverageReviewService.GetReviewsNumberByGuest(guest);
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 14);

            

            StringBuilder stringBuilder2 = new StringBuilder("Total number of reviews from owners: " + reviewsNumber.ToString() + ".");
            Graphics.DrawString(stringBuilder2.ToString(), Font, PdfBrushes.Black, new Point(0, 330));
        }

        public override void GenerateContent()
        { 
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 14);
            
            StringBuilder stringBuilder1 = new StringBuilder("");
            stringBuilder1.Append("The report shows the average ratings that the guest received from the owners for");

            StringBuilder stringBuilder2 = new StringBuilder("");
            stringBuilder2.Append("each of the categories.");


            StringBuilder stringBuilder3 = new StringBuilder("");
            stringBuilder3.Append("Guest: ").Append(guest.Name + " " + guest.LastName);


            Graphics.DrawString(stringBuilder1.ToString(), Font, PdfBrushes.Black, new PointF(0, 160));
            Graphics.DrawString(stringBuilder2.ToString(), Font, PdfBrushes.Black, new PointF(0, 180));
            Graphics.DrawString(stringBuilder3.ToString(), Font, PdfBrushes.Black, new PointF(0, 200));


            //ratings

            averageCleanliness = guestAverageReviewService.GetAverageCleanlinessReview(guest);
            averageFollowingRules = guestAverageReviewService.GetAverageFollowingRulesReview(guest);

            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);

            StringBuilder stringBuilder4 = new StringBuilder("");
            stringBuilder4.Append("Cleanliness - average rating: " + Math.Round(averageCleanliness, 2));

            StringBuilder stringBuilder5 = new StringBuilder("");
            stringBuilder5.Append("Following rules - average rating: " + Math.Round(averageFollowingRules, 2));

            Graphics.DrawString(stringBuilder4.ToString(), Font, PdfBrushes.Black, new PointF(0, 250));
            Graphics.DrawString(stringBuilder5.ToString(), Font, PdfBrushes.Black, new PointF(0, 270));


            double averageRating = guestAverageReviewService.GetAverageRatingForReport(guest);
            StringBuilder stringBuilder6 = new StringBuilder("Average rating for both categories: " + Math.Round(averageRating, 2));
            Graphics.DrawString(stringBuilder6.ToString(), Font, PdfBrushes.Black, new PointF(0, 290));


        }
    


        public override void GenerateTableContent() { }
        public override void GenerateTable1Content() { }
        public override void GenerateTable2Content() { }

        public override void GenerateTitle()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 24);
            Graphics.DrawString("Guest's average rating report", Font, PdfBrushes.Black, new PointF(100,100));
        }

        public override void SavePdf()
        {
             Document.Save("../../../Resources/Reports/Guest1Report.pdf");
             Document.Close(true);
         
        }
    }
}
