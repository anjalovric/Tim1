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
using Org.BouncyCastle.Asn1.Ocsp;
using InitialProject.WPF.Views.Guest2Views;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;

namespace InitialProject.ReportPatterns
{
    public class Guest2ReportPattern : ReportGenerator
    {
        Guest2 guest2;
        int tCounter = 0;
        int sCounter = 0;
        int rCounter = 0;
        int aCounter = 0;
        int gCounter = 0;
        int hCounter = 0;
        int iCounter = 0;
        int bCounter = 0;
        int rusCounter = 0;
        int arabCounter = 0;
        int srbCounter = 0;
        int engCounter = 0;
        int itCounter = 0;
        int spCounter = 0;
        public Guest2ReportPattern(Guest2 guest2,int t,int s,int r, int a,int g,int h,int i,int b,int rusCounter, int spCounter, int arabCounter, int engCounter, int itCounter, int srbCounter)
        {
            this.guest2 = guest2;
            tCounter = t;
            sCounter = s;
            rCounter = r;
            aCounter = a;
            gCounter = g;
            hCounter = h;
            iCounter = i;
            bCounter = b;
            this.spCounter = spCounter;
            this.engCounter = engCounter;
            this.arabCounter = arabCounter;
            this.itCounter = itCounter;
            this.srbCounter = srbCounter;
            this.rusCounter = rusCounter;
        }

        


        public override void GenerateConclusion()
        {
           
        }

        public override void GenerateContent()
        {

            RequestStatisticsService requestStatisticsService = new RequestStatisticsService();
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("Guest: ").Append(guest2.Name).Append(" ").Append(guest2.LastName);
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 140));
            stringBuilder = new StringBuilder("");
            stringBuilder.Append("Precent of accepted tour requests: ").Append(requestStatisticsService.ProcentOfAcceptedRequest(guest2)).Append("\n");
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 155));
            stringBuilder = new StringBuilder("");
            stringBuilder.Append("Precent of invalid tour requests: ").Append(requestStatisticsService.ProcentOfInvalidRequest(guest2));
            
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 170));
            
            stringBuilder = new StringBuilder("");
            stringBuilder.Append("Average number of people in accepted requests: ").Append(requestStatisticsService.AverageNumberOfPeopleInAcceptedRequests(guest2)).Append("\n");
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 185));

            stringBuilder = new StringBuilder("");
            stringBuilder.Append("There is number of requests by languages");

            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 210));
            stringBuilder = new StringBuilder("");
            stringBuilder.Append("There is number of requests by locations");

            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 310));
        }

        public override void GenerateTable2Content()
        {
            
            Table2.Columns.Add("Languages");
            Table2.Columns.Add("Number of requests by languages");
            Table2.Rows.Add(new string[] { "Languages", "Number of requests by languages" });
            Table2.Rows.Add("english", engCounter);
            Table2.Rows.Add("serbian", srbCounter);
            Table2.Rows.Add("russian", rusCounter);
            Table2.Rows.Add("italian", itCounter);
            Table2.Rows.Add("arabic", arabCounter);
            Table2.Rows.Add("spain", spCounter);
        }
        public override void GenerateTableContent() { }
        public override void GenerateTable1Content()
        {
            
            Table1.Columns.Add("Locations");
            Table1.Columns.Add("Number of requests by locations");
            Table1.Rows.Add(new string[] { "Location", "Number of requests by locations" });
            Table1.Rows.Add("Turkey", tCounter);
            Table1.Rows.Add("Serbia", sCounter);
            Table1.Rows.Add("Russia", rCounter);
            Table1.Rows.Add("Greece", gCounter);
            Table1.Rows.Add("Italy", iCounter);
            Table1.Rows.Add("Hungary", hCounter);
            Table1.Rows.Add("Austria", aCounter);
            Table1.Rows.Add("BiH", bCounter);
        }



        public override void GenerateTitle()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            Graphics.DrawString("Tour statistic report", Font, PdfBrushes.Black, new PointF(150, 100));
        }

        public override void SavePdf()
        {
            Document.Save("../../../Resources/Reports/Guest2Report.pdf");
            Document.Close(true);

        }
    }
}
