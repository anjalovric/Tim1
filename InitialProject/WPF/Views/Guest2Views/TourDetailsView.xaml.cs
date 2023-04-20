using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using InitialProject.Model;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for TourDetails.xaml
    /// </summary>
    public partial class TourDetailsView : Window
    {
        public TourDetailsView(TourInstance tourInstance,Guest2 guest2)
        {
            InitializeComponent();
            this.DataContext = new TourDetailsViewModel(tourInstance,guest2);
        }
    }
}
