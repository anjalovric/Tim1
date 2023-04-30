using System.Windows.Controls;
using InitialProject.Domain.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for RenovationDetailsView.xaml
    /// </summary>
    public partial class RenovationDetailsView : Page
    {
        public RenovationDetailsView(AccommodationRenovation renovation)
        {
            InitializeComponent();
            DataContext = new RenovationDetailsViewModel(renovation);
        }
    }
}
