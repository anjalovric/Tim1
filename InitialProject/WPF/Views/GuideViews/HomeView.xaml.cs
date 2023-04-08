using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {   
        public HomeViewModel viewModel;
        public HomeView(ObservableCollection<TourInstance> Instances)
        {
            InitializeComponent();
            viewModel = new HomeViewModel(Instances);
            DataContext = viewModel;
            
        }
        private void StartTour_Click(object sender, RoutedEventArgs e)
        {
            viewModel.StartTour();
        }


    }
}
