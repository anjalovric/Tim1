using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.Guest1ViewModels;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for AccommodationDetails.xaml
    /// </summary>
    public partial class AccommodationDetailsView : Page
    {
        private AccommodationDetailsViewModel accommodationDetailsViewModel;
        
        public AccommodationDetailsView(Accommodation currentAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            accommodationDetailsViewModel = new AccommodationDetailsViewModel(currentAccommodation, guest1);
            this.DataContext = accommodationDetailsViewModel;
        }
    }
}
