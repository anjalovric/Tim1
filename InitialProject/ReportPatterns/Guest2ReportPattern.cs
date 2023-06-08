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
            stringBuilder.Append("Here is the report on the tour request statistics for guest ").Append(guest2.Name).Append(" ").Append(guest2.LastName).Append(" Based on the calculation of tour request statistics, we can conclude that ");
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(30, 140));
            stringBuilder = new StringBuilder("");
            stringBuilder.Append(requestStatisticsService.ProcentOfAcceptedRequest(guest2)).Append("%").Append(" of tour requests have been accepted, while ").Append(requestStatisticsService.ProcentOfInvalidRequest(guest2)).Append("% have been invalid.")
                         .Append("Additionally, based on the number of people in the ");
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 155));
            stringBuilder = new StringBuilder("");
            stringBuilder.Append("accepted tours, we have determined that the average number of people in those tours is ").Append(requestStatisticsService.AverageNumberOfPeopleInAcceptedRequests(guest2)).Append(".");
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 170));
            
            stringBuilder = new StringBuilder("");
            stringBuilder.Append(DateTime.Now.ToString());
            Graphics.DrawString(stringBuilder.ToString(), Font, PdfBrushes.Black, new PointF(0, 750));
        }

        public override void GenerateTableContent()
        {
            Table.Columns.Add("Languages");
            Table.Columns.Add("Number of requests by languages");
            Table.Rows.Add(new string[] { "Languages", "Number of requests by languages" });
            Table.Rows.Add("english", engCounter);
            Table.Rows.Add("serbian", srbCounter);
            Table.Rows.Add("russian", rusCounter);
            Table.Rows.Add("italian", itCounter);
            Table.Rows.Add("arabic", arabCounter);
            Table.Rows.Add("spain", spCounter);
        }
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
