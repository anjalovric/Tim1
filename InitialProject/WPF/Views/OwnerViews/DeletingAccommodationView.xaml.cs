using System.Linq;
using System.Windows;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for DeletingAccommodationView.xaml
    /// </summary>
    public partial class DeletingAccommodationView : Window
    {
        public DeletingAccommodationView(Accommodation accommodation)
        {
            InitializeComponent();
            this.Owner = Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault();
            DataContext = new DeletingAccommodationViewModel(accommodation);
        }
    }
}
