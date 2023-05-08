using InitialProject.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Domain.Model;
using System.Diagnostics.Metrics;
using InitialProject.WPF.ViewModels.Guest2ViewModels;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for TourRequestStatisticsView.xaml
    /// </summary>
    public partial class TourRequestStatisticsView : Window
    {
        public TourRequestStatisticsView(Model.Guest2 guest2)
        {
            InitializeComponent();
            DataContext = new TourRequestStatisticsViewModel(guest2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
