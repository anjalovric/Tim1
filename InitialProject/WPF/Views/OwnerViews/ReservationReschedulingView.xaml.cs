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
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for ReservationReschedulingView.xaml
    /// </summary>
    public partial class ReservationReschedulingView : Page
    {
        public ReservationReschedulingViewModel reservationReschedulingViewModel;
        public ReservationReschedulingView(Owner owner)
        {
            InitializeComponent();
            reservationReschedulingViewModel = new ReservationReschedulingViewModel(owner);
            DataContext = reservationReschedulingViewModel;
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            DecliningRequestView decliningRequestView = new DecliningRequestView(reservationReschedulingViewModel.SelectedRequest.Request);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = decliningRequestView;
            reservationReschedulingViewModel.DeclineRequest(decliningRequestView.decliningRequestViewModel.ReschedulingAccommodationRequest);
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            reservationReschedulingViewModel.ApproveRequest();
        }
    }
}
