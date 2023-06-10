using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for PdfPreviewView.xaml
    /// </summary>
    public partial class PDFPreviewView : Page
    {
        public PDFPreviewView()
        {
            InitializeComponent();
            DataContext = this;
            FileStream stream = new FileStream(@"../../../Resources/Reports/Guest2Report.pdf", FileMode.Open);
            pdfViewer.Load(stream);
        }
    }
}
