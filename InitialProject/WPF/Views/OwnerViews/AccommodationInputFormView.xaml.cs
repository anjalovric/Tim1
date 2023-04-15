using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationInputFormView.xaml
    /// </summary>
    public partial class AccommodationInputFormView : Page
    {
        private AccommodationInputFormViewModel formViewModel;
        private Owner owner;
        public AccommodationInputFormView(Owner owner)
        {
            InitializeComponent();
            this.owner = owner;
            formViewModel = new AccommodationInputFormViewModel(owner);
            DataContext = formViewModel;
        }

        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            formViewModel.EnableCityComboBox();
        }
    }
}
