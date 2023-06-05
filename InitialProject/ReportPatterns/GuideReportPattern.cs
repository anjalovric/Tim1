using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using InitialProject.Model;

namespace InitialProject.ReportPatterns
{
    public class GuideReportPattern : ReportGenerator
    {
        public GuideReportPattern(TourInstance finishedTourInstance,List<Guest2> guestsOnTour, double ageUnder,double ageBetween, double ageOver, double withVoucher, double withoutVoucher, double attendance) 
        {
            guests=guestsOnTour.ToList();
            finishedInstance=finishedTourInstance;
            ageUnder18=ageUnder;
            ageBetween18And50=ageBetween;
            ageOver50=ageOver;
            withoutVoucherPercent = withoutVoucher;
            withVoucherPercent= withVoucher;
            attendancePercent=attendance;
        }

        private List<Guest2> guests {  get; set; }

        private TourInstance finishedInstance;
        private double ageUnder18;
        private double ageBetween18And50;
        private double ageOver50;
        private double withVoucherPercent;
        private double withoutVoucherPercent;
        private double attendancePercent;

        public override void GenerateConclusion()
        {
           
        }

        public override void GenerateContent()
        {
           
        }

        public override void GenerateTableContent()
        {
            Table.Columns.Add("Guest");

            Table.Rows.Add(new string[] { "Guest" });

            foreach (Guest2 guest in guests)
            {
                Table.Rows.Add(new string[] { guest.ToString() });
            }
        }

        public override void GenerateTitle()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            Graphics.DrawString("Finished tour report", Font, PdfBrushes.Black, new PointF(120, 100));
        }

        public override void SavePdf()
        {
            Document.Save("../../Reports/GuideReport.pdf");
            Document.Close(true);
          //  throw new NotImplementedException();
        }
    }
}
