using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
using System.Net;
using System.IO;
using System.DirectoryServices.ActiveDirectory;
using Image = System.Windows.Controls.Image;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for TourStatisticsView.xaml
    /// </summary>
    public partial class TourStatisticsView : Page
    {
       public TourStatisticsViewModel viewModel;

        public TourStatisticsView()
        {

            InitializeComponent();
            viewModel = new TourStatisticsViewModel();
            DataContext = viewModel;

        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ViewDetails(this.TourListDataGrid);
         
        }

        private void MostVisited_Click(object sender, RoutedEventArgs e)
        {
            viewModel.MostVisited();
         
        }

        private void MostVisitedForYear_Click(object sender, RoutedEventArgs e)
        {
            viewModel.MostVisitedForYear();
        }
    }
}
