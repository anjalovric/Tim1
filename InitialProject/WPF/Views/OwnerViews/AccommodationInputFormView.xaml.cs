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
        public AccommodationInputFormView(Owner owner)
        {
            InitializeComponent();
            DataContext = new AccommodationInputFormViewModel(owner);
        }
    }
}
