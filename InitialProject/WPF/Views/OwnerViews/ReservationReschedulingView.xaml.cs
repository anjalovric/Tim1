using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;
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
    }
}
