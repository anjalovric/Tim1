using InitialProject.Model;
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

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for GuideWindow.xaml
    /// </summary>
    public partial class GuideWindow : Window
    {
        private HomeView homeView;
        private AddTourView addTourView;
        private CancelView cancelView;
        private TourStatisticsView tourStatisticsView;
        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            homeView = new HomeView(user);
            cancelView = new CancelView(user);
            addTourView = new AddTourView(homeView.Tours,user,cancelView.TourInstances);
            tourStatisticsView = new TourStatisticsView();

            Main.Content = homeView;

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = cancelView;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = addTourView;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = homeView;
        }
        private void TourStatitcs_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = tourStatisticsView;
        }
    }
}
