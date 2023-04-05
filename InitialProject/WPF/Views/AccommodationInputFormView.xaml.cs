using System.Windows.Controls;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationInputFormView.xaml
    /// </summary>
    public partial class AccommodationInputFormView : Page
    {
        public AccommodationInputFormView()
        {
            InitializeComponent();
            DataContext = new AccommodationInputFormViewModel();
        }
    }
}
