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

namespace InitialProject.ReportPatterns
{
    public class GuideReportPattern : ReportGenerator
    {
        public GuideReportPattern(TourInstance finishedTourInstance,List<Guest2> guestsOnTour, double ageUnder,double ageBetween, double ageOver, double withVoucher, double withoutVoucher, double attendance) 
        {
            guests=guestsOnTour.ToList();
            finishedInstance=finishedTourInstance;
            TourService tourService = new TourService();
            LocationService locationService = new LocationService();
            GuideService guideService = new GuideService();
            TourReservationService tourReservationSevice =new TourReservationService();
            tourService.SetTour(finishedInstance);
            locationService.SetLocation(finishedInstance);
            guideService.SetGuide(finishedInstance);
            reservationCount = tourReservationSevice.CountReservationForTour(finishedInstance);
            ageUnder18 =ageUnder;
            ageBetween18And50=ageBetween;
            ageOver50=ageOver;
            withoutVoucherPercent = withoutVoucher;
            withVoucherPercent= withVoucher;
            attendancePercent=attendance;
        }

        private List<Guest2> guests {  get; set; }
        private int reservationCount;
        private TourInstance finishedInstance;
        private double ageUnder18;
        private double ageBetween18And50;
        private double ageOver50;
        private double withVoucherPercent;
        private double withoutVoucherPercent;
        private double attendancePercent;


        public override void GenerateConclusion()
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("Based on our calculations we can conclude that among present guests on tour percent of people that are under 18 years ");
               
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(30, 300));
            StringBuilder stringBuilder1 = new StringBuilder("");
            stringBuilder1.Append("old were ").Append(ageUnder18).Append("%. ").Append
                ("Percent of guests that are between 18 and 50 years old were ").Append(ageBetween18And50).Append("% and percent of guests that are over 50 years old ");
            Graphics.DrawString(stringBuilder1.ToString(), Font, PdfBrushes.Black, new PointF(0, 315));
            StringBuilder stringBuilder2 = new StringBuilder("");
            stringBuilder2.Append("was ").Append(ageOver50).Append(" %.").Append("With vocher there were ").Append(withVoucherPercent).Append("% and without ").Append(withoutVoucherPercent).Append("% of guests").Append
            (" Everything was held regulary as planned. ");

            Graphics.DrawString(stringBuilder2.ToString(), Font, PdfBrushes.Black, new PointF(0, 330));
            StringBuilder stringBuilder3 = new StringBuilder("");
            stringBuilder3.Append(DateTime.Now.ToString());
           
            Graphics.DrawString(stringBuilder3.ToString(), Font, PdfBrushes.Black, new PointF(0, 750));
        }

        public override void GenerateContent()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("Report for tour ").Append(finishedInstance.Tour.Name.ToString()).Append(" held at ")
                .Append(finishedInstance.StartDate.ToString("dd.MM.yyyy.")).Append(" with duration of ").Append(finishedInstance.Tour.Duration.ToString()).Append(" hours, ")
                .Append(" on location ").Append(finishedInstance.Tour.Location.ToString()).Append(". The tour was led by guide ");
            

            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(30, 140));
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            StringBuilder stringBuilderNext = new StringBuilder("");
            stringBuilderNext.Append(finishedInstance.Guide.ToString()).Append(" and held on ").Append(finishedInstance.Tour.Language.ToString()).Append(" language.")
                .Append("There were ").Append(reservationCount.ToString()).Append(" reservations for this tour. Total attendance was ").Append(attendancePercent).Append(" %.");

            Graphics.DrawString(stringBuilderNext.ToString(), Font, PdfBrushes.Black, new PointF(0, 155));


           

        }

        public override void GenerateTableContent()
        {
            Table.Columns.Add("Guests:");

            Table.Rows.Add(new string[] { "Guests" });

            foreach (Guest2 guest in guests)
            {
                Table.Rows.Add(new string[] { guest.ToString() });
            }
        }
        public override void GenerateTable1Content() { }
        public override void GenerateTable2Content() { }
        public override void GenerateTitle()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            Graphics.DrawString("Finished tour report", Font, PdfBrushes.Black, new PointF(150, 100));
        }

        public override void SavePdf()
        {
            Document.Save("../../../Resources/Reports/GuideReport.pdf");
            Document.Close(true);
          
        }
    }
}
