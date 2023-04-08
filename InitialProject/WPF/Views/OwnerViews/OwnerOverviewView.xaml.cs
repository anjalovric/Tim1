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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Model;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerOverviewView.xaml
    /// </summary>
    public partial class OwnerOverviewView : Page
    {
        private ReservationReschedulingView reservationReschedulingView;
        public OwnerOverviewView(Owner owner)
        {
            InitializeComponent();
            reservationReschedulingView = new ReservationReschedulingView(owner);
        }

        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = reservationReschedulingView;
        }
    }
}
