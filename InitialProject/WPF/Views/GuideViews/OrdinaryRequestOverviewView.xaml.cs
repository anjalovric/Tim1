using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
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

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for OrdinaryRequestOverviewView.xaml
    /// </summary>
    public partial class OrdinaryRequestOverviewView : Page
    {
        public OrdinaryRequestOverviewViewModel viewModel;
        public OrdinaryRequestOverviewView(User user, ObservableCollection<TourInstance> Tours, ObservableCollection<TourInstance> Future)
        {
            InitializeComponent();
            viewModel = new OrdinaryRequestOverviewViewModel(user, Tours, Future);
            DataContext = viewModel;
        }
    }
}
