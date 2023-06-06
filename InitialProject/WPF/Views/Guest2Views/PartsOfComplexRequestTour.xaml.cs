using InitialProject.Domain.Model;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for PartsOfComplexRequestTour.xaml
    /// </summary>
    public partial class PartsOfComplexRequestTour : Window
    {
        public PartsOfComplexRequestTour(ComplexTourRequests complex)
        {
            InitializeComponent();
            DataContext = new PartsOfComplexRequestTourViewModel(complex,this);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }
    }
}
