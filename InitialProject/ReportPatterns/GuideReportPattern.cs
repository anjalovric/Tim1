using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace InitialProject.ReportPatterns
{
    public class GuideReportPattern : ReportGenerator
    {
        public override void GenerateConclusion()
        {
            throw new NotImplementedException();
        }

        public override void GenerateContent()
        {
            throw new NotImplementedException();
        }

        public override void GenerateTableContent()
        {
            throw new NotImplementedException();
        }

        public override void GenerateTitle()
        {
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            Graphics.DrawString("Finished tour report", Font, PdfBrushes.Black, new PointF(120, 100));
        }

        public override void SavePdf()
        {
            Document.Save("/Reports/GuideReport.pdf");
            Document.Close(true);
            throw new NotImplementedException();
        }
    }
}
