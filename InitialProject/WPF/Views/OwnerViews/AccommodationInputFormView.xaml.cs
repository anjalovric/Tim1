using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;

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

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AccommodationView accommodationView = new AccommodationView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            formViewModel.SaveAccommodation();
            AccommodationView accommodationView = new AccommodationView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }

        private void AddImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            formViewModel.AddImageFromFileSystem();
        }

        private void RemoveImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
