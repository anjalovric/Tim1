﻿using System;
using System.Data;
using System.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;

namespace InitialProject.ReportPatterns
{
    public abstract class ReportGenerator
    {
        public PdfGraphics Graphics { get; set; }
        public PdfFont Font { get; set; }

        public PdfDocument Document { get; set; }
        public PdfLightTable PdfLightTable { get; set; }
        public DataTable Table { get; set; }

        public ReportGenerator()
        {
            PdfLightTable = new PdfLightTable();
            Table = new DataTable();
            Document = new PdfDocument();
        }

        public void GenerateReport()
        {
            using (Document)
            {
                PdfPage page = AddPdfPage();

                InitializeGraphics(page);
                GenerateHeader();
                InsertImage(page);

                GenerateTitle();
                GenerateContent();

                GenerateTableContent();
                GenerateTable(page);
                GenerateConclusion();

                SavePdf();
            }
        }

        public abstract void GenerateConclusion();

        public abstract void GenerateTableContent();

        public abstract void GenerateContent();

        private void GenerateTable(PdfPage page)
        {
            PdfLightTable.DataSource = Table;
            PdfLightTable.Draw(page, new PointF(0, 185));
        }

        public abstract void SavePdf();

        public abstract void GenerateTitle();

        private void InsertImage(PdfPage page)
        {
            PdfImage image = PdfImage.FromFile("Resources/Images/logoMyTravel.png");
            RectangleF bounds = new RectangleF(400, 12, 100, 50);
            page.Graphics.DrawImage(image, bounds);
        }

        private void GenerateHeader()
        {
            Graphics.DrawString("Address: Trg Dositeja Obradovica 6", Font, PdfBrushes.Black, new PointF(0, 19));
            Graphics.DrawString("21000 Novi Sad, Serbia", Font, PdfBrushes.Black, new PointF(0, 27));
            Graphics.DrawString("Contact: 021021021", Font, PdfBrushes.Black, new PointF(0, 36));
            Graphics.DrawString("E-mail address: mytravel@gmail.com", Font, PdfBrushes.Black, new PointF(0, 44));
            Graphics.DrawString("Report generated on: " + DateTime.Now, Font, PdfBrushes.Black, new PointF(0, 53));
        }

        private void InitializeGraphics(PdfPage page)
        {
            Graphics = page.Graphics;
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
        }

        private PdfPage AddPdfPage()
        {
            return Document.Pages.Add();
        }
    }
}