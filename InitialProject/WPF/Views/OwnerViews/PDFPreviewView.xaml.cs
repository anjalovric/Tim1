using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using Org.BouncyCastle.Crypto.IO;
using Syncfusion.Windows.PdfViewer;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for PDFPreviewView.xaml
    /// </summary>
    public partial class PDFPreviewView : Page
    {
        public PDFPreviewView()
        {
            InitializeComponent();
            DataContext = this;
            FileStream stream = new FileStream(@"../../../Resources/Reports/OwnerReport.pdf", FileMode.Open);

            //Load PDF file using stream.
            pdfViewer.Load(stream);
        }
    }
}
