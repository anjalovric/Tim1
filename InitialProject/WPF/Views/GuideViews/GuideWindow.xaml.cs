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
        private User loggedUser;
        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            tourStatisticsView = new TourStatisticsView();
            homeView = new HomeView(user,tourStatisticsView.viewModel.Instances);
            cancelView = new CancelView(user);
            loggedUser = user;
         

            Main.Content = homeView;

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            cancelView = new CancelView(loggedUser);
            Main.Content = cancelView;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            addTourView = new AddTourView(homeView.viewModel.Tours, loggedUser, cancelView.cancelViewModel.TourInstances);
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

        private void Review_Click(object sender, RoutedEventArgs e)
        {
            TourInstanceReviewView tourInstanceReviewView = new TourInstanceReviewView(loggedUser);
            Main.Content= tourInstanceReviewView;
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }
    }
}
