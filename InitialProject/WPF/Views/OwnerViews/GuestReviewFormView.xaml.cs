using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for GuestReviewFormView.xaml
    /// </summary>
    public partial class GuestReviewFormView : Page
    {
        public GuestReviewFormView(AccommodationReservation reservation)
        {
            InitializeComponent();
            DataContext = new GuestReviewFormViewModel(reservation);
        }
    }
}
